using CryptoExchange.Net.Clients;
using CryptoExchange.Net.Converters.MessageParsing.DynamicConverters;
using CryptoExchange.Net.Converters.SystemTextJson;
using CryptoExchange.Net.Interfaces;
using CryptoExchange.Net.Objects;
using CryptoExchange.Net.Objects.Errors;
using CryptoExchange.Net.SharedApis;
using Microsoft.Extensions.Logging;
using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Tapbit.Net.Clients.MessageHandlers;
using Tapbit.Net.Interfaces.Clients.SpotApi;
using Tapbit.Net.Objects.Models;
using Tapbit.Net.Objects.Options;

namespace Tapbit.Net.Clients.SpotApi
{
    /// <inheritdoc cref="ITapbitRestClientSpotApi" />
    internal partial class TapbitRestClientSpotApi : RestApiClient<TapbitEnvironment, TapbitAuthenticationProvider, TapbitCredentials>, ITapbitRestClientSpotApi
    {
        #region fields 
        private readonly TapbitRestClientSpotSharedApi _sharedApi;

        protected override ErrorMapping ErrorMapping => TapbitErrors.Errors;

        /// <inheritdoc />
        protected override IRestMessageHandler MessageHandler { get; } = new TapbitRestMessageHandler(TapbitErrors.Errors);
        #endregion

        #region Api clients
        /// <inheritdoc />
        public ITapbitRestClientSpotApiAccount Account { get; }
        /// <inheritdoc />
        public ITapbitRestClientSpotApiExchangeData ExchangeData { get; }
        /// <inheritdoc />
        public ITapbitRestClientSpotApiTrading Trading { get; }
        /// <inheritdoc />
        public string ExchangeName => "Tapbit";
        #endregion

        #region constructor/destructor
        internal TapbitRestClientSpotApi(ILoggerFactory? loggerFactory, HttpClient? httpClient, TapbitRestOptions options)
            : base(loggerFactory, TapbitExchange.Metadata.Id, httpClient, options.Environment.RestClientAddress, options, options.SpotOptions)
        {
            Account = new TapbitRestClientSpotApiAccount(this);
            ExchangeData = new TapbitRestClientSpotApiExchangeData(_logger, this);
            Trading = new TapbitRestClientSpotApiTrading(_logger, this);

            _sharedApi = new TapbitRestClientSpotSharedApi(this);
        }
        #endregion

        /// <inheritdoc />
        protected override IMessageSerializer CreateSerializer() => new SystemTextJsonMessageSerializer(TapbitExchange._serializerContext);


        /// <inheritdoc />
        protected override TapbitAuthenticationProvider CreateAuthenticationProvider(TapbitCredentials credentials)
            => new TapbitAuthenticationProvider(credentials);

        internal async Task<HttpResult<T>> SendAsync<T>(RequestDefinition definition, Parameters? parameters, CancellationToken cancellationToken, int? weight = null)
        {
            var result = await base.SendAsync<TapbitResponse<T>>(definition, parameters, cancellationToken, null, weight).ConfigureAwait(false); if (!result.Success)
                return HttpResult.Fail<T>(result);

            return HttpResult.Ok(result, result.Data.Data);
        }

        /// <inheritdoc />
        protected override Task<HttpResult<DateTime>> GetServerTimestampAsync()
            => ExchangeData.GetServerTimeAsync();

        /// <inheritdoc />
        public override string FormatSymbol(string baseAsset, string quoteAsset, TradingMode tradingMode, DateTime? deliverDate = null) 
            => TapbitExchange.FormatSymbol(baseAsset, quoteAsset, tradingMode, deliverDate);

        /// <inheritdoc />
        public ITapbitRestClientSpotApiShared SharedClient => _sharedApi;
        /// <inheritdoc />
        public ITapbitRestClientSpotSharedApi SharedApi => _sharedApi;
    }
}
