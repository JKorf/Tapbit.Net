using Tapbit.Net.Interfaces.Clients;
using Tapbit.Net.Interfaces.Clients.SpotApi;

namespace Tapbit.Net.Clients
{
    /// <inheritdoc />
    public class TapbitSharedApiClient : ITapbitSharedApiClient
    {
        /// <inheritdoc />
        public ITapbitRestClientSpotSharedApi SpotRest { get; }

        /// <summary>
        /// ctor
        /// </summary>
        public TapbitSharedApiClient(ITapbitRestClient restClient)
        {
            SpotRest = restClient.SpotApi.SharedApi;
        }
    }
}
