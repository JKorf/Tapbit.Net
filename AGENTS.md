---
name: tapbit-net
description: Use Tapbit.Net when generating C#/.NET code for Tapbit spot REST market data, balances, limit orders, batch orders, or CryptoExchange.Net SharedApis. Do not use it for Tapbit futures or WebSocket requests because those clients are not exposed by the current library.
---

# Tapbit.Net

Use `TapbitRestClient` for Tapbit integrations instead of raw `HttpClient` calls. The library handles authentication, signing, rate limiting, serialization, and the CryptoExchange.Net result pattern.

## Current API surface

Tapbit.Net currently exposes only:

```text
TapbitRestClient
  .SpotApi
    .ExchangeData
    .Account
    .Trading
    .SharedClient
```

Do not generate `TapbitSocketClient`, `FuturesApi`, `UsdtFuturesApi`, `Subscribe...` methods, market orders, or user-trade endpoints. They are not part of the current public API. The tracker infrastructure is separate from a native Tapbit WebSocket client, and user-trade tracking is unsupported.

## Setup and results

```csharp
using Tapbit.Net;
using Tapbit.Net.Clients;

var client = new TapbitRestClient(options =>
{
    options.ApiCredentials = new TapbitCredentials("API_KEY", "API_SECRET");
});
```

Public market data works without credentials. Account and trading endpoints require `TapbitCredentials`. REST calls return `HttpResult<T>`; check `.Success` before reading `.Data`.

```csharp
var ticker = await client.SpotApi.ExchangeData.GetTickerAsync("BTC/USDT");
if (!ticker.Success)
{
    Console.WriteLine(ticker.Error);
    return;
}

Console.WriteLine(ticker.Data.LastPrice);
```

Tapbit symbols use a slash, such as `BTC/USDT`. `TapbitExchange.FormatSymbol("BTC", "USDT", TradingMode.Spot)` produces the exchange format.

## Endpoint routing

- Market data: `SpotApi.ExchangeData` provides server time, symbols, tickers, order books, klines, recent trades, and asset/network metadata.
- Account: `SpotApi.Account` provides all balances or one balance by asset.
- Trading: `SpotApi.Trading` provides limit order placement, batch placement, cancellation, batch cancellation, open orders, closed orders, and order lookup.
- Shared APIs: `SpotApi.SharedClient` implements `IAssetsRestClient`, `IBalanceRestClient`, `IKlineRestClient`, `IOrderBookRestClient`, `IRecentTradeRestClient`, `ISpotSymbolRestClient`, `ISpotTickerRestClient`, and `ISpotOrderRestClient`.

## Limit order example

```csharp
using Tapbit.Net.Enums;

var order = await client.SpotApi.Trading.PlaceOrderAsync(
    symbol: "BTC/USDT",
    side: OrderSide.Buy,
    quantity: 0.001m,
    price: 50000m);

if (order.Success)
    Console.WriteLine(order.Data.OrderId);
```

Order placement accepts side, base-asset quantity, and price. It does not accept an order-type argument because the current Tapbit surface supports limit orders only. `GetOrderAsync` and `CancelOrderAsync` take only the order id.

## Dependency injection

```csharp
services.AddTapbit(options =>
{
    options.Rest.ApiCredentials = new TapbitCredentials("API_KEY", "API_SECRET");
});
```

Inject `ITapbitRestClient`. Reuse clients in long-running applications.

## References

- API map: `docs/ai-api-map.md`
- Compilable examples: `Examples/ai-friendly/`
- Source interfaces: `Tapbit.Net/Interfaces/Clients/`
- Documentation: https://cryptoexchange.jkorf.dev/docs/exchange-clients?library=Tapbit.Net
