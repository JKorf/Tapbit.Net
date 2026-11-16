---
name: tapbit-net
description: Generate and troubleshoot C#/.NET code consuming Tapbit.Net for Spot REST market data, balances, limit orders, batches, REST-polled user tracking and Shared API V2 integrations.
---

# Tapbit.Net

Install the consumer package:

```sh
dotnet add package Tapbit.Net
```

Use `TapbitRestClient.SpotApi.{ExchangeData|Account|Trading}`. The public library exposes Spot REST only: no socket client, market-order placement, futures, user-trade, funds-movement or local order-book APIs. Reuse and dispose clients, and await calls.

Credentials are `TapbitCredentials(key, secret)` without a passphrase. Public market data needs no credentials; balances and trading require the corresponding permissions.

Native symbols use uppercase `BASE/QUOTE`, e.g. `BTC/USDT`. Placement is limit-only and takes symbol, side, base quantity and price, with no order-type argument. Native order IDs are `long`; query/cancel methods take the order ID without a symbol.

Check `Success` before `Data`: native REST returns `HttpResult<T>` and some Shared helpers return `ExchangeCallResult<T>`. Batch placement/cancellation additionally contains per-item `CallResult<TapbitOrderId>` values; inspect each item after outer success.

Use `.SpotApi.SharedApi` and narrow V2 capabilities for portable operations. `ITapbitSharedApiClient` exposes `SpotRest`. User-data tracking polls REST; configure tracked symbols and `TrackTrades = false`. Tracker infrastructure does not imply socket or public-trade/kline tracker support.

## Read when needed

- [API surfaces](references/api-surfaces.md): endpoint routing, capabilities, catalogs, quantities and tracker prerequisites.
- [Usage](references/usage.md): setup, authentication, reads, live order/batch syntax, Shared API V2 and DI.
- [Operational considerations](references/safety.md): execution, partial batches, polling state and uncertain submissions.

For work on the library itself, use [repository instructions](../../../AGENTS.md) (checkout-only).
