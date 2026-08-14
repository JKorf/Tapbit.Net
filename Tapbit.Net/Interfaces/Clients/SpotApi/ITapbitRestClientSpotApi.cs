using CryptoExchange.Net.Interfaces.Clients;
using System;

namespace Tapbit.Net.Interfaces.Clients.SpotApi
{
    /// <summary>
    /// Tapbit Spot API endpoints
    /// </summary>
    public interface ITapbitRestClientSpotApi : IRestApiClient<TapbitCredentials>, IDisposable
    {
        /// <summary>
        /// Endpoints related to account settings, info or actions
        /// </summary>
        /// <see cref="ITapbitRestClientSpotApiAccount" />
        public ITapbitRestClientSpotApiAccount Account { get; }

        /// <summary>
        /// Endpoints related to retrieving market and system data
        /// </summary>
        /// <see cref="ITapbitRestClientSpotApiExchangeData" />
        public ITapbitRestClientSpotApiExchangeData ExchangeData { get; }

        /// <summary>
        /// Endpoints related to orders and trades
        /// </summary>
        /// <see cref="ITapbitRestClientSpotApiTrading" />
        public ITapbitRestClientSpotApiTrading Trading { get; }

        /// <summary>
        /// Get the shared rest requests client. This interface is shared with other exchanges to allow for a common implementation for different exchanges.
        /// </summary>
        public ITapbitRestClientSpotApiShared SharedClient { get; }
    }
}
