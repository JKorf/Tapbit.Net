using System;
using System.Threading;
using System.Threading.Tasks;
using CryptoExchange.Net.Objects;
using Tapbit.Net.Enums;
using Tapbit.Net.Objects.Models;

namespace Tapbit.Net.Interfaces.Clients.SpotApi
{
    /// <summary>
    /// Tapbit Spot exchange data endpoints. Exchange data includes market data (tickers, order books, etc) and system status.
    /// </summary>
    public interface ITapbitRestClientSpotApiExchangeData
    {
        /// <summary>
        /// 
        /// <para><a href="XXX" /></para>
        /// </summary>
        /// <param name="ct">Cancellation token</param>
        /// <returns></returns>
        Task<HttpResult<DateTime>> GetServerTimeAsync(CancellationToken ct = default);

        /// <summary>
        /// Get symbol info
        /// <para>
        /// Docs:<br />
        /// <a href="https://www.tapbit.com/openapi-docs/spot_v2/public/trade_pair_one/" /><br />
        /// Endpoint:<br />
        /// GET /spot-v2/api/spot/instruments/trade_pair_one<br />
        /// </para>
        /// </summary>
        /// <param name="symbol">["<c>instrument_id</c>"] The symbol, for example `ETH/USDT`</param>
        /// <param name="ct">Cancellation token</param>
        Task<HttpResult<TapbitSymbol>> GetSymbolAsync(string symbol, CancellationToken ct = default);

        /// <summary>
        /// Get symbols 
        /// <para>
        /// Docs:<br />
        /// <a href="https://www.tapbit.com/openapi-docs/spot_v2/public/trade_pair_one/" /><br />
        /// Endpoint:<br />
        /// GET /spot-v2/api/spot/instruments/trade_pair_list<br />
        /// </para>
        /// </summary>
        /// <param name="ct">Cancellation token</param>
        Task<HttpResult<TapbitSymbol[]>> GetSymbolsAsync(CancellationToken ct = default);

        /// <summary>
        /// Get order book snapshot
        /// <para>
        /// Docs:<br />
        /// <a href="https://www.tapbit.com/openapi-docs/spot_v2/public/depth/" /><br />
        /// Endpoint:<br />
        /// GET /spot-v2/api/spot/instruments/depth<br />
        /// </para>
        /// </summary>
        /// <param name="symbol">["<c>instrument_id</c>"] The symbol, for example `ETH/USDT`</param>
        /// <param name="depth">["<c>depth</c>"] Book depth, 5, 10, 50 or 100</param>
        /// <param name="ct">Cancellation token</param>
        Task<HttpResult<TapbitOrderBook>> GetOrderBookAsync(
            string symbol,
            int depth,
            CancellationToken ct = default);

        /// <summary>
        /// Get price ticker stats for a symbol
        /// <para>
        /// Docs:<br />
        /// <a href="https://www.tapbit.com/openapi-docs/spot_v2/public/ticker/" /><br />
        /// Endpoint:<br />
        /// GET /spot-v2/api/spot/instruments/ticker_one<br />
        /// </para>
        /// </summary>
        /// <param name="symbol">["<c>instrument_id</c>"] The symbol, for example `ETH/USDT`</param>
        /// <param name="ct">Cancellation token</param>
        Task<HttpResult<TapbitTicker>> GetTickerAsync(string symbol, CancellationToken ct = default);

        /// <summary>
        /// Get price ticker stats for all symbols
        /// <para>
        /// Docs:<br />
        /// <a href="https://www.tapbit.com/openapi-docs/spot_v2/public/ticker_list/" /><br />
        /// Endpoint:<br />
        /// GET /spot-v2/api/spot/instruments/ticker_list<br />
        /// </para>
        /// </summary>
        /// <param name="ct">Cancellation token</param>
        Task<HttpResult<TapbitTicker[]>> GetTickersAsync(CancellationToken ct = default);

        /// <summary>
        /// Get kline/candlestick data
        /// <para>
        /// Docs:<br />
        /// <a href="https://www.tapbit.com/openapi-docs/spot_v2/public/kline/" /><br />
        /// Endpoint:<br />
        /// GET /spot-v2/api/spot/instruments/candles<br />
        /// </para>
        /// </summary>
        /// <param name="symbol">["<c>instrument_id</c>"] The symbol, for example `ETHUSDT`</param>
        /// <param name="interval">["<c>period</c>"] Kline interval</param>
        /// <param name="startTime">["<c>start_time</c>"] Filter by start time</param>
        /// <param name="endTime">["<c>end_time</c>"] Filter by end time</param>
        /// <param name="ct">Cancellation token</param>
        Task<HttpResult<TapbitKline[]>> GetKlinesAsync(
            string symbol,
            KlineInterval interval,
            DateTime? startTime = null,
            DateTime? endTime = null,
            CancellationToken ct = default);

        /// <summary>
        /// Get recent trade list
        /// <para>
        /// Docs:<br />
        /// <a href="https://www.tapbit.com/openapi-docs/spot_v2/public/latest_trade_list/" /><br />
        /// Endpoint:<br />
        /// GET /spot-v2/api/spot/instruments/trade_list<br />
        /// </para>
        /// </summary>
        /// <param name="symbol">["<c>instrument_id</c>"] The symbol, for example `ETH/USDT`</param>
        /// <param name="ct">Cancellation token</param>
        Task<HttpResult<TapbitTrade[]>> GetRecentTradesAsync(string symbol, CancellationToken ct = default);

        /// <summary>
        /// Get assets list
        /// <para>
        /// Docs:<br />
        /// <a href="https://www.tapbit.com/openapi-docs/spot_v2/public/asset_list/" /><br />
        /// Endpoint:<br />
        /// GET /spot-v2/api/spot/instruments/asset/list<br />
        /// </para>
        /// </summary>
        /// <param name="asset">["<c>currency</c>"] Filter by asset, for example `ETH`</param>
        /// <param name="ct">Cancellation token</param>
        Task<HttpResult<TapbitAsset[]>> GetAssetsAsync(string? asset = null, CancellationToken ct = default);

    }
}
