# AI-Friendly Examples

These self-contained console examples compile against the current Tapbit.Net API surface. Tapbit.Net currently supports spot REST; it does not expose Tapbit futures or WebSocket clients.

| File | What it shows |
|---|---|
| `01-spot-quickstart.cs` | Public ticker, authenticated balance, limit order placement, lookup, and cancellation |
| `02-market-data.cs` | Symbol metadata, order book, klines, recent trades, and assets |
| `03-batch-orders.cs` | Batch limit order placement, per-item results, and batch cancellation |
| `04-multi-exchange.cs` | Shared API discovery and exchange-agnostic spot ticker access |
| `05-error-handling.cs` | `HttpResult<T>`, structured errors, and transient-only retry |

## Run an example

```bash
dotnet new console -n MyTapbitApp
cd MyTapbitApp
dotnet add package Tapbit.Net
# Copy one example into Program.cs.
dotnet run
```

Private examples contain placeholder credentials. Replace them only when intentionally calling authenticated endpoints; order examples can place real orders.
