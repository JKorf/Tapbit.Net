# Tapbit operational considerations

## Credentials and polling

Use HMAC key/secret from application secret storage. Public reads need no credentials; private reads and orders require their intended permissions. Clear cached per-user clients after credential rotation. REST-polled trackers need explicit symbols and `TrackTrades = false`; allow for polling delays when reconciling order state rather than treating the cache as a live execution stream.

## Limit orders and cancellation

Placement is live and limit-only; there is no test-order endpoint. Query current symbol precision, minimum quantity/notional, allowed price fluctuation and available balances before sizing. A distant limit price can still execute and may violate exchange bounds; it is not a safety mechanism.

Order IDs are numeric `long` values. Retain the returned ID for query/cancel; those methods do not take a symbol. Cancellation can race with fills, so inspect quantity filled and remaining order state before concluding that no exposure remains. The native API has no futures position-mode or leverage workflow.

## Partial batches

Batch place/cancel returns an outer `HttpResult` with per-item `CallResult<TapbitOrderId>` entries. Check both layers and record successful IDs as well as failures. Failed items do not roll back accepted siblings. Reconcile successful and uncertain entries before retrying a subset; do not repeat an entire batch based on one rejection.

## Uncertain submissions and retries

A timeout or connection loss can leave placement or cancellation accepted but unconfirmed. `Error.IsTransient` alone does not justify repeating it. Query retained order IDs or inspect open/closed orders and account balances before deciding whether to resubmit, allowing for propagation and polling delays. There is no client-order-ID placement/query mechanism to invent for deduplication. Bounded backoff is appropriate for transient public reads; keep generic read-retry helpers away from account changes.
