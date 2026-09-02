using CryptoExchange.Net.Objects;
using CryptoExchange.Net.Objects.Errors;
using CryptoExchange.Net.RateLimiting.Guards;
using Microsoft.Extensions.Logging;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Tapbit.Net.Enums;
using Tapbit.Net.Interfaces.Clients.SpotApi;
using Tapbit.Net.Objects.Models;

namespace Tapbit.Net.Clients.SpotApi
{
    /// <inheritdoc />
    internal class TapbitRestClientSpotApiTrading : ITapbitRestClientSpotApiTrading
    {
        private static readonly RequestDefinitionCache _definitions = new RequestDefinitionCache();
        private readonly TapbitRestClientSpotApi _baseClient;
        private readonly ILogger _logger;

        internal TapbitRestClientSpotApiTrading(ILogger logger, TapbitRestClientSpotApi baseClient)
        {
            _baseClient = baseClient;
            _logger = logger;
        }

        #region Place Order

        /// <inheritdoc />
        public async Task<HttpResult<TapbitOrderId>> PlaceOrderAsync(
            string symbol,
            OrderSide side,
            decimal quantity,
            decimal price,
            CancellationToken ct = default)
        {
            var parameters = new Parameters(TapbitExchange._parameterSerializationSettings);
            parameters.Add("instrument_id", symbol);
            parameters.Add("direction", side);
            parameters.Add("quantity", quantity);
            parameters.Add("price", price);
            var request = _definitions.GetOrCreate(HttpMethod.Post, _baseClient.BaseAddress, "/spot-v2/api/v1/spot/order", TapbitExchange.RateLimiter.Tapbit, 1, true, 
                limitGuard: new SingleLimitGuard(1, TimeSpan.FromSeconds(1), RateLimitWindowType.Sliding, keySelector: SingleLimitGuard.PerApiKey));
            var result = await _baseClient.SendAsync<TapbitOrderId>(request, parameters, ct).ConfigureAwait(false);
            return result;
        }

        #endregion

        #region Place Multiple Orders

        /// <inheritdoc />
        public async Task<HttpResult<CallResult<TapbitOrderId>[]>> PlaceMultipleOrdersAsync(
            IEnumerable<TapbitOrderRequest> orders,
            CancellationToken ct = default)
        {
            var parameters = new Parameters(orders.ToArray(), TapbitExchange._parameterSerializationSettings);
            var request = _definitions.GetOrCreate(HttpMethod.Post, _baseClient.BaseAddress, "/spot-v2/api/v1/spot/batch_order", TapbitExchange.RateLimiter.Tapbit, 1, true,
                limitGuard: new SingleLimitGuard(1, TimeSpan.FromSeconds(1), RateLimitWindowType.Sliding, keySelector: SingleLimitGuard.PerApiKey));
            var result = await _baseClient.SendAsync<TapbitOrderResult[]>(request, parameters, ct).ConfigureAwait(false);
            if (!result.Success)
                return HttpResult.Fail<CallResult<TapbitOrderId>[]>(result);

            var ordersResult = new List<CallResult<TapbitOrderId>>();
            foreach (var item in result.Data)
            {
                if (item.Code != 200)
                    ordersResult.Add(CallResult.Fail<TapbitOrderId>(new ServerError(item.Code, _baseClient.GetErrorInfo(item.Code, item.Message!))));
                else
                    ordersResult.Add(CallResult.Ok(new TapbitOrderId { OrderId = item.OrderId!.Value }));
            }

            if (ordersResult.All(x => !x.Success))
                return HttpResult.Fail<CallResult<TapbitOrderId>[]>(result, new ServerError(new ErrorInfo(ErrorType.AllOrdersFailed, "All orders failed")), ordersResult.ToArray());

            return HttpResult.Ok(result, ordersResult.ToArray());
        }

        #endregion

        #region Cancel Order

        /// <inheritdoc />
        public async Task<HttpResult<TapbitOrderId>> CancelOrderAsync(long orderId, CancellationToken ct = default)
        {
            var parameters = new Parameters(TapbitExchange._parameterSerializationSettings);
            parameters.Add("order_id", orderId);
            var request = _definitions.GetOrCreate(HttpMethod.Post, _baseClient.BaseAddress, "/spot-v2/api/v1/spot/cancel_order", TapbitExchange.RateLimiter.Tapbit, 1, true,
                limitGuard: new SingleLimitGuard(1, TimeSpan.FromSeconds(1), RateLimitWindowType.Sliding, keySelector: SingleLimitGuard.PerApiKey));
            var result = await _baseClient.SendAsync<TapbitOrderId>(request, parameters, ct).ConfigureAwait(false);
            return result;
        }

