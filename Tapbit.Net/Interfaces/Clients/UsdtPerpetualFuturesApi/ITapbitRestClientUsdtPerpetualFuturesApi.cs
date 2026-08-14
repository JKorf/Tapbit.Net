using CryptoExchange.Net.Interfaces.Clients;
using System;

namespace Tapbit.Net.Interfaces.Clients.UsdtPerpetualFuturesApi
{
    /// <summary>
    /// Tapbit UsdtPerpetualFutures API endpoints
    /// </summary>
    public interface ITapbitRestClientUsdtPerpetualFuturesApi : IRestApiClient<TapbitCredentials>, IDisposable
    {
        /// <summary>
        /// Endpoints related to account settings, info or actions
        /// </summary>
        /// <see cref="ITapbitRestClientUsdtPerpetualFuturesApiAccount" />
        public ITapbitRestClientUsdtPerpetualFuturesApiAccount Account { get; }

        /// <summary>
        /// Endpoints related to retrieving market and system data
        /// </summary>
        /// <see cref="ITapbitRestClientUsdtPerpetualFuturesApiExchangeData" />
        public ITapbitRestClientUsdtPerpetualFuturesApiExchangeData ExchangeData { get; }

        /// <summary>
        /// Endpoints related to orders and trades
        /// </summary>
        /// <see cref="ITapbitRestClientUsdtPerpetualFuturesApiTrading" />
        public ITapbitRestClientUsdtPerpetualFuturesApiTrading Trading { get; }

        /// <summary>
        /// Get the shared rest requests client. This interface is shared with other exchanges to allow for a common implementation for different exchanges.
        /// </summary>
        public ITapbitRestClientUsdtPerpetualFuturesApiShared SharedClient { get; }
    }
}
