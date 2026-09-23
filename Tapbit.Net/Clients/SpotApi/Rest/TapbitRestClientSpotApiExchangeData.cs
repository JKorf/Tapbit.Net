using CryptoExchange.Net.Objects;
using CryptoExchange.Net.Objects.Errors;
using CryptoExchange.Net.RateLimiting.Guards;
using Microsoft.Extensions.Logging;
using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Tapbit.Net.Enums;
using Tapbit.Net.Interfaces.Clients.SpotApi;
using Tapbit.Net.Objects.Models;

namespace Tapbit.Net.Clients.SpotApi
{
    /// <inheritdoc />
    internal class TapbitRestClientSpotApiExchangeData : ITapbitRestClientSpotApiExchangeData
    {
        private readonly TapbitRestClientSpotApi _baseClient;
        private static readonly RequestDefinitionCache _definitions = new RequestDefinitionCache();

        internal TapbitRestClientSpotApiExchangeData(ILogger logger, TapbitRestClientSpotApi baseClient)
        {
            _baseClient = baseClient;
        }

        #region Get Server Time

        /// <inheritdoc />
        public async Task<HttpResult<DateTime>> GetServerTimeAsync(CancellationToken ct = default)
        {
            var request = _definitions.GetOrCreate(HttpMethod.Get, _baseClient.BaseAddress, "/spot-v2/api/spot/instruments/current/timestamp", TapbitExchange.RateLimiter.Tapbit, 1, false,
                limitGuard: new SingleLimitGuard(5, TimeSpan.FromSeconds(1), RateLimitWindowType.Sliding));
            var result = await _baseClient.SendAsync<TapbitServerTime>(request, null, ct).ConfigureAwait(false);
            if (!result.Success)
                return HttpResult.Fail<DateTime>(result);

            return HttpResult.Ok(result, result.Data.Timestamp);
        }

        #endregion

        #region Get Symbol

        /// <inheritdoc />
        public async Task<HttpResult<TapbitSymbol>> GetSymbolAsync(string symbol, CancellationToken ct = default)
        {
            var parameters = new Parameters(TapbitExchange._parameterSerializationSettings);
            parameters.Add("instrument_id", symbol);
            var request = _definitions.GetOrCreate(HttpMethod.Get, _baseClient.BaseAddress, "/spot-v2/api/spot/instruments/trade_pair_one", TapbitExchange.RateLimiter.Tapbit, 1, false,
                limitGuard: new SingleLimitGuard(1, TimeSpan.FromSeconds(1), RateLimitWindowType.Sliding));
            var result = await _baseClient.SendAsync<TapbitSymbol>(request, parameters, ct).ConfigureAwait(false);
            return result;
        }

        #endregion

        #region Get Symbols

        /// <inheritdoc />
        public async Task<HttpResult<TapbitSymbol[]>> GetSymbolsAsync(CancellationToken ct = default)
        {
            var request = _definitions.GetOrCreate(HttpMethod.Get, _baseClient.BaseAddress, "/spot-v2/api/spot/instruments/trade_pair_list", TapbitExchange.RateLimiter.Tapbit, 1, false,
                limitGuard: new SingleLimitGuard(1, TimeSpan.FromSeconds(1), RateLimitWindowType.Sliding));
            var result = await _baseClient.SendAsync<TapbitSymbol[]>(request, null, ct).ConfigureAwait(false);
            return result;
        }

        #endregion

        #region Get Order Book

        /// <inheritdoc />
        public async Task<HttpResult<TapbitOrderBook>> GetOrderBookAsync(
            string symbol,
            int depth,
            CancellationToken ct = default)
        {
            var parameters = new Parameters(TapbitExchange._parameterSerializationSettings);
            parameters.Add("instrument_id", symbol);
            parameters.Add("depth", depth);
            var request = _definitions.GetOrCreate(HttpMethod.Get, _baseClient.BaseAddress, "/spot-v2/api/spot/instruments/depth", TapbitExchange.RateLimiter.Tapbit, 1, false, 
                limitGuard: new SingleLimitGuard(2, TimeSpan.FromSeconds(1), RateLimitWindowType.Sliding));
            var result = await _baseClient.SendAsync<TapbitOrderBook>(request, parameters, ct).ConfigureAwait(false);
            if (!result.Success)
                return result;

            if (result.Data.Asks.Length == 0 && result.Data.Bids.Length == 0)
                return HttpResult.Fail<TapbitOrderBook>(result, new ServerError(ErrorType.UnknownSymbol, "No data for symbol"));

            return result;
        }

