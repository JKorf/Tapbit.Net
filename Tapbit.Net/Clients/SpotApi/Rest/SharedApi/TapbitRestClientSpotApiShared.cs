using CryptoExchange.Net;
using CryptoExchange.Net.Objects;
using CryptoExchange.Net.Objects.Errors;
using CryptoExchange.Net.SharedApis;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Tapbit.Net.Enums;
using Tapbit.Net.Interfaces.Clients.SpotApi;
using Tapbit.Net.Objects.Models;

namespace Tapbit.Net.Clients.SpotApi
{
    internal partial class TapbitRestClientSpotSharedApi : 
        SharedApiBase,
        ITapbitRestClientSpotSharedApi,
        ITapbitRestClientSpotApiShared
    {
        private readonly TapbitRestClientSpotApi _api;

        private const string _topicId = "TapbitSpot";
        private const string _exchangeName = "Tapbit";

        public override SharedClientInfo Discover() => SharedUtils.GetClientInfo(TapbitExchange.Metadata, this);

        public TapbitRestClientSpotSharedApi(TapbitRestClientSpotApi api)
            : base(
                  SharedTransport.Rest,
                  api.Exchange,
                  [TradingMode.Spot],
                  () => api.Authenticated,
                  api.FormatSymbol)
        {
            _api = api;

            SetCapabilities(
                GetAssetOptions,
                GetAllAssetsOptions,
                GetBalancesOptions,
                GetKlinesOptions,
                GetOrderBookOptions,
                GetRecentTradesOptions,
                GetSpotSymbolsOptions,
                GetSpotTickerOptions,
                GetAllSpotTickersOptions,
                PlaceSpotOrderOptions,
                GetSpotOrderOptions,
                GetOpenSpotOrdersOptions,
                GetClosedSpotOrdersOptions,
                CancelSpotOrderOptions
                );
        }

    }
}
