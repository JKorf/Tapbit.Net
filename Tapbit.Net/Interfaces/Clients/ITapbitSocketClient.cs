using CryptoExchange.Net.Interfaces.Clients;
using Tapbit.Net.Interfaces.Clients.SpotApi;
using Tapbit.Net.Interfaces.Clients.UsdtPerpetualFuturesApi;

namespace Tapbit.Net.Interfaces.Clients
{
    /// <summary>
    /// Client for accessing the Tapbit websocket API
    /// </summary>
    public interface ITapbitSocketClient : ISocketClient<TapbitCredentials>
    {
        /// <summary>
        /// UsdtPerpetualFutures API endpoints
        /// </summary>
        /// <see cref="ITapbitSocketClientUsdtPerpetualFuturesApi"/>
        public ITapbitSocketClientUsdtPerpetualFuturesApi UsdtPerpetualFuturesApi { get; }

        /// <summary>
        /// Spot API endpoints
        /// </summary>
        /// <see cref="ITapbitSocketClientSpotApi"/>
        public ITapbitSocketClientSpotApi SpotApi { get; }

    }
}
