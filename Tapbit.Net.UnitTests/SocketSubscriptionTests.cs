using CryptoExchange.Net.Objects;
using CryptoExchange.Net.Testing;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NUnit.Framework;
using System.Threading.Tasks;
using Tapbit.Net.Clients;
using Tapbit.Net.Objects.Models;
using Tapbit.Net.Objects.Options;

namespace Tapbit.Net.UnitTests
{
    [TestFixture]
    public class SocketSubscriptionTests
    {
        [Test]
        public async Task ValidateSubscriptions()
        {
            var client = new TapbitSocketClient(opts =>
            {
                opts.ApiCredentials = new TapbitCredentials("123", "456");
            });
            var tester = new SocketSubscriptionValidator<TapbitSocketClient>(client, "Subscriptions/Spot", "XXX");
            //await tester.ValidateAsync<TapbitModel>((client, handler) => client.SpotApi.SubscribeToXXXUpdatesAsync(handler), "XXX");
        }

        [TestCase]
        public async Task ValidateConcurrentSpotSubscriptions()
        {
            var logger = new LoggerFactory();
            logger.AddProvider(new TraceLoggerProvider());

            var client = new TapbitSocketClient(Options.Create(new TapbitSocketOptions
            {
                ApiCredentials = new TapbitCredentials("123", "456"),
                OutputOriginalData = true
            }), logger);

            var tester = new SocketSubscriptionValidator<TapbitSocketClient>(client, "Subscriptions/Spot", "XXX", "data");
            //await tester.ValidateConcurrentAsync<TapbitModel>(
            //    (client, handler) => client.SpotApi.ExchangeData.SubscribeToKlineUpdatesAsync("BTCUSDT", Enums.KlineInterval.EightHour, handler),
            //    (client, handler) => client.SpotApi.ExchangeData.SubscribeToKlineUpdatesAsync("BTCUSDT", Enums.KlineInterval.OneHour, handler),
            //    "Concurrent");
        }
    }
}
