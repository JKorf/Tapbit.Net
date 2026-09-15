using CryptoExchange.Net;
using CryptoExchange.Net.Clients;
using CryptoExchange.Net.Interfaces;
using CryptoExchange.Net.Interfaces.Clients;
using CryptoExchange.Net.SharedApis;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Net.Http;
using System.Threading;
using Tapbit.Net;
using Tapbit.Net.Clients;
using Tapbit.Net.Interfaces;
using Tapbit.Net.Interfaces.Clients;
using Tapbit.Net.Objects.Options;

namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// Extensions for DI
    /// </summary>
    public static class ServiceCollectionExtensions
    {

        /// <summary>
        /// Add services such as the ITapbitRestClient and ITapbitSocketClient. Configures the services based on the provided configuration.
        /// </summary>
        /// <param name="services">The service collection</param>
        /// <param name="configuration">The configuration(section) containing the options</param>
        /// <returns></returns>
        public static IServiceCollection AddTapbit(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            var options = TapbitOptions.CreateFromConfiguration(configuration);

            services.AddSingleton(Options.Options.Create(options.Rest));
            services.AddSingleton(Options.Options.Create(options.Socket));
            services.AddSingleton(Options.Options.Create(options));

            return AddTapbitCore(services, options.SocketClientLifeTime);
        }

        /// <summary>
        /// Add services such as the ITapbitRestClient and ITapbitSocketClient. Services will be configured based on the provided options.
        /// </summary>
        /// <param name="services">The service collection</param>
        /// <param name="optionsDelegate">Set options for the Tapbit services</param>
        /// <returns></returns>
        public static IServiceCollection AddTapbit(
            this IServiceCollection services,
            Action<TapbitOptions>? optionsDelegate = null)
        {
            var options = TapbitOptions.Create(optionsDelegate);

            services.AddSingleton(Options.Options.Create(options.Rest));
            services.AddSingleton(Options.Options.Create(options.Socket));
            services.AddSingleton(Options.Options.Create(options));

            return AddTapbitCore(services, options.SocketClientLifeTime);
        }

        private static IServiceCollection AddTapbitCore(
            this IServiceCollection services,
            ServiceLifetime? socketClientLifeTime = null)
        {
            services.AddHttpClient<ITapbitRestClient, TapbitRestClient>((client, serviceProvider) =>
            {
                var options = serviceProvider.GetRequiredService<IOptions<TapbitRestOptions>>().Value;
                client.Timeout = options.RequestTimeout;
                return new TapbitRestClient(client, serviceProvider.GetRequiredService<ILoggerFactory>(), serviceProvider.GetRequiredService<IOptions<TapbitRestOptions>>());
            }).ConfigurePrimaryHttpMessageHandler((serviceProvider) => {
                var options = serviceProvider.GetRequiredService<IOptions<TapbitRestOptions>>().Value;
                return LibraryHelpers.CreateHttpClientMessageHandler(options);
            }).SetHandlerLifetime(Timeout.InfiniteTimeSpan);

            services.AddTransient<ITrackerFactory, TapbitTrackerFactory>();
            services.AddTransient<ITapbitTrackerFactory, TapbitTrackerFactory>();
            services.AddSingleton<ITapbitUserClientProvider, TapbitUserClientProvider>(x =>
                new TapbitUserClientProvider(
                    x.GetRequiredService<IHttpClientFactory>().CreateClient(typeof(ITapbitRestClient).Name),
                    x.GetRequiredService<ILoggerFactory>(),
                    x.GetRequiredService<IOptions<TapbitRestOptions>>()));

            services.RegisterSharedRestInterfaces(x => x.GetRequiredService<ITapbitRestClient>().SpotApi.SharedClient);

            services.RegisterSharedApiClient<
                ITapbitSharedApiClient,
                TapbitSharedApiClient>(sharedApis => sharedApis
                    .Add(client => client.SpotRest)
                    );

            return services;
        }
    }
}
