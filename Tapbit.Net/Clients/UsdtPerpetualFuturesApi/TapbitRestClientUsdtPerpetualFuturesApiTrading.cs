using CryptoExchange.Net.Objects;
using Microsoft.Extensions.Logging;
using Tapbit.Net.Interfaces.Clients.UsdtPerpetualFuturesApi;

namespace Tapbit.Net.Clients.UsdtPerpetualFuturesApi
{
    /// <inheritdoc />
    internal class TapbitRestClientUsdtPerpetualFuturesApiTrading : ITapbitRestClientUsdtPerpetualFuturesApiTrading
    {
        private static readonly RequestDefinitionCache _definitions = new RequestDefinitionCache();
        private readonly TapbitRestClientUsdtPerpetualFuturesApi _baseClient;
        private readonly ILogger _logger;

        internal TapbitRestClientUsdtPerpetualFuturesApiTrading(ILogger logger, TapbitRestClientUsdtPerpetualFuturesApi baseClient)
        {
            _baseClient = baseClient;
            _logger = logger;
        }
    }
}
