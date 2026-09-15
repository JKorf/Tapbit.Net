using CryptoExchange.Net.Authentication;
using CryptoExchange.Net.Objects.Options;
using CryptoExchange.Net.SharedApis;
using Microsoft.Extensions.Configuration;
using System;

namespace Tapbit.Net.Objects.Options
{
    /// <summary>
    /// Tapbit options
    /// </summary>
    public class TapbitOptions : LibraryOptions<TapbitRestOptions, TapbitSocketOptions, TapbitCredentials, TapbitEnvironment>
    {
        /// <summary>
        /// Options for Shared API usage
        /// </summary>
        public SharedApiOptions SharedApi { get; set; } = new();
        /// <summary>
        /// Create TapbitOptions instance using the provided configuration action
        /// </summary>
        public static TapbitOptions Create(Action<TapbitOptions>? configure = null)
        {
            var options = CreateUnconfigured();
            configure?.Invoke(options);
            return Normalize(options);
        }

        /// <summary>
        /// Create TapbitOptions using the provided IConfiguration
        /// </summary>
        public static TapbitOptions CreateFromConfiguration(IConfiguration configuration)
        {
            if (configuration == null)
                throw new ArgumentNullException(nameof(configuration));

            var options = CreateUnconfigured();
            try
            {
                configuration.Bind(options);
            }
            catch (InvalidOperationException ex)
            {
                throw new InvalidOperationException("Invalid Tapbit configuration provided", ex);
            }

            if (options.Environment != null)
                options.Environment = TapbitEnvironment.GetEnvironmentByName(options.Environment.Name) ?? options.Environment;
            if (options.Rest?.Environment != null)
                options.Rest.Environment = TapbitEnvironment.GetEnvironmentByName(options.Rest.Environment.Name) ?? options.Rest.Environment;
            if (options.Socket?.Environment != null)
                options.Socket.Environment = TapbitEnvironment.GetEnvironmentByName(options.Socket.Environment.Name) ?? options.Socket.Environment;

            return Normalize(options);
        }

        private static TapbitOptions CreateUnconfigured()
        {
            var options = new TapbitOptions();
            options.Rest.Environment = null!;
            options.Socket.Environment = null!;
            return options;
        }

        private static TapbitOptions Normalize(TapbitOptions options)
        {
            if (options.Rest == null)
                throw new ArgumentException("REST options cannot be null", nameof(options));
            if (options.Socket == null)
                throw new ArgumentException("Socket options cannot be null", nameof(options));

            options.Rest.Environment ??= options.Environment ?? TapbitEnvironment.Live;
            options.Rest.ApiCredentials ??= options.ApiCredentials;
            options.Socket.Environment ??= options.Environment ?? TapbitEnvironment.Live;
            options.Socket.ApiCredentials ??= options.ApiCredentials;
            return options;
        }
    }
}
