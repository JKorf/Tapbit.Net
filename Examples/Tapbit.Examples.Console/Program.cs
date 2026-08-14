using Tapbit.Net.Clients;
using CryptoExchange.Net.Objects;
using Microsoft.Extensions.Logging;
using Tapbit.Net.Objects.Options;
using Microsoft.Extensions.Options;

// REST
var restClient = new TapbitRestClient();
var ticker = await restClient.SpotApi.ExchangeData.GetTickerAsync("ETHUSDT");
if (!ticker.Success)
{
    Console.WriteLine($"Failed to get ticker: {ticker.Error}");
    return;
}

Console.WriteLine($"Rest client ticker price for ETHUSDT: {ticker.Data.LastPrice}");

Console.WriteLine();
Console.WriteLine("Press enter to start websocket subscription");
Console.ReadLine();

// Websocket
// Optional, manually add logging
var logFactory = new LoggerFactory();
logFactory.AddProvider(new TraceLoggerProvider());

var socketClient = new TapbitSocketClient(Options.Create(new TapbitSocketOptions { }), logFactory);
var subscription = await socketClient.SpotApi.SubscribeToMiniTickerUpdatesAsync("ETHUSDT", update =>
{
    Console.WriteLine($"Websocket client ticker price for ETHUSDT: {update.Data.LastPrice}");
});

if (!subscription.Success)
{
    Console.WriteLine($"Failed to subscribe to ticker updates: {subscription.Error}");
    return;
}

Console.ReadLine();
