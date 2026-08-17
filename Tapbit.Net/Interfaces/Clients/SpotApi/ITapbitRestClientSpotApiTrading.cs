using CryptoExchange.Net.Objects;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Tapbit.Net.Enums;
using Tapbit.Net.Objects.Models;

namespace Tapbit.Net.Interfaces.Clients.SpotApi
{
    /// <summary>
    /// Tapbit Spot trading endpoints, placing and managing orders.
    /// </summary>
    public interface ITapbitRestClientSpotApiTrading
    {
        /// <summary>
        /// Place a new order
        /// <para>
        /// Docs:<br />
        /// <a href="https://www.tapbit.com/openapi-docs/spot_v2/private/order/" /><br />
        /// Endpoint:<br />
        /// POST /spot-v2/api/v1/spot/order<br />
        /// </para>
        /// </summary>
        /// <param name="symbol">["<c>instrument_id</c>"] The symbol, for example `ETH/USDT`</param>
        /// <param name="side">["<c>direction</c>"] Order side</param>
        /// <param name="quantity">["<c>quantity</c>"] Order quantity</param>
        /// <param name="price">["<c>price</c>"] Limit price</param>
        /// <param name="ct">Cancellation token</param>
        Task<HttpResult<TapbitOrderId>> PlaceOrderAsync(
            string symbol,
            OrderSide side,
            decimal quantity,
            decimal price,
            CancellationToken ct = default);

        /// <summary>
        /// Place multiple new orders 
        /// <para>
        /// Docs:<br />
        /// <a href="https://www.tapbit.com/openapi-docs/spot_v2/private/batch_order/" /><br />
        /// Endpoint:<br />
        /// POST /spot-v2/api/v1/spot/batch_order<br />
        /// </para>
        /// </summary>
        /// <param name="orders">The orders to place</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>The result of the batch order placement</returns>
        Task<HttpResult<CallResult<TapbitOrderId>[]>> PlaceMultipleOrdersAsync(
            IEnumerable<TapbitOrderRequest> orders,
            CancellationToken ct = default);

        /// <summary>
        /// Cancel an active order
        /// <para>
        /// Docs:<br />
        /// <a href="https://www.tapbit.com/openapi-docs/spot_v2/private/cancel_order/" /><br />
        /// Endpoint:<br />
        /// POST /spot-v2/api/v1/spot/cancel_order<br />
        /// </para>
        /// </summary>
        /// <param name="orderId">["<c>order_id</c>"] The id of the order to cancel</param>
        /// <param name="ct">Cancellation token</param>
        Task<HttpResult<TapbitOrderId>> CancelOrderAsync(string orderId, CancellationToken ct = default);

        /// <summary>
        /// Cancel multiple active orders
        /// <para>
        /// Docs:<br />
        /// <a href="https://www.tapbit.com/openapi-docs/spot_v2/private/batch_cancel_order/" /><br />
        /// Endpoint:<br />
        /// POST /spot-v2/api/v1/spot/batch_cancel_order<br />
        /// </para>
        /// </summary>
        /// <param name="orderIds">["<c>orderIds</c>"] The ids of the orders to cancel</param>
        /// <param name="ct">Cancellation token</param>
        Task<HttpResult<CallResult<TapbitOrderId>[]>> CancelOrdersAsync(IEnumerable<string> orderIds, CancellationToken ct = default);

        /// <summary>
        /// Get a list of open orders
        /// <para>
        /// Docs:<br />
        /// <a href="https://www.tapbit.com/openapi-docs/spot_v2/private/open_order_list/" /><br />
        /// Endpoint:<br />
        /// GET /spot-v2/api/v1/spot/open_order_list<br />
        /// </para>
        /// </summary>
        /// <param name="symbol">["<c>instrument_id</c>"] The symbol, for example `ETHUSDT`</param>
        /// <param name="fromId">["<c>next_order_id</c>"] Filter from id</param>
        /// <param name="ct">Cancellation token</param>
        Task<HttpResult<TapbitOrder[]>> GetOpenOrdersAsync(
            string symbol,
            string? fromId = null,
            CancellationToken ct = default);

        /// <summary>
        /// Get closed order history
        /// <para>
        /// Docs:<br />
        /// <a href="https://www.tapbit.com/openapi-docs/spot_v2/private/closed_order_list/" /><br />
        /// Endpoint:<br />
        /// GET /spot-v2/api/v1/spot/closed_order_list<br />
        /// </para>
        /// </summary>
        /// <param name="symbol">["<c>instrument_id</c>"] The symbol, for example `ETH/USDT`</param>
        /// <param name="fromId">["<c>next_order_id</c>"] From id filter</param>
        /// <param name="ct">Cancellation token</param>
        Task<HttpResult<TapbitOrder[]>> GetClosedOrdersAsync(
            string symbol,
            string? fromId = null,
            CancellationToken ct = default);

        /// <summary>
        /// Get order info
        /// <para>
        /// Docs:<br />
        /// <a href="https://www.tapbit.com/openapi-docs/spot_v2/private/order_info/" /><br />
        /// Endpoint:<br />
        /// GET /spot-v2/api/v1/spot/order_info<br />
        /// </para>
        /// </summary>
        /// <param name="orderId">["<c>order_id</c>"] Order id</param>
        /// <param name="ct">Cancellation token</param>
        Task<HttpResult<TapbitOrder>> GetOrderAsync(string orderId, CancellationToken ct = default);

    }
}
