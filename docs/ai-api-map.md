# Tapbit.Net AI API Map

Tapbit.Net currently supports spot REST only. `TapbitRestClient.SpotApi` is the complete exchange API surface; there is no public socket or futures client.

## Client map

| Intent | Authentication | Method |
|---|---|---|
| Server time | Public | `SpotApi.ExchangeData.GetServerTimeAsync()` |
| One symbol | Public | `SpotApi.ExchangeData.GetSymbolAsync("BTC/USDT")` |
| All symbols | Public | `SpotApi.ExchangeData.GetSymbolsAsync()` |
| Order book | Public | `SpotApi.ExchangeData.GetOrderBookAsync("BTC/USDT", 10)` |
| One ticker | Public | `SpotApi.ExchangeData.GetTickerAsync("BTC/USDT")` |
| All tickers | Public | `SpotApi.ExchangeData.GetTickersAsync()` |
| Klines | Public | `SpotApi.ExchangeData.GetKlinesAsync("BTC/USDT", KlineInterval.OneHour)` |
| Recent trades | Public | `SpotApi.ExchangeData.GetRecentTradesAsync("BTC/USDT")` |
| Assets and networks | Public | `SpotApi.ExchangeData.GetAssetsAsync()` |
| One asset | Public | `SpotApi.ExchangeData.GetAssetsAsync("BTC")` |
| All balances | API key | `SpotApi.Account.GetBalancesAsync()` |
| One balance | API key | `SpotApi.Account.GetBalanceAsync("USDT")` |
| Place limit order | API key | `SpotApi.Trading.PlaceOrderAsync("BTC/USDT", OrderSide.Buy, 0.001m, 50000m)` |
| Place batch | API key | `SpotApi.Trading.PlaceMultipleOrdersAsync(orders)` |
| Cancel order | API key | `SpotApi.Trading.CancelOrderAsync(orderId)` |
| Cancel batch | API key | `SpotApi.Trading.CancelOrdersAsync(orderIds)` |
| Open orders | API key | `SpotApi.Trading.GetOpenOrdersAsync("BTC/USDT")` |
| Closed orders | API key | `SpotApi.Trading.GetClosedOrdersAsync("BTC/USDT")` |
| One order | API key | `SpotApi.Trading.GetOrderAsync(orderId)` |

Every method accepts an optional `CancellationToken` as its final parameter. Open and closed order queries also accept an optional `fromId` before the cancellation token.

## Shared REST interfaces

`TapbitRestClient.SpotApi.SharedApi` supports:

| Interface | Capability |
|---|---|
| `IGetAssetRest` / `IGetAllAssetsRest` | One asset or all assets and network metadata |
| `IGetBalancesRest` | Spot balances |
| `IGetKlinesRest` | Spot klines |
| `IGetOrderBookRest` | Spot order-book snapshots |
| `IGetRecentTradesRest` | Recent public trades |
| `IGetSpotSymbolsRest` | Spot symbol catalog and filters |
| `IGetTickerRest` / `IGetAllTickersRest` | One spot ticker or all spot tickers |
| `IPlaceSpotOrderRest` | Place a limit order |
| `IGetSpotOrderRest` | Get one order |
| `IGetOpenSpotOrdersRest` / `IGetClosedSpotOrdersRest` | List open or closed orders |
| `ICancelSpotOrderRest` | Cancel an order |

Use the exchange-level `ITapbitSharedApiClient` aggregate's `GetCapability(...)` or `GetCapabilities(...)` methods for runtime capability lookup; use an API surface's `.SharedApi` property when the transport and API are known.

## Unsupported requests

Do not route futures, positions, leverage, funding, WebSocket subscriptions, deposits, withdrawals, transfers, or user trades to Tapbit.Net. The current public library surface has no such endpoints.
