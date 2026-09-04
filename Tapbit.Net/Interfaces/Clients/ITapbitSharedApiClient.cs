using Tapbit.Net.Interfaces.Clients.SpotApi;

namespace Tapbit.Net.Interfaces.Clients
{
    /// <summary>
    /// Client for the shared REST API implementations of Tapbit
    /// </summary>
    public interface ITapbitSharedApiClient
    {
        /// <summary>
        /// Spot REST shared API implementations
        /// </summary>
        ITapbitRestClientSpotSharedApi SpotRest { get; }
    }
}
