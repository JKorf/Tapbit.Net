using CryptoExchange.Net.SharedApis;

namespace Tapbit.Net.Interfaces.Clients.SpotApi
{
    /// <summary>
    /// Shared interface for Spot rest API usage
    /// </summary>
    public interface ITapbitRestClientSpotApiShared :
        IAssetsRestClient,
        IBalanceRestClient,
        IKlineRestClient,
        IOrderBookRestClient,
        IRecentTradeRestClient,
        ISpotSymbolRestClient,
        ISpotTickerRestClient,
        ISpotOrderRestClient
    {
    }

    /// <summary>
    /// Shared API interface. Shared APIs provide a common,
    /// exchange-independent contract for accessing functionality across different
    /// exchange client libraries.
    /// </summary>
    public interface ITapbitRestClientSpotSharedApi :
        IGetAssetEndpoint,
        IGetAllAssetsEndpoint,
        IGetBalancesEndpoint,
        IGetKlinesEndpoint,
        IGetOrderBookEndpoint,
        IGetRecentTradesEndpoint,
        IGetSpotSymbolsEndpoint,
        IGetSpotTickerEndpoint,
        IGetAllSpotTickersEndpoint,
        IPlaceSpotOrderEndpoint,
        IGetSpotOrderEndpoint,
        IGetOpenSpotOrdersEndpoint,
        IGetClosedSpotOrdersEndpoint,
        ICancelSpotOrderEndpoint
    {

    }
}
