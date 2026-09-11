using CryptoExchange.Net.SharedApis;
using Microsoft.Extensions.Options;
using Tapbit.Net.Interfaces.Clients;
using Tapbit.Net.Interfaces.Clients.SpotApi;
using Tapbit.Net.Objects.Options;

namespace Tapbit.Net.Clients
{
    /// <inheritdoc />
    public class TapbitSharedApiClient : SharedApiClientBase, ITapbitSharedApiClient
    {
        /// <inheritdoc />
        public ITapbitRestClientSpotSharedApi SpotRest { get; }

        /// <summary>
        /// ctor
        /// </summary>
        public TapbitSharedApiClient(ITapbitRestClient restClient,
            IOptions<TapbitOptions> options)
            : base(options.Value.SharedApi.PreferredTransport,
                restClient.SpotApi.SharedApi)
        {
            SpotRest = restClient.SpotApi.SharedApi;
        }
    }
}
