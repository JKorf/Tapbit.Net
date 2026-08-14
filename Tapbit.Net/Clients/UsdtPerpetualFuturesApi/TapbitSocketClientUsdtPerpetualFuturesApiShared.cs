using CryptoExchange.Net.SharedApis;
using Tapbit.Net.Interfaces.Clients.UsdtPerpetualFuturesApi;

namespace Tapbit.Net.Clients.UsdtPerpetualFuturesApi
{
    internal partial class TapbitSocketClientUsdtPerpetualFuturesApi : ITapbitSocketClientUsdtPerpetualFuturesApiShared
    {
        private const string _topicId = "TapbitUsdtPerpetualFutures";
        private const string _exchangeName = "Tapbit";

        public TradingMode[] SupportedTradingModes => new[] { TradingMode.Spot };

        public void SetDefaultExchangeParameter(string key, object value) => ExchangeParameters.SetStaticParameter(Exchange, key, value);
        public void ResetDefaultExchangeParameters() => ExchangeParameters.ResetStaticParameters();
        public SharedClientInfo Discover() => SharedUtils.GetClientInfo(TapbitExchange.Metadata, this);
    }
}
