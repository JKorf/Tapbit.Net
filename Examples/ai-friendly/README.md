# Tapbit.Net examples

Each file is an independent console program. Install `Tapbit.Net` in a console project and copy one program to `Program.cs`.

| Program | Workflow |
|---|---|
| [01-spot-quickstart.cs](01-spot-quickstart.cs) | Public market data and authenticated account/order operations |
| [02-market-data.cs](02-market-data.cs) | Symbols, books, candles and public trades |
| [03-batch-orders.cs](03-batch-orders.cs) | Live batches and per-item results |
| [04-multi-exchange.cs](04-multi-exchange.cs) | Shared API V2 ticker and symbol catalog |
| [05-error-handling.cs](05-error-handling.cs) | Results and bounded public-read retry |

Replace credential placeholders for intended private operations. Order examples change live account state and cancellation can race with fills. Read [operational considerations](../../.claude/skills/tapbit-net/references/safety.md) first. See [usage snippets](../../.claude/skills/tapbit-net/references/usage.md) for focused patterns.

`AiExampleCompileTests` compiles these programs without executing exchange operations. This library is Spot REST-only.
