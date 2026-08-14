using System;
using System.Threading;
using System.Threading.Tasks;
using CryptoExchange.Net.Objects;

namespace Tapbit.Net.Interfaces.Clients.UsdtPerpetualFuturesApi
{
    /// <summary>
    /// Tapbit UsdtPerpetualFutures exchange data endpoints. Exchange data includes market data (tickers, order books, etc) and system status.
    /// </summary>
    public interface ITapbitRestClientUsdtPerpetualFuturesApiExchangeData
    {
        /// <summary>
        /// 
        /// <para><a href="XXX" /></para>
        /// </summary>
        /// <param name="ct">Cancellation token</param>
        /// <returns></returns>
        Task<HttpResult<DateTime>> GetServerTimeAsync(CancellationToken ct = default);
    }
}
