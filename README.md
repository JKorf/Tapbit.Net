# ![Tapbit.Net](https://raw.githubusercontent.com/JKorf/Tapbit.Net/main/Tapbit.Net/Icon/icon.png) Tapbit.Net  

[![.NET](https://img.shields.io/github/actions/workflow/status/JKorf/Tapbit.Net/dotnet.yml?style=for-the-badge)](https://github.com/JKorf/Tapbit.Net/actions/workflows/dotnet.yml) ![License](https://img.shields.io/github/license/JKorf/Tapbit.Net?style=for-the-badge)

Tapbit.Net is a client library for accessing the [Tapbit spot REST API](https://www.tapbit.com/openapi-docs/spot_v2/).

## Features
* Response data is mapped to descriptive models
* Input parameters and response values are mapped to discriptive enum values where possible
* High performance
* Client side rate limiting 
* Support for managing different accounts
* Extensive logging
* Support for different environments
* Easy integration with other exchange clients based on the CryptoExchange.Net base library
* Native AOT support

## Documentation

The [Tapbit.Net documentation](https://cryptoexchange.jkorf.dev/docs/exchange-clients?library=Tapbit.Net) is the main resource for installing, configuring, and using the library.

| Resource | Description |
|--|--|
| [Client guide](https://cryptoexchange.jkorf.dev/docs/exchange-clients?library=Tapbit.Net) | Installation, REST clients, authentication, dependency injection, error handling, and advanced features |
| [Examples](https://cryptoexchange.jkorf.dev/docs/exchange-clients/examples?library=Tapbit.Net) | Common REST operations |
| [API reference](https://cryptoexchange.jkorf.dev/docs/exchange-clients/reference?library=Tapbit.Net) | Client interfaces, methods, and properties |
| [Shared API guide](https://cryptoexchange.jkorf.dev/docs/shared-api) | Common interfaces and models for working with multiple exchanges |

## Supported Frameworks
The library is targeting both `.NET Standard 2.0` and `.NET Standard 2.1` for optimal compatibility, as well as the latest dotnet versions to use the latest framework features.

|.NET implementation|Version Support|
|--|--|
|.NET Core|`2.0` and higher|
|.NET Framework|`4.6.1` and higher|
|Mono|`5.4` and higher|
|Xamarin.iOS|`10.14` and higher|
|Xamarin.Android|`8.0` and higher|
|UWP|`10.0.16299` and higher|
|Unity|`2018.1` and higher|

## Install the library

### NuGet 
[![NuGet version](https://img.shields.io/nuget/v/Tapbit.net.svg?style=for-the-badge)](https://www.nuget.org/packages/Tapbit.Net)  [![Nuget downloads](https://img.shields.io/nuget/dt/Tapbit.Net.svg?style=for-the-badge)](https://www.nuget.org/packages/Tapbit.Net)

	dotnet add package Tapbit.Net
	
### GitHub packages
Tapbit.Net is available on [GitHub packages](https://github.com/JKorf/Tapbit.Net/pkgs/nuget/Tapbit.Net). You'll need to add `https://nuget.pkg.github.com/JKorf/index.json` as a NuGet package source.

### Download release
[![GitHub Release](https://img.shields.io/github/v/release/JKorf/Tapbit.Net?style=for-the-badge&label=GitHub)](https://github.com/JKorf/Tapbit.Net/releases)

The NuGet package files are added along side the source with the latest GitHub release which can found [here](https://github.com/JKorf/Tapbit.Net/releases).

## How to use
*Basic request:* 
```csharp
// Get the ETH/USDT ticker via rest request
var restClient = new TapbitRestClient();
var tickerResult = await restClient.SpotApi.ExchangeData.GetTickerAsync("ETH/USDT");
if (tickerResult.Success)
    Console.WriteLine(tickerResult.Data.LastPrice);
```

*Place order:*
```csharp
var restClient = new TapbitRestClient(opts => {
	opts.ApiCredentials = new TapbitCredentials("APIKEY", "APISECRET");
});

// Place a spot limit order for 0.1 ETH at 2000 USDT
var orderResult = await restClient.SpotApi.Trading.PlaceOrderAsync(
    "ETH/USDT",
    OrderSide.Buy,
    0.1m,
    2000m);
```

For more examples and explanations, continue with the [Tapbit.Net documentation](https://cryptoexchange.jkorf.dev/docs/exchange-clients?library=Tapbit.Net) or browse the [compilable repository examples](https://github.com/JKorf/Tapbit.Net/tree/main/Examples).

## AI / LLM documentation

Tapbit.Net includes AI-oriented documentation and examples for code generation tools:

|File|Purpose|
|--|--|
|[`AGENTS.md`](AGENTS.md)|Assistant skill with core Tapbit.Net patterns, pitfalls, and examples|
|[`llms.txt`](llms.txt)|Short LLM index with links to docs, examples, and critical usage rules|
|[`llms-full.txt`](llms-full.txt)|Detailed LLM context with endpoint routing, authentication levels, code patterns, and anti-hallucination checks|
|[`docs/ai-api-map.md`](docs/ai-api-map.md)|Table-style intent-to-method map|
|[`Examples/ai-friendly`](Examples/ai-friendly)|Compilable single-file examples for spot REST, authentication/trading, batch orders, shared APIs, and error handling|

See [cryptoexchange-skills-hub](https://github.com/JKorf/cryptoexchange-skills-hub) for installable skills.

## Shared / unified API

The CryptoExchange.Net [Shared APIs](https://cryptoexchange.jkorf.dev/docs/shared-api) provide exchange-agnostic, unified interfaces for common operations such as retrieving tickers, order books and balances, and placing orders.

This allows the same application code to work with different exchange libraries. The supported Tapbit API surface exposes shared functionality through a `SharedClient` property. Because support differs between exchanges and API surfaces, call `Discover()` to inspect the available trading modes, environments, and endpoints at runtime.

### Supported shared interfaces

| API | Type | Supported interfaces |
|--|--|--|
| Spot | REST | `IAssetsRestClient`, `IBalanceRestClient`, `IKlineRestClient`, `IOrderBookRestClient`, `IRecentTradeRestClient`, `ISpotSymbolRestClient`, `ISpotTickerRestClient`, `ISpotOrderRestClient` |

### Discover supported functionality

```csharp
var sharedClient = new TapbitRestClient().SpotApi.SharedClient;
var clientInfo = sharedClient.Discover();

Console.WriteLine(clientInfo);
```

### Example

```csharp
using Tapbit.Net.Clients;
using CryptoExchange.Net.SharedApis;

var sharedClient = new TapbitRestClient().SpotApi.SharedClient;
ISpotTickerRestClient tickerClient = sharedClient;

var symbol = new SharedSymbol(TradingMode.Spot, "ETH", "USDT");
var result = await tickerClient.GetSpotTickerAsync(
    new GetTickerRequest(symbol));

if (!result.Success)
{
    Console.WriteLine(result.Error);
    return;
}

Console.WriteLine(result.Data.LastPrice);
```

The request and response models belong to `CryptoExchange.Net.SharedApis`, so the same pattern can be used with another exchange's `SharedClient`.

## CryptoExchange.Net
Tapbit.Net is based on the [CryptoExchange.Net](https://github.com/JKorf/CryptoExchange.Net) base library. Other exchange API implementations based on the CryptoExchange.Net base library are available and follow the same logic.

CryptoExchange.Net also provides [shared access to different exchange APIs](https://cryptoexchange.jkorf.dev/docs/shared-api).

|Exchange|Repository|Nuget|
|--|--|--|
|Aster|[JKorf/Aster.Net](https://github.com/JKorf/Aster.Net)|[![Nuget version](https://img.shields.io/nuget/v/JKorf.Aster.net.svg?style=flat-square)](https://www.nuget.org/packages/JKorf.Aster.Net)|
|Binance|[JKorf/Binance.Net](https://github.com/JKorf/Binance.Net)|[![Nuget version](https://img.shields.io/nuget/v/Binance.net.svg?style=flat-square)](https://www.nuget.org/packages/Binance.Net)|
|BingX|[JKorf/BingX.Net](https://github.com/JKorf/BingX.Net)|[![Nuget version](https://img.shields.io/nuget/v/JK.BingX.net.svg?style=flat-square)](https://www.nuget.org/packages/JK.BingX.Net)|
|Bitfinex|[JKorf/Bitfinex.Net](https://github.com/JKorf/Bitfinex.Net)|[![Nuget version](https://img.shields.io/nuget/v/Bitfinex.net.svg?style=flat-square)](https://www.nuget.org/packages/Bitfinex.Net)|
|Bitget|[JKorf/Bitget.Net](https://github.com/JKorf/Bitget.Net)|[![Nuget version](https://img.shields.io/nuget/v/JK.Bitget.net.svg?style=flat-square)](https://www.nuget.org/packages/JK.Bitget.Net)|
|BitMart|[JKorf/BitMart.Net](https://github.com/JKorf/BitMart.Net)|[![Nuget version](https://img.shields.io/nuget/v/BitMart.net.svg?style=flat-square)](https://www.nuget.org/packages/BitMart.Net)|
|BitMEX|[JKorf/BitMEX.Net](https://github.com/JKorf/BitMEX.Net)|[![Nuget version](https://img.shields.io/nuget/v/JKorf.BitMEX.net.svg?style=flat-square)](https://www.nuget.org/packages/JKorf.BitMEX.Net)|
|Bitstamp|[JKorf/Bitstamp.Net](https://github.com/JKorf/Bitstamp.Net)|[![Nuget version](https://img.shields.io/nuget/v/Bitstamp.Net.svg?style=flat-square)](https://www.nuget.org/packages/Bitstamp.Net)|
|BloFin|[JKorf/BloFin.Net](https://github.com/JKorf/BloFin.Net)|[![Nuget version](https://img.shields.io/nuget/v/BloFin.net.svg?style=flat-square)](https://www.nuget.org/packages/BloFin.Net)|
|Bybit|[JKorf/Bybit.Net](https://github.com/JKorf/Bybit.Net)|[![Nuget version](https://img.shields.io/nuget/v/Bybit.net.svg?style=flat-square)](https://www.nuget.org/packages/Bybit.Net)|
|Coinbase|[JKorf/Coinbase.Net](https://github.com/JKorf/Coinbase.Net)|[![Nuget version](https://img.shields.io/nuget/v/JKorf.Coinbase.net.svg?style=flat-square)](https://www.nuget.org/packages/JKorf.Coinbase.Net)|
|CoinEx|[JKorf/CoinEx.Net](https://github.com/JKorf/CoinEx.Net)|[![Nuget version](https://img.shields.io/nuget/v/CoinEx.net.svg?style=flat-square)](https://www.nuget.org/packages/CoinEx.Net)|
|CoinGecko|[JKorf/CoinGecko.Net](https://github.com/JKorf/CoinGecko.Net)|[![Nuget version](https://img.shields.io/nuget/v/CoinGecko.net.svg?style=flat-square)](https://www.nuget.org/packages/CoinGecko.Net)|
|CoinW|[JKorf/CoinW.Net](https://github.com/JKorf/CoinW.Net)|[![Nuget version](https://img.shields.io/nuget/v/CoinW.net.svg?style=flat-square)](https://www.nuget.org/packages/CoinW.Net)|
|Crypto.com|[JKorf/CryptoCom.Net](https://github.com/JKorf/CryptoCom.Net)|[![Nuget version](https://img.shields.io/nuget/v/CryptoCom.net.svg?style=flat-square)](https://www.nuget.org/packages/CryptoCom.Net)|
|DeepCoin|[JKorf/DeepCoin.Net](https://github.com/JKorf/DeepCoin.Net)|[![Nuget version](https://img.shields.io/nuget/v/DeepCoin.net.svg?style=flat-square)](https://www.nuget.org/packages/DeepCoin.Net)|
|Gate.io|[JKorf/GateIo.Net](https://github.com/JKorf/GateIo.Net)|[![Nuget version](https://img.shields.io/nuget/v/GateIo.net.svg?style=flat-square)](https://www.nuget.org/packages/GateIo.Net)|
|HTX|[JKorf/HTX.Net](https://github.com/JKorf/HTX.Net)|[![Nuget version](https://img.shields.io/nuget/v/JKorf.HTX.net.svg?style=flat-square)](https://www.nuget.org/packages/Jkorf.HTX.Net)|
|HyperLiquid|[JKorf/HyperLiquid.Net](https://github.com/JKorf/HyperLiquid.Net)|[![Nuget version](https://img.shields.io/nuget/v/HyperLiquid.Net.svg?style=flat-square)](https://www.nuget.org/packages/HyperLiquid.Net)|
|Kraken|[JKorf/Kraken.Net](https://github.com/JKorf/Kraken.Net)|[![Nuget version](https://img.shields.io/nuget/v/KrakenExchange.net.svg?style=flat-square)](https://www.nuget.org/packages/KrakenExchange.Net)|
|Kucoin|[JKorf/Kucoin.Net](https://github.com/JKorf/Kucoin.Net)|[![Nuget version](https://img.shields.io/nuget/v/Kucoin.net.svg?style=flat-square)](https://www.nuget.org/packages/Kucoin.Net)|
|LBank|[JKorf/LBank.Net](https://github.com/JKorf/LBank.Net)|[![Nuget version](https://img.shields.io/nuget/v/LBank.net.svg?style=flat-square)](https://www.nuget.org/packages/LBank.Net)|
|Lighter|[JKorf/Lighter.Net](https://github.com/JKorf/Lighter.Net)|[![Nuget version](https://img.shields.io/nuget/v/JKorf.Lighter.net.svg?style=flat-square)](https://www.nuget.org/packages/JKorf.Lighter.Net)|
|Mexc|[JKorf/Mexc.Net](https://github.com/JKorf/Mexc.Net)|[![Nuget version](https://img.shields.io/nuget/v/JK.Mexc.net.svg?style=flat-square)](https://www.nuget.org/packages/JK.Mexc.Net)|
|OKX|[JKorf/OKX.Net](https://github.com/JKorf/OKX.Net)|[![Nuget version](https://img.shields.io/nuget/v/JK.OKX.net.svg?style=flat-square)](https://www.nuget.org/packages/JK.OKX.Net)|
|Pionex|[JKorf/Pionex.Net](https://github.com/JKorf/Pionex.Net)|[![Nuget version](https://img.shields.io/nuget/v/Pionex.net.svg?style=flat-square)](https://www.nuget.org/packages/Pionex.Net)|
|Polymarket|[JKorf/Polymarket.Net](https://github.com/JKorf/Polymarket.Net)|[![Nuget version](https://img.shields.io/nuget/v/Polymarket.net.svg?style=flat-square)](https://www.nuget.org/packages/Polymarket.Net)|
|Toobit|[JKorf/Toobit.Net](https://github.com/JKorf/Toobit.Net)|[![Nuget version](https://img.shields.io/nuget/v/Toobit.net.svg?style=flat-square)](https://www.nuget.org/packages/Toobit.Net)|
|Upbit|[JKorf/Upbit.Net](https://github.com/JKorf/Upbit.Net)|[![Nuget version](https://img.shields.io/nuget/v/JKorf.Upbit.net.svg?style=flat-square)](https://www.nuget.org/packages/JKorf.Upbit.Net)|
|Weex|[JKorf/Weex.Net](https://github.com/JKorf/Weex.Net)|[![Nuget version](https://img.shields.io/nuget/v/Weex.net.svg?style=flat-square)](https://www.nuget.org/packages/Weex.Net)|
|WhiteBit|[JKorf/WhiteBit.Net](https://github.com/JKorf/WhiteBit.Net)|[![Nuget version](https://img.shields.io/nuget/v/WhiteBit.net.svg?style=flat-square)](https://www.nuget.org/packages/WhiteBit.Net)|
|XT|[JKorf/XT.Net](https://github.com/JKorf/XT.Net)|[![Nuget version](https://img.shields.io/nuget/v/XT.net.svg?style=flat-square)](https://www.nuget.org/packages/XT.Net)|

When using multiple of these API's the [CryptoClients.Net](https://github.com/JKorf/CryptoClients.Net) package can be used which combines this and the other packages and allows easy access to all exchange API's.

## Discord
[![Nuget version](https://img.shields.io/discord/847020490588422145?style=for-the-badge)](https://discord.gg/MSpeEtSY8t)  
A Discord server is available [here](https://discord.gg/MSpeEtSY8t). For discussion and/or questions around the CryptoExchange.Net and implementation libraries, feel free to join.

## Supported functionality

### Spot
|API|Supported|Location|
|--|--:|--|
|Market data|✓|`restClient.SpotApi.ExchangeData`|
|Account/Trades|✓|`restClient.SpotApi.Account` / `restClient.SpotApi.Trading`|

## Support the project
Any support is greatly appreciated.

### Referral
If you do not yet have an account, please consider using this referral link to sign up:

[Link](https://www.tapbit.com/en/invite/SPBGMSK)

### Donate
Make a one time donation in a crypto currency of your choice. If you prefer to donate in a different currency or network send me a message.
   
**USDT (TRX)**  TKigKeJPXZYyMVDgMyXxMf17MWYia92Rjd 

### Sponsor
Alternatively, sponsor me on Github using [Github Sponsors](https://github.com/sponsors/JKorf). 

## Release notes
