using CryptoExchange.Net.Objects.Errors;
using CryptoExchange.Net.Testing;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Tapbit.Net.Clients;
using Tapbit.Net.Objects.Options;

namespace Tapbit.Net.UnitTests
{
    [NonParallelizable]
    public class TapbitRestIntegrationTests : RestIntegrationTest<TapbitRestClient>
    {
        public override bool Run { get; set; } = false;

        public override TapbitRestClient GetClient(ILoggerFactory loggerFactory)
        {
            var key = Environment.GetEnvironmentVariable("APIKEY");
            var sec = Environment.GetEnvironmentVariable("APISECRET");

            Authenticated = key != null && sec != null;
            return new TapbitRestClient(null, loggerFactory, Options.Create(new TapbitRestOptions
            {
                AutoTimestamp = false,
                OutputOriginalData = true,
                ApiCredentials = Authenticated ? new TapbitCredentials(key, sec) : null
            }));
        }

        [Test]
        public async Task TestErrorResponseParsing()
        {
            if (!ShouldRun())
                return;

            var result = await CreateClient().SpotApi.ExchangeData.GetOrderBookAsync("TSTTST", 12, default);

            Assert.That(result.Success, Is.False);
            Assert.That(result.Error.Code, Is.EqualTo(11001));
            Assert.That(result.Error.ErrorType, Is.EqualTo(ErrorType.InvalidParameter));
        }

        [Test]
        public async Task TestSpotAccount()
        {
            var warnings = new List<Exception>();
            await RunAndCheckResult(warnings, client => client.SpotApi.Account.GetBalancesAsync(default), true, "data");
            await RunAndCheckResult(warnings, client => client.SpotApi.Account.GetBalanceAsync("USDT", default), true, "data");
            foreach (var warning in warnings)
                Assert.Warn(warning.Message);
        }

        [Test]
        public async Task TestSpotExchangeData()
        {
            var warnings = new List<Exception>();
            await RunAndCheckResult(warnings, client => client.SpotApi.ExchangeData.GetSymbolAsync("ETH/USDT", default), false, "data");
            await RunAndCheckResult(warnings, client => client.SpotApi.ExchangeData.GetSymbolsAsync(default), false, "data");
            await RunAndCheckResult(warnings, client => client.SpotApi.ExchangeData.GetOrderBookAsync("ETH/USDT", 5, default), false, "data");
            await RunAndCheckResult(warnings, client => client.SpotApi.ExchangeData.GetTickerAsync("ETH/USDT", default), false, "data");
            await RunAndCheckResult(warnings, client => client.SpotApi.ExchangeData.GetTickersAsync(default), false, "data");
            await RunAndCheckResult(warnings, client => client.SpotApi.ExchangeData.GetKlinesAsync("ETH/USDT", Enums.KlineInterval.OneDay, default, default, default), false, "data");
            await RunAndCheckResult(warnings, client => client.SpotApi.ExchangeData.GetRecentTradesAsync("ETH/USDT", default), false, "data");
            await RunAndCheckResult(warnings, client => client.SpotApi.ExchangeData.GetAssetsAsync(default, default), false, "data");
            foreach (var warning in warnings)
                Assert.Warn(warning.Message);
        }

        [Test]
        public async Task TestSpotTrading()
        {
            var warnings = new List<Exception>();
            await RunAndCheckResult(warnings, client => client.SpotApi.Trading.GetClosedOrdersAsync("ETH/USDT", default, default), true, "data");
            await RunAndCheckResult(warnings, client => client.SpotApi.Trading.GetOpenOrdersAsync("ETH/USDT", default, default), true, "data");
            foreach (var warning in warnings)
                Assert.Warn(warning.Message);
        }

    }
}
