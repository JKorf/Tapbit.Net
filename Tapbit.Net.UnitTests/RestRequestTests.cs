using CryptoExchange.Net.Objects;
using CryptoExchange.Net.Testing;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tapbit.Net.Clients;
using Tapbit.Net.Enums;

namespace Tapbit.Net.UnitTests
{
    [TestFixture]
    public class RestRequestTests
    {
        [Test]
        public async Task ValidateAccountCalls()
        {
            var client = new TapbitRestClient(opts =>
            {
                opts.AutoTimestamp = false;
                opts.ApiCredentials = new TapbitCredentials("123", "456");
            });
            var tester = new RestRequestValidator<TapbitRestClient>(client, "Endpoints/Spot/Account", "XXX", IsAuthenticated);
            await tester.ValidateAsync(client => client.SpotApi.Account.GetBalancesAsync(), "GetBalances", nestedJsonProperty: "data");

        }

        [Test]
        public async Task ValidateExchangeDataCalls()
        {
            var client = new TapbitRestClient(opts =>
            {
                opts.AutoTimestamp = false;
                opts.ApiCredentials = new TapbitCredentials("123", "456");
            });
            var tester = new RestRequestValidator<TapbitRestClient>(client, "Endpoints/Spot/ExchangeData", "XXX", IsAuthenticated);
            await tester.ValidateAsync(client => client.SpotApi.ExchangeData.GetSymbolsAsync(), "GetSymbols", nestedJsonProperty: "data");
            await tester.ValidateAsync(client => client.SpotApi.ExchangeData.GetSymbolAsync("ETH/USDT"), "GetSymbol", nestedJsonProperty: "data");
            await tester.ValidateAsync(client => client.SpotApi.ExchangeData.GetOrderBookAsync("ETH/USDT", 10), "GetOrderBook", nestedJsonProperty: "data");
            await tester.ValidateAsync(client => client.SpotApi.ExchangeData.GetTickerAsync("ETH/USDT"), "GetTicker", nestedJsonProperty: "data");
            await tester.ValidateAsync(client => client.SpotApi.ExchangeData.GetTickersAsync(), "GetTickers", nestedJsonProperty: "data");
            await tester.ValidateAsync(client => client.SpotApi.ExchangeData.GetKlinesAsync("ETH/USDT", KlineInterval.OneDay), "GetKlines", nestedJsonProperty: "data");
            await tester.ValidateAsync(client => client.SpotApi.ExchangeData.GetRecentTradesAsync("ETH/USDT"), "GetRecentTrades", nestedJsonProperty: "data");
            await tester.ValidateAsync(client => client.SpotApi.ExchangeData.GetAssetsAsync("BTC"), "GetAssets", nestedJsonProperty: "data");

        }

        private bool IsAuthenticated(IHttpResult result)
        {
            return result.RequestHeaders?.Any(x => x.Key == "ACCESS-SIGN") == true;
        }
    }
}
