// HttpResult handling and transient-only retry for Tapbit spot REST calls.
// Setup: dotnet add package Tapbit.Net

using CryptoExchange.Net.Objects;
using Tapbit.Net.Clients;

var client = new TapbitRestClient();

var ticker = await client.SpotApi.ExchangeData.GetTickerAsync("BTC/USDT");
if (ticker.Success)
{
    Console.WriteLine($"BTC/USDT: {ticker.Data.LastPrice}");
}
else
{
    Console.WriteLine($"Code: {ticker.Error?.Code}");
    Console.WriteLine($"Message: {ticker.Error?.Message}");
    Console.WriteLine($"Type: {ticker.Error?.ErrorType}");
    Console.WriteLine($"Transient: {ticker.Error?.IsTransient}");
}

var tickers = await WithRetryAsync(
    () => client.SpotApi.ExchangeData.GetTickersAsync(),
    maxAttempts: 3);

Console.WriteLine(tickers.Success
    ? $"Received {tickers.Data.Length} tickers"
    : $"Ticker lookup failed: {tickers.Error}");

async Task<HttpResult<T>> WithRetryAsync<T>(
    Func<Task<HttpResult<T>>> call,
    int maxAttempts)
{
    HttpResult<T> last = default!;

    for (var attempt = 1; attempt <= maxAttempts; attempt++)
    {
        last = await call();
        if (last.Success || last.Error?.IsTransient != true)
            return last;

        await Task.Delay(TimeSpan.FromMilliseconds(250 * Math.Pow(2, attempt - 1)));
    }

    return last;
}

// Authentication, invalid symbol, invalid price/quantity, and insufficient-balance
// errors are not fixed by blind retries. Correct or surface those failures instead.
