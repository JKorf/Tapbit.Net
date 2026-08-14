using CryptoExchange.Net.SharedApis;
using Tapbit.Net.Interfaces.Clients.SpotApi;

namespace Tapbit.Net.Clients.SpotApi
{
    internal partial class TapbitSocketClientSpotApi : ITapbitSocketClientSpotApiShared
    {
        private const string _topicId = "TapbitSpot";
        private const string _exchangeName = "Tapbit";

        public TradingMode[] SupportedTradingModes => new[] { TradingMode.Spot };

        public void SetDefaultExchangeParameter(string key, object value) => ExchangeParameters.SetStaticParameter(Exchange, key, value);
        public void ResetDefaultExchangeParameters() => ExchangeParameters.ResetStaticParameters();
        public SharedClientInfo Discover() => SharedUtils.GetClientInfo(TapbitExchange.Metadata, this);
    }
}
