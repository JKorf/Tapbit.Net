// Exchange-agnostic ticker access through CryptoExchange.Net SharedApis.
// Setup: dotnet add package Tapbit.Net

using CryptoExchange.Net.SharedApis;
using Tapbit.Net.Clients;

var tapbit = new TapbitRestClient();
var shared = tapbit.SpotApi.SharedClient;

var info = shared.Discover();
var supported = info.Features
    .Where(x => x.Supported)
    .Select(x => x.EndpointName);

Console.WriteLine($"{info.Exchange} {info.TypeName}: {string.Join(", ", supported)}");

ISpotTickerRestClient tickerClient = shared;
var symbol = new SharedSymbol(TradingMode.Spot, "BTC", "USDT");
await PrintTickerAsync(tickerClient, symbol);

async Task PrintTickerAsync(ISpotTickerRestClient client, SharedSymbol requestedSymbol)
{
    var result = await client.GetSpotTickerAsync(new GetTickerRequest(requestedSymbol));
    if (!result.Success)
    {
        Console.WriteLine($"[{client.Exchange}] Failed: {result.Error}");
        return;
    }

    Console.WriteLine($"[{client.Exchange}] {result.Data.Symbol}: {result.Data.LastPrice}");
}

// The same PrintTickerAsync method accepts another exchange library's
// ISpotTickerRestClient implementation. SharedSymbol avoids hard-coding Tapbit's BTC/USDT format.
