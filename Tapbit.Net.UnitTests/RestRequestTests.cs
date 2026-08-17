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
            var tester = new RestRequestValidator<TapbitRestClient>(client, "Endpoints/Spot/Account", "https://openapi.tapbit.com", IsAuthenticated);
            await tester.ValidateAsync(client => client.SpotApi.Account.GetBalancesAsync(), "GetBalances", nestedJsonProperty: "data");
            await tester.ValidateAsync(client => client.SpotApi.Account.GetBalanceAsync("ETH"), "GetBalance", nestedJsonProperty: "data");
        }

        [Test]
        public async Task ValidateExchangeDataCalls()
        {
            var client = new TapbitRestClient(opts =>
            {
                opts.AutoTimestamp = false;
                opts.ApiCredentials = new TapbitCredentials("123", "456");
            });
            var tester = new RestRequestValidator<TapbitRestClient>(client, "Endpoints/Spot/ExchangeData", "https://openapi.tapbit.com", IsAuthenticated);
            await tester.ValidateAsync(client => client.SpotApi.ExchangeData.GetSymbolsAsync(), "GetSymbols", nestedJsonProperty: "data");
            await tester.ValidateAsync(client => client.SpotApi.ExchangeData.GetSymbolAsync("ETH/USDT"), "GetSymbol", nestedJsonProperty: "data");
            await tester.ValidateAsync(client => client.SpotApi.ExchangeData.GetOrderBookAsync("ETH/USDT", 10), "GetOrderBook", nestedJsonProperty: "data");
            await tester.ValidateAsync(client => client.SpotApi.ExchangeData.GetTickerAsync("ETH/USDT"), "GetTicker", nestedJsonProperty: "data");
            await tester.ValidateAsync(client => client.SpotApi.ExchangeData.GetTickersAsync(), "GetTickers", nestedJsonProperty: "data");
            await tester.ValidateAsync(client => client.SpotApi.ExchangeData.GetKlinesAsync("ETH/USDT", KlineInterval.OneDay), "GetKlines", nestedJsonProperty: "data");
            await tester.ValidateAsync(client => client.SpotApi.ExchangeData.GetRecentTradesAsync("ETH/USDT"), "GetRecentTrades", nestedJsonProperty: "data");
            await tester.ValidateAsync(client => client.SpotApi.ExchangeData.GetAssetsAsync("ETH"), "GetAssets", nestedJsonProperty: "data");

        }

        [Test]
        public async Task ValidateTradingCalls()
        {
            var client = new TapbitRestClient(opts =>
            {
                opts.AutoTimestamp = false;
                opts.ApiCredentials = new TapbitCredentials("123", "456");
            });
            var tester = new RestRequestValidator<TapbitRestClient>(client, "Endpoints/Spot/Trading", "https://openapi.tapbit.com", IsAuthenticated);
            await tester.ValidateAsync(client => client.SpotApi.Trading.PlaceOrderAsync("ETH/USDT", OrderSide.Buy, 0.1m, 0.1m), "PlaceOrder", nestedJsonProperty: "data");
            await tester.ValidateAsync(client => client.SpotApi.Trading.CancelOrderAsync("123"), "CancelOrder", nestedJsonProperty: "data");
            await tester.ValidateAsync(client => client.SpotApi.Trading.GetOpenOrdersAsync("ETH/USDT"), "GetOrders", nestedJsonProperty: "data");
            await tester.ValidateAsync(client => client.SpotApi.Trading.GetClosedOrdersAsync("ETH/USDT"), "GetClosedOrders", nestedJsonProperty: "data");
            await tester.ValidateAsync(client => client.SpotApi.Trading.GetOrderAsync("123"), "GetOrder", nestedJsonProperty: "data");
        }


        private bool IsAuthenticated(IHttpResult result)
        {
            return result.RequestHeaders?.Any(x => x.Key == "ACCESS-SIGN") == true;
        }
    }
}
