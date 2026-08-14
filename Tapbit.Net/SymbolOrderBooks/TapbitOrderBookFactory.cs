using CryptoExchange.Net.Interfaces;
using CryptoExchange.Net.SharedApis;
using CryptoExchange.Net.OrderBook;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using Tapbit.Net.Interfaces;
using Tapbit.Net.Interfaces.Clients;
using Tapbit.Net.Objects.Options;

namespace Tapbit.Net.SymbolOrderBooks
{
    /// <summary>
    /// Tapbit order book factory
    /// </summary>
    public class TapbitOrderBookFactory : ITapbitOrderBookFactory
    {
        private readonly IServiceProvider _serviceProvider;

        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="serviceProvider">Service provider for resolving logging and clients</param>
        public TapbitOrderBookFactory(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            
                        UsdtPerpetualFutures = new OrderBookFactory<TapbitOrderBookOptions>(CreateUsdtPerpetualFutures, Create);

            Spot = new OrderBookFactory<TapbitOrderBookOptions>(CreateSpot, Create);

        }

                 /// <inheritdoc />
        public IOrderBookFactory<TapbitOrderBookOptions> UsdtPerpetualFutures { get; }

         /// <inheritdoc />
        public IOrderBookFactory<TapbitOrderBookOptions> Spot { get; }


        /// <inheritdoc />
        public ISymbolOrderBook Create(SharedSymbol symbol, Action<TapbitOrderBookOptions>? options = null)
        {
            var symbolName = symbol.GetSymbol(TapbitExchange.FormatSymbol);
            throw new NotImplementedException();

#warning TODO
            //if (symbol.TradingMode == TradingMode.Spot)
            //    return CreateSpot(symbolName, options);

            //return Create(symbolName, options);
        }

                 /// <inheritdoc />
        public ISymbolOrderBook CreateUsdtPerpetualFutures(string symbol, Action<TapbitOrderBookOptions>? options = null)
            => new TapbitUsdtPerpetualFuturesSymbolOrderBook(symbol, options, 
                                                          _serviceProvider.GetRequiredService<ILoggerFactory>(),
                                                          _serviceProvider.GetRequiredService<ITapbitRestClient>(),
                                                          _serviceProvider.GetRequiredService<ITapbitSocketClient>());

         /// <inheritdoc />
        public ISymbolOrderBook CreateSpot(string symbol, Action<TapbitOrderBookOptions>? options = null)
            => new TapbitSpotSymbolOrderBook(symbol, options, 
                                                          _serviceProvider.GetRequiredService<ILoggerFactory>(),
                                                          _serviceProvider.GetRequiredService<ITapbitRestClient>(),
                                                          _serviceProvider.GetRequiredService<ITapbitSocketClient>());


    }
}
