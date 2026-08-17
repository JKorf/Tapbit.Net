using Tapbit.Net.Interfaces.Clients;
using Tapbit.Net.Objects.Options;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Concurrent;
using System.Net.Http;
using CryptoExchange.Net.Clients;

namespace Tapbit.Net.Clients
{
    /// <inheritdoc />
    public class TapbitUserClientProvider : UserClientProvider<
        ITapbitRestClient,
        TapbitRestOptions,
        TapbitCredentials,
        TapbitEnvironment
        >, ITapbitUserClientProvider
    {
        /// <inheritdoc />
        public override string ExchangeName => TapbitExchange.Metadata.Id;

        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="optionsDelegate">Options to use for created clients</param>
        public TapbitUserClientProvider(Action<TapbitOptions>? optionsDelegate = null)
            : this(null, null, Options.Create(ApplyOptionsDelegate(optionsDelegate).Rest))
        {
        }

        /// <summary>
        /// ctor
        /// </summary>
        public TapbitUserClientProvider(
            HttpClient? httpClient,
            ILoggerFactory? loggerFactory,
            IOptions<TapbitRestOptions> restOptions)
            : base(httpClient, loggerFactory, restOptions)
        {
        }

        /// <inheritdoc />
        protected override ITapbitRestClient ConstructRestClient(HttpClient client, ILoggerFactory? loggerFactory, IOptions<TapbitRestOptions> options)
            => new TapbitRestClient(client, loggerFactory, options);
    }
}
