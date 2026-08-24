// Public Tapbit spot REST market-data endpoints.
// Tapbit.Net currently has no futures or WebSocket client.
// Setup: dotnet add package Tapbit.Net

using Tapbit.Net.Clients;
using Tapbit.Net.Enums;

const string symbol = "BTC/USDT";
var client = new TapbitRestClient();

var symbolInfo = await client.SpotApi.ExchangeData.GetSymbolAsync(symbol);
if (!symbolInfo.Success)
{
    Console.WriteLine($"Failed to get symbol: {symbolInfo.Error}");
    return;
}

Console.WriteLine(
    $"{symbolInfo.Data.Symbol}: price decimals={symbolInfo.Data.PricePrecision}, " +
    $"quantity decimals={symbolInfo.Data.QuantityPrecision}, min quantity={symbolInfo.Data.MinQuantity}");

var book = await client.SpotApi.ExchangeData.GetOrderBookAsync(symbol, depth: 10);
if (book.Success)
    Console.WriteLine($"Order book: {book.Data.Bids.Length} bids, {book.Data.Asks.Length} asks");
else
    Console.WriteLine($"Failed to get order book: {book.Error}");

var klines = await client.SpotApi.ExchangeData.GetKlinesAsync(
    symbol,
    KlineInterval.OneHour,
    startTime: DateTime.UtcNow.AddDays(-1),
    endTime: DateTime.UtcNow);

if (klines.Success)
{
    foreach (var kline in klines.Data.Take(3))
        Console.WriteLine($"{kline.OpenTime:u} O={kline.OpenPrice} H={kline.HighPrice} L={kline.LowPrice} C={kline.ClosePrice}");
}
else
{
    Console.WriteLine($"Failed to get klines: {klines.Error}");
}

var trades = await client.SpotApi.ExchangeData.GetRecentTradesAsync(symbol);
if (trades.Success)
    Console.WriteLine($"Received {trades.Data.Length} recent trades");

var assets = await client.SpotApi.ExchangeData.GetAssetsAsync("BTC");
if (assets.Success)
    Console.WriteLine($"Received metadata for {assets.Data.Length} BTC asset entries");