        #endregion

        #region Cancel Orders

        /// <inheritdoc />
        public async Task<HttpResult<CallResult<TapbitOrderId>[]>> CancelOrdersAsync(IEnumerable<long> orderIds, CancellationToken ct = default)
        {
            var parameters = new Parameters(TapbitExchange._parameterSerializationSettings);
            parameters.Add("orderIds", orderIds.ToArray());
            var request = _definitions.GetOrCreate(HttpMethod.Post, _baseClient.BaseAddress, "/spot-v2/api/v1/spot/batch_cancel_order", TapbitExchange.RateLimiter.Tapbit, 1, true,
                limitGuard: new SingleLimitGuard(1, TimeSpan.FromSeconds(1), RateLimitWindowType.Sliding, keySelector: SingleLimitGuard.PerApiKey));
            var result = await _baseClient.SendAsync<TapbitOrderResult[]>(request, parameters, ct).ConfigureAwait(false);
            if (!result.Success)
                return HttpResult.Fail<CallResult<TapbitOrderId>[]>(result);

            var ordersResult = new List<CallResult<TapbitOrderId>>();
            foreach (var item in result.Data)
            {
                if (item.Code != 200)
                    ordersResult.Add(CallResult.Fail<TapbitOrderId>(new ServerError(item.Code, _baseClient.GetErrorInfo(item.Code, item.Message!))));
                else
                    ordersResult.Add(CallResult.Ok(new TapbitOrderId { OrderId = item.OrderId!.Value }));
            }

            if (ordersResult.All(x => !x.Success))
                return HttpResult.Fail<CallResult<TapbitOrderId>[]>(result, new ServerError(new ErrorInfo(ErrorType.AllOrdersFailed, "All orders failed")), ordersResult.ToArray());

            return HttpResult.Ok(result, ordersResult.ToArray());
        }

        #endregion

        #region Get Orders

        /// <inheritdoc />
        public async Task<HttpResult<TapbitOrder[]>> GetOpenOrdersAsync(
            string symbol,
            long? fromId = null,
            CancellationToken ct = default)
        {
            var parameters = new Parameters(TapbitExchange._parameterSerializationSettings);
            parameters.Add("instrument_id", symbol);
            parameters.Add("next_order_id", fromId);
            var request = _definitions.GetOrCreate(HttpMethod.Get, _baseClient.BaseAddress, "/spot-v2/api/v1/spot/open_order_list", TapbitExchange.RateLimiter.Tapbit, 1, true, 
                limitGuard: new SingleLimitGuard(1, TimeSpan.FromSeconds(1), RateLimitWindowType.Sliding, keySelector: SingleLimitGuard.PerApiKey));
            var result = await _baseClient.SendAsync<TapbitOrder[]>(request, parameters, ct).ConfigureAwait(false);
            return result;
        }

        #endregion

        #region Get Closed Orders

        /// <inheritdoc />
        public async Task<HttpResult<TapbitOrder[]>> GetClosedOrdersAsync(
            string symbol,
            long? fromId = null,
            CancellationToken ct = default)
        {
            var parameters = new Parameters(TapbitExchange._parameterSerializationSettings);
            parameters.Add("instrument_id", symbol);
            parameters.Add("next_order_id", fromId);
            var request = _definitions.GetOrCreate(HttpMethod.Get, _baseClient.BaseAddress, "/spot-v2/api/v1/spot/closed_order_list", TapbitExchange.RateLimiter.Tapbit, 1, true, 
                limitGuard: new SingleLimitGuard(1, TimeSpan.FromSeconds(1), RateLimitWindowType.Sliding, keySelector: SingleLimitGuard.PerApiKey));
            var result = await _baseClient.SendAsync<TapbitOrder[]>(request, parameters, ct).ConfigureAwait(false);
            return result;
        }

        #endregion

        #region Get Order

        /// <inheritdoc />
        public async Task<HttpResult<TapbitOrder>> GetOrderAsync(long orderId, CancellationToken ct = default)
        {
            var parameters = new Parameters(TapbitExchange._parameterSerializationSettings);
            parameters.Add("order_id", orderId);
            var request = _definitions.GetOrCreate(HttpMethod.Get, _baseClient.BaseAddress, "/spot-v2/api/v1/spot/order_info", TapbitExchange.RateLimiter.Tapbit, 1, true, 
                limitGuard: new SingleLimitGuard(1, TimeSpan.FromSeconds(1), RateLimitWindowType.Sliding, keySelector: SingleLimitGuard.PerApiKey));
            var result = await _baseClient.SendAsync<TapbitOrder>(request, parameters, ct).ConfigureAwait(false);
            return result;
        }

        #endregion

    }
}
