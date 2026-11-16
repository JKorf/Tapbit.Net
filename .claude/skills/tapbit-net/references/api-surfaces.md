# Tapbit.Net API surfaces

## Client roots

`TapbitRestClient` / `ITapbitRestClient` expose `SpotApi.{ExchangeData|Account|Trading}`. There is no socket client or futures, margin, user-trade, deposit, withdrawal or transfer API. Historical order models having an order type do not imply support for placing market orders.

## Exchange Data

`client.SpotApi.ExchangeData` provides:

- `GetServerTimeAsync()`
- `GetSymbolAsync(symbol)`
- `GetSymbolsAsync()`
- `GetOrderBookAsync(symbol, depth)` where depth is 5, 10, 50, or 100
- `GetTickerAsync(symbol)`
- `GetTickersAsync()`
- `GetKlinesAsync(symbol, interval, startTime, endTime)`
- `GetRecentTradesAsync(symbol)`
- `GetAssetsAsync(asset)` where the asset filter is optional

Kline intervals are one, three, five, fifteen, or thirty minutes; one, two, four, six, or twelve hours; one day; one week; or one month.

`TapbitSymbol` includes base/quote assets, price and quantity precision, maker/taker fee rates, minimum quantity, minimum notional, and allowed price fluctuation. Use it before production trading.

## Account

`client.SpotApi.Account` provides:

- `GetBalancesAsync()`
- `GetBalanceAsync(asset)`

Both require credentials. `TapbitBalance` exposes `Asset`, `Available`, `FrozenBalance`, and `TotalBalance`.

## Trading

`client.SpotApi.Trading` provides:

- `PlaceOrderAsync(symbol, side, quantity, price)`
- `PlaceMultipleOrdersAsync(IEnumerable<TapbitOrderRequest>)`
- `CancelOrderAsync(long orderId)`
- `CancelOrdersAsync(IEnumerable<long> orderIds)`
- `GetOpenOrdersAsync(symbol, long? fromId)`
- `GetClosedOrdersAsync(symbol, long? fromId)`
- `GetOrderAsync(long orderId)`

Placement is limit-only even though returned historical order models can contain an `OrderType`. Do not generate a market-order placement overload.

Batch placement and cancellation return `HttpResult<CallResult<TapbitOrderId>[]>`; outer success does not imply every inner item succeeded.

## Symbols And Environments

Native symbols use slash-separated uppercase form, for example `BTC/USDT`. Use:

```csharp
var symbol = TapbitExchange.FormatSymbol(
    "BTC", "USDT", TradingMode.Spot);
```

Only `TapbitEnvironment.Live` is built in. `TapbitEnvironment.CreateCustom(name, spotRestAddress)` creates a deliberate custom REST environment. There is no built-in testnet.

## Shared API V2

`ITapbitSharedApiClient.SpotRest` and `client.SpotApi.SharedApi` expose `ITapbitRestClientSpotSharedApi`:

`IGetAssetRest`, `IGetAllAssetsRest`, `IGetBalancesRest`, `IGetKlinesRest`, `IGetOrderBookRest`, `IGetRecentTradesRest`, `IGetSpotSymbolsRest`, `IGetTickerRest`, `IGetAllTickersRest`, `IPlaceSpotOrderRest`, `IGetSpotOrderRest`, `IGetOpenSpotOrdersRest`, `IGetClosedSpotOrdersRest`, `ICancelSpotOrderRest`.

For known workflows use the aggregate property or inject a narrow capability. For dynamic selection call `aggregate.GetCapability(SharedCapabilities.Tickers.GetTicker.Rest, tradingMode: TradingMode.Spot)` and check for null before using `Capability` and `Options`. Descriptors identify operations, not guarantees of support. `GetCapabilities` enumerates matches and `Discover` summarizes metadata. With several exchanges registered, inject `IEnumerable<TCapability>` and select by exchange, supported mode and options. There is no Shared socket surface.

## Catalog and quantities

Load `IGetSpotSymbolsRest.GetSpotSymbolsAsync(new GetSymbolsRequest())`, check success, then inspect `SpotSymbolCatalog`. Requests honor base/quote asset type/subtype filters. Returned metadata includes display names, precision, min/max constraints and crypto/stablecoin classifications where populated; do not assume TradFi or fiat classifications are populated here.

Native limit-order quantities are base-asset amounts. Shared limit orders use `SharedOrderQuantity.QuantityInBaseAsset` and order books identify `SharedQuantityType.BaseAsset`. Inspect supported order types/options; quantity-support metadata does not imply market-order placement is available.

## DI, user clients and trackers

`services.AddTapbit(...)` registers `ITapbitRestClient`, `ITapbitSharedApiClient`, supported strict capabilities, `ITapbitUserClientProvider`, and tracker factories. User clients are cached by application user identifier; clear stale clients after credential changes.

`ITapbitTrackerFactory` does not create public trade or kline trackers. Its user Spot tracker polls REST balances/orders. `SpotUserDataTrackerConfig` must provide at least one `TrackedSymbols` entry and set `TrackTrades = false`. Check tracker start results and stop it when its owning service ends. It does not provide realtime socket state or user-trade tracking.

## Checkout-only sources

- [Public interfaces](../../../../Tapbit.Net/Interfaces/Clients/): exact signatures, pagination and cancellation tokens (checkout-only).
- [Shared implementations](../../../../Tapbit.Net/Clients/): supported options, quantity conversion and symbol classification (checkout-only).
- [Complete examples](../../../../Examples/ai-friendly/): independent consumer programs (checkout-only).
