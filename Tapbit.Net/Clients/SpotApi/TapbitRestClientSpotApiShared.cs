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
    internal class TapbitRestClientSpotSharedApi : 
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

        #region Asset client
        Task<HttpResult<SharedAsset[]>> IAssetsRestClient.GetAssetsAsync(GetAssetsRequest request, CancellationToken ct)
            => GetAllAssetsAsync(request, ct);
        GetAllAssetsOptions IAssetsRestClient.GetAssetsOptions => GetAllAssetsOptions;

        public GetAllAssetsOptions GetAllAssetsOptions { get; } = new GetAllAssetsOptions(_exchangeName, false);

        public async Task<HttpResult<SharedAsset[]>> GetAllAssetsAsync(GetAssetsRequest request, CancellationToken ct)
        {
            var validationError = GetAllAssetsOptions.ValidateRequest(request, this);
            if (validationError != null)
                return HttpResult.Fail<SharedAsset[]>(Exchange, validationError);

            var assets = await _api.ExchangeData.GetAssetsAsync(ct: ct).ConfigureAwait(false);
            if (!assets.Success)
                return HttpResult.Fail<SharedAsset[]>(assets);

            return HttpResult.Ok(assets, assets.Data.Select(x => new SharedAsset(x.Asset)
            {
                FullName = x.FullName,
                Networks = x.Networks.Select(x => new SharedAssetNetwork(x.Network)
                {
                    DepositEnabled = x.IsDepositEnabled,
                    MinWithdrawQuantity = x.WithdrawLimitMin,
                    WithdrawEnabled = x.IsWithdrawEnabled,
                    WithdrawFee = x.Fee,
                    MinConfirmations = x.DepositMinConfirm
                }).ToArray()
            }).ToArray());
        }

        public GetAssetOptions GetAssetOptions { get; } = new GetAssetOptions(_exchangeName, false);
        public async Task<HttpResult<SharedAsset>> GetAssetAsync(GetAssetRequest request, CancellationToken ct)
        {
            var validationError = GetAssetOptions.ValidateRequest(request, this);
            if (validationError != null)
                return HttpResult.Fail<SharedAsset>(Exchange, validationError);

            var assets = await _api.ExchangeData.GetAssetsAsync(ct: ct).ConfigureAwait(false);
            if (!assets.Success)
                return HttpResult.Fail<SharedAsset>(assets);

            var asset = assets.Data.SingleOrDefault(x => x.Asset.Equals(request.Asset, StringComparison.InvariantCultureIgnoreCase));
            if (asset == null)
                return HttpResult.Fail<SharedAsset>(assets, new ServerError(new ErrorInfo(ErrorType.UnknownAsset, "Asset not found")));

            return HttpResult.Ok(assets, new SharedAsset(asset.Asset)
            {
                FullName = asset.FullName,
                Networks = asset.Networks.Select(x => new SharedAssetNetwork(x.Network)
                {
                    DepositEnabled = x.IsDepositEnabled,
                    MinWithdrawQuantity = x.WithdrawLimitMin,
                    WithdrawEnabled = x.IsWithdrawEnabled,
                    WithdrawFee = x.Fee,
                    MinConfirmations = x.DepositMinConfirm
                }).ToArray()
            });
        }

        #endregion

        #region Balance Client
        public GetBalancesOptions GetBalancesOptions { get; } = new GetBalancesOptions(_exchangeName, AccountTypeFilter.Spot);

        public async Task<HttpResult<SharedBalance[]>> GetBalancesAsync(GetBalancesRequest request, CancellationToken ct)
        {
            var validationError = GetBalancesOptions.ValidateRequest(request, this);
            if (validationError != null)
                return HttpResult.Fail<SharedBalance[]>(Exchange, validationError);

            var result = await _api.Account.GetBalancesAsync(ct: ct).ConfigureAwait(false);
            if (!result.Success)
                return HttpResult.Fail<SharedBalance[]>(result);

            return HttpResult.Ok(result, result.Data.Select(x =>
                new SharedBalance(SupportedTradingModes, x.Asset, x.Available, x.TotalBalance)).ToArray());
        }

        #endregion

        #region Klines Client

        public GetKlinesOptions GetKlinesOptions { get; } = new GetKlinesOptions(_exchangeName, false, true, true, 200, false);

        public async Task<HttpResult<SharedKline[]>> GetKlinesAsync(GetKlinesRequest request, PageRequest? pageRequest, CancellationToken ct)
        {
            var interval = (Enums.KlineInterval)request.Interval;
            var validationError = GetKlinesOptions.ValidateRequest(request, this);
            if (validationError != null)
                return HttpResult.Fail<SharedKline[]>(Exchange, validationError);

            int limit = request.Limit ?? 200;
            var direction = DataDirection.Descending;
            var pageParams = Pagination.GetPaginationParameters(direction, limit, request.StartTime, request.EndTime ?? DateTime.UtcNow, pageRequest, true);

            // Get data
            var symbol = request.Symbol!.GetSymbol(FormatSymbol);
            var result = await _api.ExchangeData.GetKlinesAsync(
                symbol,
                interval,
                pageParams.StartTime,
                pageParams.EndTime,
                ct: ct
                ).ConfigureAwait(false);
            if (!result.Success)
                return HttpResult.Fail<SharedKline[]>(result);

            var nextPageRequest = Pagination.GetNextPageRequest(
                     () => Pagination.NextPageFromTime(pageParams, result.Data.Min(x => x.OpenTime).AddSeconds(-1)),
                     result.Data.Length,
                     result.Data.Select(x => x.OpenTime),
                     request.StartTime,
                     request.EndTime ?? DateTime.UtcNow,
                     pageParams);

            return HttpResult.Ok(result, ExchangeHelpers.ApplyFilter(result.Data, x => x.OpenTime, request.StartTime, request.EndTime, direction)
                    .Take(limit)
                    .Select(x =>
                        new SharedKline(
                            request.Symbol,
                            symbol,
                            x.OpenTime,
                            x.ClosePrice,
                            x.HighPrice,
                            x.LowPrice,
                            x.OpenPrice,
                            new SharedOrderQuantity(x.Volume)))
                    .ToArray(), nextPageRequest);
        }

        #endregion

        #region Order Book client
        public GetOrderBookOptions GetOrderBookOptions { get; } = new GetOrderBookOptions(_exchangeName, new[] { 5, 10, 50, 100 }, false);
        public async Task<HttpResult<SharedOrderBook>> GetOrderBookAsync(GetOrderBookRequest request, CancellationToken ct)
        {
            var validationError = GetOrderBookOptions.ValidateRequest(request, this);
            if (validationError != null)
                return HttpResult.Fail<SharedOrderBook>(Exchange, validationError);

            var result = await _api.ExchangeData.GetOrderBookAsync(
                request.Symbol!.GetSymbol(FormatSymbol),
                depth: request.Limit ?? 10,
                ct: ct).ConfigureAwait(false);
            if (!result.Success)
                return HttpResult.Fail<SharedOrderBook>(result);

            return HttpResult.Ok(result, new SharedOrderBook(SharedQuantityType.BaseAsset, null, result.Data.Asks, result.Data.Bids));
        }

        #endregion

        #region Recent Trades client
        public GetRecentTradesOptions GetRecentTradesOptions { get; } = new GetRecentTradesOptions(_exchangeName, 50, false);

        public async Task<HttpResult<SharedTrade[]>> GetRecentTradesAsync(GetRecentTradesRequest request, CancellationToken ct)
        {
            var validationError = GetRecentTradesOptions.ValidateRequest(request, this);
            if (validationError != null)
                return HttpResult.Fail<SharedTrade[]>(Exchange, validationError);

            // Get data
            var symbol = request.Symbol!.GetSymbol(FormatSymbol);
            var result = await _api.ExchangeData.GetRecentTradesAsync(
                symbol,
                ct: ct).ConfigureAwait(false);
            if (!result.Success)
                return HttpResult.Fail<SharedTrade[]>(result);

            // Return
            return HttpResult.Ok(result, result.Data.Select(x =>
                new SharedTrade(request.Symbol, symbol, new SharedOrderQuantity(x.Quantity), x.Price, x.Timestamp)
                {
                    Side = x.Side == Enums.OrderSide.Sell ? SharedOrderSide.Sell : SharedOrderSide.Buy,
                }).ToArray());
        }
        #endregion

        #region Spot Symbol client
        public SharedSymbolCatalog? SpotSymbolCatalog => ExchangeSymbolCache.GetSymbolCatalog(_exchangeName, _topicId, _api.EnvironmentName, null);
        public GetSpotSymbolsOptions GetSpotSymbolsOptions { get; } = new GetSpotSymbolsOptions(_exchangeName, false);

        public async Task<HttpResult<SharedSpotSymbol[]>> GetSpotSymbolsAsync(GetSymbolsRequest request, CancellationToken ct)
        {
            var validationError = GetSpotSymbolsOptions.ValidateRequest(request, this);
            if (validationError != null)
                return HttpResult.Fail<SharedSpotSymbol[]>(Exchange, validationError);

            var result = await _api.ExchangeData.GetSymbolsAsync(ct: ct).ConfigureAwait(false);
            if (!result.Success)
                return HttpResult.Fail<SharedSpotSymbol[]>(result);

            var resultData =
                 result.Data.Select(x => ParseSymbol(x))
                .ToArray();

            ExchangeSymbolCache.UpdateSymbolInfo(_topicId, _api.EnvironmentName, null, resultData);
            return HttpResult.Ok(result, SharedUtils.ApplySymbolFilter(resultData, request));
        }

        private SharedSpotSymbol ParseSymbol(TapbitSymbol s)
        {
            var result = new SharedSpotSymbol(s.BaseAsset, s.QuoteAsset, s.Symbol, true)
            {
                MinTradeQuantity = s.MinQuantity,
                MinNotionalValue = s.MinNotional,
                QuantityDecimals = s.QuantityPrecision,
                PriceDecimals = s.PricePrecision,
                QuoteAssetType = SharedAssetType.Crypto,
                QuoteAssetSubType = SharedAssetSubType.StableCoin,
                MakerFeePercentage = s.MakerFeeRate * 100,
                TakerFeePercentage = s.TakerFeeRate * 100,
                LowerPriceLimitPercentage = -s.PriceFluctuation * 100,
                UpperPriceLimitPercentage = s.PriceFluctuation * 100
            };

            result.BaseAssetType = SharedAssetType.Crypto;
            if (LibraryHelpers.IsStableCoin(s.BaseAsset))
                result.BaseAssetSubType = SharedAssetSubType.StableCoin;

            return result;
        }

        public async Task<ExchangeCallResult<SharedSymbol[]>> GetSpotSymbolsForBaseAssetAsync(string baseAsset)
        {
            if (!ExchangeSymbolCache.HasCached(_topicId, _api.EnvironmentName, null))
            {
                var symbols = await GetSpotSymbolsAsync(new GetSymbolsRequest(), default).ConfigureAwait(false);
                if (!symbols.Success)
                    return ExchangeCallResult<SharedSymbol[]>.Fail(Exchange, symbols.Error!);
            }

            return ExchangeCallResult<SharedSymbol[]>.Ok(Exchange, ExchangeSymbolCache.GetSymbolsForBaseAsset(_topicId, _api.EnvironmentName, null, baseAsset));
        }

        public async Task<ExchangeCallResult<bool>> SupportsSpotSymbolAsync(SharedSymbol symbol)
        {
            if (symbol.TradingMode != TradingMode.Spot)
                throw new ArgumentException(nameof(symbol), "Only Spot symbols allowed");

            if (!ExchangeSymbolCache.HasCached(_topicId, _api.EnvironmentName, null))
            {
                var symbols = await GetSpotSymbolsAsync(new GetSymbolsRequest(), default).ConfigureAwait(false);
                if (!symbols.Success)
                    return ExchangeCallResult<bool>.Fail(Exchange, symbols.Error!);
            }

            return ExchangeCallResult<bool>.Ok(Exchange, ExchangeSymbolCache.SupportsSymbol(_topicId, _api.EnvironmentName, null, symbol));
        }

        public async Task<ExchangeCallResult<bool>> SupportsSpotSymbolAsync(string symbolName)
        {
            if (!ExchangeSymbolCache.HasCached(_topicId, _api.EnvironmentName, null))
            {
                var symbols = await GetSpotSymbolsAsync(new GetSymbolsRequest(), default).ConfigureAwait(false);
                if (!symbols.Success)
                    return ExchangeCallResult<bool>.Fail(Exchange, symbols.Error!);
            }

            return ExchangeCallResult<bool>.Ok(Exchange, ExchangeSymbolCache.SupportsSymbol(_topicId, _api.EnvironmentName, null, symbolName));
        }
        #endregion

        #region Ticker client

        public GetSpotTickerOptions GetSpotTickerOptions { get; } = new GetSpotTickerOptions(_exchangeName);
        public async Task<HttpResult<SharedSpotTicker>> GetSpotTickerAsync(GetTickerRequest request, CancellationToken ct)
        {
            var validationError = GetSpotTickerOptions.ValidateRequest(request, this);
            if (validationError != null)
                return HttpResult.Fail<SharedSpotTicker>(Exchange, validationError);

            var result = await _api.ExchangeData.GetTickerAsync(request.Symbol!.GetSymbol(FormatSymbol), ct).ConfigureAwait(false);
            if (!result.Success)
                return HttpResult.Fail<SharedSpotTicker>(result);

            return HttpResult.Ok(result,
                new SharedSpotTicker(
                    ExchangeSymbolCache.ParseSymbol(_topicId, _api.EnvironmentName, null, result.Data.Symbol),
                    result.Data.Symbol,
                    result.Data.LastPrice,
                    result.Data.HighPrice24h,
                    result.Data.LowPrice24h,
                    new SharedOrderQuantity(result.Data.Volume24h, result.Data.QuoteVolume24h),
                    result.Data.Change24h == 0 ? null : result.Data.Change24h * 100) // change is incorrectly 0 for some symbols
                {
                });
        }

        Task<HttpResult<SharedSpotTicker[]>> ISpotTickerRestClient.GetSpotTickersAsync(GetTickersRequest request, CancellationToken ct)
            => GetAllSpotTickersAsync(request, ct);
        GetAllSpotTickersOptions ISpotTickerRestClient.GetSpotTickersOptions => GetAllSpotTickersOptions;

        public GetAllSpotTickersOptions GetAllSpotTickersOptions { get; } = new GetAllSpotTickersOptions(_exchangeName);
        public async Task<HttpResult<SharedSpotTicker[]>> GetAllSpotTickersAsync(GetTickersRequest request, CancellationToken ct)
        {
            var validationError = GetAllSpotTickersOptions.ValidateRequest(request, this);
            if (validationError != null)
                return HttpResult.Fail<SharedSpotTicker[]>(Exchange, validationError);

            var result = await _api.ExchangeData.GetTickersAsync(ct: ct).ConfigureAwait(false);
            if (!result.Success)
                return HttpResult.Fail<SharedSpotTicker[]>(result);

            return HttpResult.Ok(result, result.Data.Select(x =>
                new SharedSpotTicker(
                    ExchangeSymbolCache.ParseSymbol(_topicId, _api.EnvironmentName, null, x.Symbol),
                    x.Symbol,
                    x.LastPrice,
                    x.HighPrice24h,
                    x.LowPrice24h,
                    new SharedOrderQuantity(x.Volume24h, x.QuoteVolume24h),
                    x.Change24h == 0 ? null : x.Change24h * 100) // change is incorrectly 0 for some symbols
                {
                }).ToArray());
        }

        #endregion

        #region Spot Order Client

        public SharedFeeDeductionType SpotFeeDeductionType => SharedFeeDeductionType.DeductFromOutput;
        public SharedFeeAssetType SpotFeeAssetType => SharedFeeAssetType.QuoteAsset;
        public SharedOrderType[] SpotSupportedOrderTypes { get; } = new[] { SharedOrderType.Limit };
        public SharedTimeInForce[] SpotSupportedTimeInForce { get; } = new[] { SharedTimeInForce.GoodTillCanceled };
        public SharedQuantitySupport SpotSupportedOrderQuantity { get; } = new SharedQuantitySupport(
                SharedQuantityType.BaseAsset,
                SharedQuantityType.BaseAsset,
                SharedQuantityType.QuoteAsset,
                SharedQuantityType.BaseAsset);

        public string GenerateClientOrderId() => ExchangeHelpers.RandomString(20);

        public PlaceSpotOrderOptions PlaceSpotOrderOptions { get; } = new PlaceSpotOrderOptions(_exchangeName);
        public async Task<HttpResult<SharedId>> PlaceSpotOrderAsync(PlaceSpotOrderRequest request, CancellationToken ct)
        {
            var validationError = PlaceSpotOrderOptions.ValidateRequest(request, this);
            if (validationError != null)
                return HttpResult.Fail<SharedId>(Exchange, validationError);

            var result = await _api.Trading.PlaceOrderAsync(
                request.Symbol!.GetSymbol(FormatSymbol),
                request.Side == SharedOrderSide.Buy ? Enums.OrderSide.Buy : Enums.OrderSide.Sell,
                quantity: request.Quantity?.QuantityInBaseAsset ?? 0,
                price: request.Price ?? 0,
                ct: ct).ConfigureAwait(false);

            if (!result.Success)
                return HttpResult.Fail<SharedId>(result);

            return HttpResult.Ok(result, new SharedId(result.Data.OrderId.ToString()));
        }

        public GetSpotOrderOptions GetSpotOrderOptions { get; } = new GetSpotOrderOptions(_exchangeName, true);
        public async Task<HttpResult<SharedSpotOrder>> GetSpotOrderAsync(GetOrderRequest request, CancellationToken ct)
        {
            var validationError = GetSpotOrderOptions.ValidateRequest(request, this);
            if (validationError != null)
                return HttpResult.Fail<SharedSpotOrder>(Exchange, validationError);

            if (!long.TryParse(request.OrderId, out var orderId))
                return HttpResult.Fail<SharedSpotOrder>(Exchange, new ServerError(new ErrorInfo(ErrorType.InvalidParameter, "Invalid OrderId")));

            var order = await _api.Trading.GetOrderAsync(orderId, ct: ct).ConfigureAwait(false);
            if (!order.Success)
                return HttpResult.Fail<SharedSpotOrder>(order);

            return HttpResult.Ok(order, new SharedSpotOrder(
                ExchangeSymbolCache.ParseSymbol(_topicId, _api.EnvironmentName, null, order.Data.Symbol),
                order.Data.Symbol,
                order.Data.OrderId.ToString(),
                order.Data.OrderType == OrderType.Market ? SharedOrderType.Market : SharedOrderType.Limit,
                order.Data.Side == OrderSide.Buy ? SharedOrderSide.Buy : SharedOrderSide.Sell,
                ParseOrderStatus(order.Data.Status),
                order.Data.Timestamp)
            {
                OrderPrice = order.Data.Price,
#pragma warning disable CS0618 // Type or member is obsolete
                Fee = order.Data.Fee,
#pragma warning restore CS0618 // Type or member is obsolete
                OrderQuantity = new SharedOrderQuantity(order.Data.Quantity > 0 ? order.Data.Quantity : null, order.Data.QuoteQuantity > 0 ? order.Data.QuoteQuantity : null),
                QuantityFilled = new SharedOrderQuantity(order.Data.QuantityFilled, order.Data.QuoteQuantityFilled),
            });
        }

        public GetOpenSpotOrdersOptions GetOpenSpotOrdersOptions { get; } = new GetOpenSpotOrdersOptions(_exchangeName, true)
        {
            RequiredRequestParameters = [
                RequestParameter<GetOpenOrdersRequest>.Required(x => x.Symbol, "Symbol to request open orders for", new SharedSymbol(TradingMode.Spot, "ETH", "USDT"))
                ]
        };
        public async Task<HttpResult<SharedSpotOrder[]>> GetOpenSpotOrdersAsync(GetOpenOrdersRequest request, CancellationToken ct)
        {
            var validationError = GetOpenSpotOrdersOptions.ValidateRequest(request, this);
            if (validationError != null)
                return HttpResult.Fail<SharedSpotOrder[]>(Exchange, validationError);

            var symbol = request.Symbol!.GetSymbol(FormatSymbol);
            var orders = await _api.Trading.GetOpenOrdersAsync(symbol: symbol, ct: ct).ConfigureAwait(false);
            if (!orders.Success)
                return HttpResult.Fail<SharedSpotOrder[]>(orders);

            return HttpResult.Ok(orders, orders.Data.Select(x => new SharedSpotOrder(
                ExchangeSymbolCache.ParseSymbol(_topicId, _api.EnvironmentName, null, x.Symbol),
                x.Symbol,
                x.OrderId.ToString(),
                x.OrderType == OrderType.Market ? SharedOrderType.Market : SharedOrderType.Limit,
                x.Side == OrderSide.Buy ? SharedOrderSide.Buy : SharedOrderSide.Sell,
                ParseOrderStatus(x.Status),
                x.Timestamp)
            {
                OrderPrice = x.Price,
#pragma warning disable CS0618 // Type or member is obsolete
                Fee = x.Fee,
#pragma warning restore CS0618 // Type or member is obsolete
                OrderQuantity = new SharedOrderQuantity(x.Quantity > 0 ? x.Quantity : null, x.QuoteQuantity > 0 ? x.QuoteQuantity : null),
                QuantityFilled = new SharedOrderQuantity(x.QuantityFilled, x.QuoteQuantityFilled),
            }).ToArray());
        }

        public GetSpotClosedOrdersOptions GetClosedSpotOrdersOptions { get; } = new GetSpotClosedOrdersOptions(_exchangeName, false, true, false, 20);
        public async Task<HttpResult<SharedSpotOrder[]>> GetClosedSpotOrdersAsync(GetClosedOrdersRequest request, PageRequest? pageRequest, CancellationToken ct)
        {
            var validationError = GetClosedSpotOrdersOptions.ValidateRequest(request, this);
            if (validationError != null)
                return HttpResult.Fail<SharedSpotOrder[]>(Exchange, validationError);

            int limit = request.Limit ?? 20;
            var direction = DataDirection.Descending;
            var pageParams = Pagination.GetPaginationParameters(direction, limit, request.StartTime, request.EndTime ?? DateTime.UtcNow, pageRequest);

            // Get data
            var result = await _api.Trading.GetClosedOrdersAsync(symbol: request.Symbol!.GetSymbol(FormatSymbol),
                fromId: pageParams.FromId != null ? long.Parse(pageParams.FromId) : null,
                ct: ct).ConfigureAwait(false);
            if (!result.Success)
                return HttpResult.Fail<SharedSpotOrder[]>(result);

            var nextPageRequest = Pagination.GetNextPageRequest(
                         () => Pagination.NextPageFromId(result.Data.Min(x => x.OrderId) - 1),
                         result.Data.Length,
                         result.Data.Select(x => x.Timestamp),
                         request.StartTime,
                         request.EndTime ?? DateTime.UtcNow,
                         pageParams);

            return HttpResult.Ok(result, ExchangeHelpers.ApplyFilter(result.Data, x => x.Timestamp, request.StartTime, request.EndTime, direction)
                .Take(limit)
                .Select(x =>
                    new SharedSpotOrder(
                        ExchangeSymbolCache.ParseSymbol(_topicId, _api.EnvironmentName, null, x.Symbol),
                        x.Symbol,
                        x.OrderId.ToString(),
                        x.OrderType == OrderType.Market ? SharedOrderType.Market : SharedOrderType.Limit,
                        x.Side == OrderSide.Buy ? SharedOrderSide.Buy : SharedOrderSide.Sell,
                        ParseOrderStatus(x.Status),
                        x.Timestamp)
                    {
                         OrderPrice = x.Price,
#pragma warning disable CS0618 // Type or member is obsolete
                         Fee = x.Fee,
#pragma warning restore CS0618 // Type or member is obsolete
                         OrderQuantity = new SharedOrderQuantity(x.Quantity > 0 ? x.Quantity : null, x.QuoteQuantity > 0 ? x.QuoteQuantity : null),
                         QuantityFilled = new SharedOrderQuantity(x.QuantityFilled, x.QuoteQuantityFilled),
                    })
                .ToArray(), nextPageRequest);
        }

        public GetSpotOrderTradesOptions GetSpotOrderTradesOptions { get; } = new GetSpotOrderTradesOptions(_exchangeName, true)
        {
            Supported = false,
            RequestNotes = "There is no way to retrieve user trades in the Tapbit API"
        };
        public Task<HttpResult<SharedUserTrade[]>> GetSpotOrderTradesAsync(GetOrderTradesRequest request, CancellationToken ct)
        {
            return Task.FromResult(HttpResult.Fail<SharedUserTrade[]>(Exchange, new InvalidOperationError($"Method not available for {Exchange}")));
        }

        public GetSpotUserTradeHistoryOptions GetSpotUserTradesOptions { get; } = new GetSpotUserTradeHistoryOptions(_exchangeName, false, false, false, 1000)
        {
            Supported = false,
            RequestNotes = "There is no way to retrieve user trades in the Tapbit API"
        };
        public Task<HttpResult<SharedUserTrade[]>> GetSpotUserTradesAsync(GetUserTradesRequest request, PageRequest? pageRequest, CancellationToken ct)
        {
            return Task.FromResult(HttpResult.Fail<SharedUserTrade[]>(Exchange, new InvalidOperationError($"Method not available for {Exchange}")));
        }

        public CancelSpotOrderOptions CancelSpotOrderOptions { get; } = new CancelSpotOrderOptions(_exchangeName, true);
        public async Task<HttpResult<SharedId>> CancelSpotOrderAsync(CancelOrderRequest request, CancellationToken ct)
        {
            var validationError = CancelSpotOrderOptions.ValidateRequest(request, this);
            if (validationError != null)
                return HttpResult.Fail<SharedId>(Exchange, validationError);

            if (!long.TryParse(request.OrderId, out var orderId))
                return HttpResult.Fail<SharedId>(Exchange, new ServerError(new ErrorInfo(ErrorType.InvalidParameter, "Invalid OrderId")));

            var order = await _api.Trading.CancelOrderAsync(orderId, ct: ct).ConfigureAwait(false);
            if (!order.Success)
                return HttpResult.Fail<SharedId>(order);

            return HttpResult.Ok(order, new SharedId(order.Data.OrderId.ToString()));
        }

        private SharedOrderStatus ParseOrderStatus(OrderStatus status)
        {
            if (status == Enums.OrderStatus.Canceled || status == Enums.OrderStatus.PartiallyCanceled)
                return SharedOrderStatus.Canceled;
            if (status == Enums.OrderStatus.Open)
                return SharedOrderStatus.Open;
            if (status == OrderStatus.Filled)
                return SharedOrderStatus.Filled;

            return SharedOrderStatus.Unknown;
        }

        #endregion
    }
}
