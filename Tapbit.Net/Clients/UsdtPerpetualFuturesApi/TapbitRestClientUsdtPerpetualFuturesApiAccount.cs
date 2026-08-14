using CryptoExchange.Net.Objects;
using Tapbit.Net.Interfaces.Clients.UsdtPerpetualFuturesApi;

namespace Tapbit.Net.Clients.UsdtPerpetualFuturesApi
{
    /// <inheritdoc />
    internal class TapbitRestClientUsdtPerpetualFuturesApiAccount : ITapbitRestClientUsdtPerpetualFuturesApiAccount
    {
        private static readonly RequestDefinitionCache _definitions = new RequestDefinitionCache();
        private readonly TapbitRestClientUsdtPerpetualFuturesApi _baseClient;

        internal TapbitRestClientUsdtPerpetualFuturesApiAccount(TapbitRestClientUsdtPerpetualFuturesApi baseClient)
        {
            _baseClient = baseClient;
        }
    }
}
