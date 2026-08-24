// Public ticker lookup, authenticated balances, and a Tapbit spot limit order.
// Setup: dotnet add package Tapbit.Net

using Tapbit.Net;
using Tapbit.Net.Clients;
using Tapbit.Net.Enums;

const string symbol = "BTC/USDT";

var publicClient = new TapbitRestClient();
var ticker = await publicClient.SpotApi.ExchangeData.GetTickerAsync(symbol);
if (!ticker.Success)
{
    Console.WriteLine($"Failed to get ticker: {ticker.Error}");
    return;
}

Console.WriteLine($"{symbol} last price: {ticker.Data.LastPrice}");

var privateClient = new TapbitRestClient(options =>
{
    options.ApiCredentials = new TapbitCredentials("API_KEY", "API_SECRET");
});

var balance = await privateClient.SpotApi.Account.GetBalanceAsync("USDT");
if (!balance.Success)
{
    Console.WriteLine($"Failed to get USDT balance: {balance.Error}");
    return;
}

Console.WriteLine($"USDT available: {balance.Data.Available}");

// Tapbit.Net's current trading surface places limit orders only. Validate the
// symbol rules, minimum quantity, price, and available balance before using real funds.
var order = await privateClient.SpotApi.Trading.PlaceOrderAsync(
    symbol: symbol,
    side: OrderSide.Buy,
    quantity: 0.001m,
    price: 50000m);

if (!order.Success)
{
    Console.WriteLine($"Failed to place order: {order.Error}");
    return;
}

var orderId = order.Data.OrderId;
Console.WriteLine($"Placed order {orderId}");

var status = await privateClient.SpotApi.Trading.GetOrderAsync(orderId);
Console.WriteLine(status.Success
    ? $"Order status: {status.Data.Status}, filled: {status.Data.QuantityFilled}"
    : $"Failed to query order: {status.Error}");

var cancel = await privateClient.SpotApi.Trading.CancelOrderAsync(orderId);
Console.WriteLine(cancel.Success
    ? $"Cancelled order {cancel.Data.OrderId}"
    : $"Failed to cancel order: {cancel.Error}");
