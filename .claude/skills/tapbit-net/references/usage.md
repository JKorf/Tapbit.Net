# Tapbit.Net usage

## Imports

```csharp
using System;
using System.Linq;
using System.Threading.Tasks;
using CryptoExchange.Net.SharedApis;
using Tapbit.Net;
using Tapbit.Net.Clients;
using Tapbit.Net.Enums;
using Tapbit.Net.Objects.Models;
using Tapbit.Net.Interfaces.Clients;
using Microsoft.Extensions.DependencyInjection;
```

## Public ticker

```csharp
using var client = new TapbitRestClient();
var ticker = await client.SpotApi.ExchangeData
    .GetTickerAsync("BTC/USDT");

if (!ticker.Success)
{
    Console.WriteLine($"Ticker failed: {ticker.Error}");
    return;
}

Console.WriteLine(ticker.Data.LastPrice);
```

## Symbol metadata

```csharp
var symbol = await client.SpotApi.ExchangeData
    .GetSymbolAsync("BTC/USDT");

if (!symbol.Success)
{
    Console.WriteLine($"Symbol lookup failed: {symbol.Error}");
    return;
}

Console.WriteLine($"Price decimals: {symbol.Data.PricePrecision}");
Console.WriteLine($"Quantity decimals: {symbol.Data.QuantityPrecision}");
Console.WriteLine($"Minimum quantity: {symbol.Data.MinQuantity}");
Console.WriteLine($"Minimum notional: {symbol.Data.MinNotional}");
```

## Authentication and balances

```csharp
using var client = new TapbitRestClient(options =>
{
    options.ApiCredentials =
        new TapbitCredentials(
            Environment.GetEnvironmentVariable("TAPBIT_KEY") ?? throw new InvalidOperationException("API key is required"),
            Environment.GetEnvironmentVariable("TAPBIT_SECRET") ?? throw new InvalidOperationException("API secret is required"));
});

var balance = await client.SpotApi.Account.GetBalanceAsync("USDT");
if (!balance.Success)
{
    Console.WriteLine($"Balance failed: {balance.Error}");
    return;
}

Console.WriteLine($"Available: {balance.Data.Available}");
```

## Live order lifecycle

This uses an authenticated client as above and changes live account state. Read [execution considerations](safety.md#limit-orders-and-cancellation) before running.

```csharp
var order = await client.SpotApi.Trading.PlaceOrderAsync(
    symbol: "BTC/USDT",
    side: OrderSide.Buy,
    quantity: 0.001m,
    price: 50000m);

if (!order.Success)
{
    Console.WriteLine($"Placement failed: {order.Error}");
    return;
}

long orderId = order.Data.OrderId;

var status = await client.SpotApi.Trading.GetOrderAsync(orderId);
if (!status.Success)
{
    Console.WriteLine($"Lookup failed: {status.Error}");
}

if (status.Success)
    Console.WriteLine($"{status.Data.Status}, filled {status.Data.QuantityFilled}");

var cancel = await client.SpotApi.Trading.CancelOrderAsync(orderId);
if (!cancel.Success)
    Console.WriteLine($"Cancellation failed: {cancel.Error}");
```

## Live batch orders

This uses an authenticated client as above and changes live account state. Read [execution considerations](safety.md#limit-orders-and-cancellation) before running.

```csharp
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

var batch = await client.SpotApi.Trading
    .PlaceMultipleOrdersAsync(requests);

if (!batch.Success)
{
    Console.WriteLine($"Batch request failed: {batch.Error}");
    return;
}

foreach (var item in batch.Data)
{
    if (item.Success)
        Console.WriteLine($"Placed {item.Data.OrderId}");
    else
        Console.WriteLine($"Item failed: {item.Error}");
}
```

## Shared API V2

```csharp
IGetTickerRest ticker = client.SpotApi.SharedApi;
var result = await ticker.GetTickerAsync(new GetTickerRequest(
    new SharedSymbol(TradingMode.Spot, "BTC", "USDT")));
if (!result.Success)
{
    Console.WriteLine(result.Error);
    return;
}
Console.WriteLine(result.Data.LastPrice);
```

Load symbols before reading the catalog:

```csharp
IGetSpotSymbolsRest symbolsClient = client.SpotApi.SharedApi;
var result = await symbolsClient.GetSpotSymbolsAsync(new GetSymbolsRequest());
if (!result.Success)
{
    Console.WriteLine(result.Error);
    return;
}
foreach (var symbol in result.Data.Take(5))
    Console.WriteLine($"{symbol.Name}: {symbol.BaseAsset}/{symbol.QuoteAsset}");
var catalog = symbolsClient.SpotSymbolCatalog;
```

## Dependency injection and dynamic lookup

```csharp
var services = new ServiceCollection();
services.AddTapbit();
using var provider = services.BuildServiceProvider();
var aggregate = provider.GetRequiredService<ITapbitSharedApiClient>();
var match = aggregate.GetCapability(
    SharedCapabilities.Tickers.GetTicker.Rest, tradingMode: TradingMode.Spot);
if (match is null)
    return;
var result = await match.Capability.GetTickerAsync(new GetTickerRequest(
    new SharedSymbol(TradingMode.Spot, "BTC", "USDT")));
if (!result.Success)
{
    Console.WriteLine(result.Error);
    return;
}
Console.WriteLine(result.Data.LastPrice);
```

Configure `options.Rest.ApiCredentials` via `services.AddTapbit(options => { ... })` for authenticated consumers. Inject `ITapbitRestClient`, the aggregate, or a registered narrow capability. [API routing](api-surfaces.md#shared-api-v2) describes multiple-exchange selection and the REST-only capability boundary. WebSocket subscription examples do not apply to this library.

## User clients and REST-polled tracking

`ITapbitUserClientProvider` caches REST clients by application user identifier. Clear the user's cached client after credential changes. `ITapbitTrackerFactory.CreateUserSpotDataTracker(...)` requires a config with explicit `TrackedSymbols` and `TrackTrades = false`. Start/check/stop the owned tracker; there are no socket subscriptions to clean up. See [tracker prerequisites](api-surfaces.md#di-user-clients-and-trackers).

## Complete programs (checkout-only)

[Examples](../../../../Examples/ai-friendly/README.md) contains independent Spot reads, market data, live batches, Shared API V2 and public-read retry programs. These links require a checkout; the snippets above work with the installed package.
