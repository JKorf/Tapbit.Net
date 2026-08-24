using Tapbit.Net.Clients;
using CryptoExchange.Net.Objects;
using Microsoft.Extensions.Logging;
using Tapbit.Net.Objects.Options;
using Microsoft.Extensions.Options;

// REST
var restClient = new TapbitRestClient();
var ticker = await restClient.SpotApi.ExchangeData.GetTickerAsync("ETH/USDT");
if (!ticker.Success)
{
    Console.WriteLine($"Failed to get ticker: {ticker.Error}");
    return;
}

Console.WriteLine($"Rest client ticker price for ETH/USDT: {ticker.Data.LastPrice}");

Console.WriteLine();
Console.ReadLine();