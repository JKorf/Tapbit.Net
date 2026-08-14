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
        ITapbitSocketClient,
        TapbitRestOptions,
        TapbitSocketOptions,
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
            : this(null, null, Options.Create(ApplyOptionsDelegate(optionsDelegate).Rest), Options.Create(ApplyOptionsDelegate(optionsDelegate).Socket))
        {
        }

        /// <summary>
        /// ctor
        /// </summary>
        public TapbitUserClientProvider(
            HttpClient? httpClient,
            ILoggerFactory? loggerFactory,
            IOptions<TapbitRestOptions> restOptions,
            IOptions<TapbitSocketOptions> socketOptions)
            : base(httpClient, loggerFactory, restOptions, socketOptions)
        {
        }

        /// <inheritdoc />
        protected override ITapbitRestClient ConstructRestClient(HttpClient client, ILoggerFactory? loggerFactory, IOptions<TapbitRestOptions> options)
            => new TapbitRestClient(client, loggerFactory, options);
        /// <inheritdoc />
        protected override ITapbitSocketClient ConstructSocketClient(ILoggerFactory? loggerFactory, IOptions<TapbitSocketOptions> options)
            => new TapbitSocketClient(options, loggerFactory);
    }
}
