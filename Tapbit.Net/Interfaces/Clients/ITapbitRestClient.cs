using CryptoExchange.Net.Interfaces.Clients;
using Tapbit.Net.Interfaces.Clients.SpotApi;

namespace Tapbit.Net.Interfaces.Clients
{
    /// <summary>
    /// Client for accessing the Tapbit Rest API. 
    /// </summary>
    public interface ITapbitRestClient : IRestClient<TapbitCredentials>
    {
        /// <summary>
        /// Spot API endpoints
        /// </summary>
        /// <see cref="ITapbitRestClientSpotApi"/>
        public ITapbitRestClientSpotApi SpotApi { get; }

    }
}
