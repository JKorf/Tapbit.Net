using CryptoExchange.Net.Objects;
using Microsoft.Extensions.Logging;
using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Tapbit.Net.Interfaces.Clients.UsdtPerpetualFuturesApi;
using Tapbit.Net.Objects.Options;
using CryptoExchange.Net.Clients;
using CryptoExchange.Net.Converters.SystemTextJson;
using CryptoExchange.Net.Interfaces;
using CryptoExchange.Net.SharedApis;
using CryptoExchange.Net.Objects.Errors;
using CryptoExchange.Net.Converters.MessageParsing.DynamicConverters;
using Tapbit.Net.Clients.MessageHandlers;

namespace Tapbit.Net.Clients.UsdtPerpetualFuturesApi
{
    /// <inheritdoc cref="ITapbitRestClientUsdtPerpetualFuturesApi" />
    internal partial class TapbitRestClientUsdtPerpetualFuturesApi : RestApiClient<TapbitEnvironment, TapbitAuthenticationProvider, TapbitCredentials>, ITapbitRestClientUsdtPerpetualFuturesApi
    {
        #region fields 
        protected override ErrorMapping ErrorMapping => TapbitErrors.Errors;

        /// <inheritdoc />
        protected override IRestMessageHandler MessageHandler { get; } = new TapbitRestMessageHandler(TapbitErrors.Errors);
        #endregion

        #region Api clients
        /// <inheritdoc />
        public ITapbitRestClientUsdtPerpetualFuturesApiAccount Account { get; }
        /// <inheritdoc />
        public ITapbitRestClientUsdtPerpetualFuturesApiExchangeData ExchangeData { get; }
        /// <inheritdoc />
        public ITapbitRestClientUsdtPerpetualFuturesApiTrading Trading { get; }
        /// <inheritdoc />
        public string ExchangeName => "Tapbit";
        #endregion

        #region constructor/destructor
        internal TapbitRestClientUsdtPerpetualFuturesApi(ILoggerFactory? loggerFactory, HttpClient? httpClient, TapbitRestOptions options)
            : base(loggerFactory, TapbitExchange.Metadata.Id, httpClient, options.Environment.RestClientAddress, options, options.UsdtPerpetualFuturesOptions)
        {
            Account = new TapbitRestClientUsdtPerpetualFuturesApiAccount(this);
            ExchangeData = new TapbitRestClientUsdtPerpetualFuturesApiExchangeData(_logger, this);
            Trading = new TapbitRestClientUsdtPerpetualFuturesApiTrading(_logger, this);
        }
        #endregion

        /// <inheritdoc />
        protected override IMessageSerializer CreateSerializer() => new SystemTextJsonMessageSerializer(TapbitExchange._serializerContext);


        /// <inheritdoc />
        protected override TapbitAuthenticationProvider CreateAuthenticationProvider(TapbitCredentials credentials)
            => new TapbitAuthenticationProvider(credentials);

        internal async Task<HttpResult> SendAsync(RequestDefinition definition, Parameters? parameters, CancellationToken cancellationToken, int? weight = null)
        {
            var result = await base.SendAsync<Unit>(definition, parameters, cancellationToken, null, weight).ConfigureAwait(false);
            return result;
        }

        internal async Task<HttpResult<T>> SendAsync<T>(RequestDefinition definition, Parameters? parameters, CancellationToken cancellationToken, int? weight = null)
        {
            var result = await base.SendAsync<T>(definition, parameters, cancellationToken, null, weight).ConfigureAwait(false);
            return result;
        }

        /// <inheritdoc />
        protected override Task<HttpResult<DateTime>> GetServerTimestampAsync()
            => ExchangeData.GetServerTimeAsync();

        /// <inheritdoc />
        public override string FormatSymbol(string baseAsset, string quoteAsset, TradingMode tradingMode, DateTime? deliverDate = null) 
            => TapbitExchange.FormatSymbol(baseAsset, quoteAsset, tradingMode, deliverDate);

        /// <inheritdoc />
        public ITapbitRestClientUsdtPerpetualFuturesApiShared SharedClient => this;
    }
}
