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
    internal partial class TapbitRestClientSpotSharedApi
    {

        #region Place Spot Order

        async Task<ICallResult<SharedId>> IPlaceSpotOrder.PlaceSpotOrderAsync(PlaceSpotOrderRequest request, CancellationToken ct)
            => await PlaceSpotOrderAsync(request, ct).ConfigureAwait(false);

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

        #endregion

        #region Get Spot Order

        async Task<ICallResult<SharedSpotOrder>> IGetSpotOrder.GetSpotOrderAsync(GetOrderRequest request, CancellationToken ct)
            => await GetSpotOrderAsync(request, ct).ConfigureAwait(false);

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

        #endregion

        #region Get Open Spot Orders

        async Task<ICallResult<SharedSpotOrder[]>> IGetOpenSpotOrders.GetOpenSpotOrdersAsync(GetOpenOrdersRequest request, CancellationToken ct)
            => await GetOpenSpotOrdersAsync(request, ct).ConfigureAwait(false);

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

        #endregion

        #region Get Closed Spot Orders

        async Task<ICallResult<SharedSpotOrder[]>> IGetClosedSpotOrders.GetClosedSpotOrdersAsync(GetClosedOrdersRequest request, PageRequest? pageRequest, CancellationToken ct)
            => await GetClosedSpotOrdersAsync(request, pageRequest, ct).ConfigureAwait(false);

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

        #endregion

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

        #region Cancel Spot Order

        async Task<ICallResult<SharedId>> ICancelSpotOrder.CancelSpotOrderAsync(CancelOrderRequest request, CancellationToken ct)
            => await CancelSpotOrderAsync(request, ct).ConfigureAwait(false);

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

        #endregion

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

    }
}
