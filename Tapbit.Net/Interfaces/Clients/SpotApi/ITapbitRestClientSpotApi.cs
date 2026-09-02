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
        /// [V1] Get the shared rest requests client. For new implementations prefer <see cref="SharedApi"/>
        /// </summary>
        public ITapbitRestClientSpotApiShared SharedClient { get; }
        /// <summary>
        /// [V2] Gets the aggregate Shared API interface. Shared APIs provide a common,
        /// exchange-independent contract for accessing functionality across different
        /// exchange client libraries.
        /// </summary>
        public ITapbitRestClientSpotSharedApi SharedApi { get; }
    }
}
