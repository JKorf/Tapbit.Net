// Authenticated Tapbit spot batch order placement and cancellation.
// Setup: dotnet add package Tapbit.Net

using Tapbit.Net;
using Tapbit.Net.Clients;
using Tapbit.Net.Enums;
using Tapbit.Net.Objects.Models;

var client = new TapbitRestClient(options =>
{
    options.ApiCredentials = new TapbitCredentials("API_KEY", "API_SECRET");
});

var requests = new[]
{
    new TapbitOrderRequest
    {
        Symbol = "BTC/USDT",
        Side = OrderSide.Buy,
        Quantity = 0.001m,
        Price = 50000m
    },
    new TapbitOrderRequest
    {
        Symbol = "ETH/USDT",
        Side = OrderSide.Buy,
        Quantity = 0.01m,
        Price = 2000m
    }
};

// These calls can place real limit orders when valid credentials are supplied.
var placement = await client.SpotApi.Trading.PlaceMultipleOrdersAsync(requests);
if (!placement.Success)
{
    Console.WriteLine($"Batch request failed: {placement.Error}");
    return;
}

var placedOrderIds = new List<long>();
foreach (var item in placement.Data)
{
    if (item.Success)
    {
        placedOrderIds.Add(item.Data.OrderId);
        Console.WriteLine($"Placed order {item.Data.OrderId}");
    }
    else
    {
        Console.WriteLine($"One order failed: {item.Error}");
    }
}

if (placedOrderIds.Count == 0)
    return;

var cancellation = await client.SpotApi.Trading.CancelOrdersAsync(placedOrderIds);
if (!cancellation.Success)
{
    Console.WriteLine($"Batch cancellation request failed: {cancellation.Error}");
    return;
}

foreach (var item in cancellation.Data)
{
    Console.WriteLine(item.Success
        ? $"Cancelled order {item.Data.OrderId}"
        : $"One cancellation failed: {item.Error}");
}