        #endregion

        #region Get Ticker

        /// <inheritdoc />
        public async Task<HttpResult<TapbitTicker>> GetTickerAsync(string symbol, CancellationToken ct = default)
        {
            var parameters = new Parameters(TapbitExchange._parameterSerializationSettings);
            parameters.Add("instrument_id", symbol);
            var request = _definitions.GetOrCreate(HttpMethod.Get, _baseClient.BaseAddress, "/spot-v2/api/spot/instruments/ticker_one", TapbitExchange.RateLimiter.Tapbit, 1, false, 
                limitGuard: new SingleLimitGuard(3, TimeSpan.FromSeconds(1), RateLimitWindowType.Sliding));
            var result = await _baseClient.SendAsync<TapbitTicker>(request, parameters, ct).ConfigureAwait(false);
            return result;
        }

        #endregion

        #region Get Tickers

        /// <inheritdoc />
        public async Task<HttpResult<TapbitTicker[]>> GetTickersAsync(CancellationToken ct = default)
        {
            var request = _definitions.GetOrCreate(HttpMethod.Get, _baseClient.BaseAddress, "/spot-v2/api/spot/instruments/ticker_list", TapbitExchange.RateLimiter.Tapbit, 1, false, 
                limitGuard: new SingleLimitGuard(2, TimeSpan.FromSeconds(1), RateLimitWindowType.Sliding));
            var result = await _baseClient.SendAsync<TapbitTicker[]>(request, null, ct).ConfigureAwait(false);
            return result;
        }

        #endregion

        #region Get Klines

        /// <inheritdoc />
        public async Task<HttpResult<TapbitKline[]>> GetKlinesAsync(
            string symbol,
            KlineInterval interval,
            DateTime? startTime = null,
            DateTime? endTime = null,
            CancellationToken ct = default)
        {
            var parameters = new Parameters(TapbitExchange._parameterSerializationSettings);
            parameters.Add("instrument_id", symbol);
            parameters.Add("period", interval);
            parameters.Add("start_time", startTime, DateTimeSerialization.SecondsString);
            parameters.Add("end_time", endTime, DateTimeSerialization.SecondsString);
            var request = _definitions.GetOrCreate(HttpMethod.Get, _baseClient.BaseAddress, "/spot-v2/api/spot/instruments/candles", TapbitExchange.RateLimiter.Tapbit, 1, false, 
                limitGuard: new SingleLimitGuard(4, TimeSpan.FromSeconds(1), RateLimitWindowType.Sliding));
            var result = await _baseClient.SendAsync<TapbitKline[]>(request, parameters, ct).ConfigureAwait(false);
            return result;
        }

        #endregion

        #region Get Recent Trades

        /// <inheritdoc />
        public async Task<HttpResult<TapbitTrade[]>> GetRecentTradesAsync(string symbol, CancellationToken ct = default)
        {
            var parameters = new Parameters(TapbitExchange._parameterSerializationSettings);
            parameters.Add("instrument_id", symbol);
            var request = _definitions.GetOrCreate(HttpMethod.Get, _baseClient.BaseAddress, "/spot-v2/api/spot/instruments/trade_list", TapbitExchange.RateLimiter.Tapbit, 1, false, 
                limitGuard: new SingleLimitGuard(1, TimeSpan.FromSeconds(1), RateLimitWindowType.Sliding));
            var result = await _baseClient.SendAsync<TapbitTrade[]>(request, parameters, ct).ConfigureAwait(false);
            return result;
        }

        #endregion

        #region Get Assets

        /// <inheritdoc />
        public async Task<HttpResult<TapbitAsset[]>> GetAssetsAsync(string? asset = null, CancellationToken ct = default)
        {
            var parameters = new Parameters(TapbitExchange._parameterSerializationSettings);
            parameters.Add("currency", asset);
            var request = _definitions.GetOrCreate(HttpMethod.Get, _baseClient.BaseAddress, "/spot-v2/api/spot/instruments/asset/list", TapbitExchange.RateLimiter.Tapbit, 1, false,
                limitGuard: new SingleLimitGuard(1, TimeSpan.FromSeconds(2), RateLimitWindowType.Sliding));
            var result = await _baseClient.SendAsync<TapbitAsset[]>(request, parameters, ct).ConfigureAwait(false);
            return result;
        }

        #endregion

    }
}
