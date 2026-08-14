using CryptoExchange.Net.Testing;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NUnit.Framework;
using System;
using System.Threading.Tasks;
using Tapbit.Net.Clients;
using Tapbit.Net.Objects.Options;

namespace Tapbit.Net.UnitTests
{
    [NonParallelizable]
    internal class TapbitSocketIntegrationTests : SocketIntegrationTest<TapbitSocketClient>
    {
        public override bool Run { get; set; } = false;

        public TapbitSocketIntegrationTests()
        {
        }

        public override TapbitSocketClient GetClient(ILoggerFactory loggerFactory)
        {
            var key = Environment.GetEnvironmentVariable("APIKEY");
            var sec = Environment.GetEnvironmentVariable("APISECRET");

            Authenticated = key != null && sec != null;
            return new TapbitSocketClient(Options.Create(new TapbitSocketOptions
            {
                OutputOriginalData = true,
                ApiCredentials = Authenticated ? new TapbitCredentials(key, sec) : null
            }), loggerFactory);
        }

        [TestCase]
        public async Task TestSubscriptions()
        {
            //await RunAndCheckUpdate<>((client, updateHandler) => client.SpotApi.Account.SubscribeToUserDataUpdatesAsync(default, default, default, default, default, default, default, default), false, true);
            //await RunAndCheckUpdate<>((client, updateHandler) => client.SpotApi.ExchangeData.SubscribeToTickerUpdatesAsync("ETHUSDT", updateHandler, default), true, false);
        } 
    }
}
