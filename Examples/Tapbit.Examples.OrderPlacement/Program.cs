using Tapbit.Net;
using Tapbit.Net.Clients;
using Tapbit.Net.Enums;

const string spotSymbol = "BTC/USDT";

// Replace with valid credentials or order placement will always fail
var apiKey = "KEY";
var apiSecret = "SECRET";

Console.WriteLine("Tapbit.Net order placement example");
Console.WriteLine();
Console.WriteLine("This example can place real orders when valid credentials are configured.");
Console.WriteLine();

var client = new TapbitRestClient(options =>
{
    options.ApiCredentials = new TapbitCredentials(apiKey, apiSecret);
});

await PlaceSpotLimitOrderAsync(client);

static async Task PlaceSpotLimitOrderAsync(TapbitRestClient client)
{
    Console.WriteLine($"Placing spot limit buy order for {spotSymbol}...");

    var ticker = await client.SpotApi.ExchangeData.GetTickerAsync(spotSymbol);
    if (!ticker.Success)
    {
        Console.WriteLine($"Failed to get spot ticker: {ticker.Error}");
        return;
    }

    var safePrice = Math.Round(ticker.Data.LastPrice!.Value * 0.95m, 2);
    var order = await client.SpotApi.Trading.PlaceOrderAsync(
        symbol: spotSymbol,
        side: OrderSide.Buy,
        quantity: 0.001m,
        price: safePrice);

    if (!order.Success)
    {
        Console.WriteLine($"Failed to place spot order: {order.Error}");
        return;
    }

    Console.WriteLine($"Placed spot order {order.Data.OrderId}");

    var orderStatus = await client.SpotApi.Trading.GetOrderAsync(order.Data.OrderId);
    if (orderStatus.Success)
        Console.WriteLine($"Spot order status: {orderStatus.Data.Status}, filled: {orderStatus.Data.QuantityFilled}");
    else
        Console.WriteLine($"Failed to query spot order: {orderStatus.Error}");

    var cancel = await client.SpotApi.Trading.CancelOrderAsync(order.Data.OrderId);
    Console.WriteLine(cancel.Success
        ? $"Cancelled spot order {order.Data.OrderId}"
        : $"Failed to cancel spot order: {cancel.Error}");
}
