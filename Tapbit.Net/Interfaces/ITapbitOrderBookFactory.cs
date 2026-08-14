using CryptoExchange.Net.Interfaces;
using CryptoExchange.Net.SharedApis;
using System;
using Tapbit.Net.Objects.Options;

namespace Tapbit.Net.Interfaces
{
    /// <summary>
    /// Tapbit local order book factory
    /// </summary>
    public interface ITapbitOrderBookFactory
    {
                /// <summary>
        /// UsdtPerpetualFutures order book factory methods
        /// </summary>
        IOrderBookFactory<TapbitOrderBookOptions> UsdtPerpetualFutures { get; }

        /// <summary>
        /// Spot order book factory methods
        /// </summary>
        IOrderBookFactory<TapbitOrderBookOptions> Spot { get; }


        /// <summary>
        /// Create a SymbolOrderBook for the symbol
        /// </summary>
        /// <param name="symbol">The symbol</param>
        /// <param name="options">Book options</param>
        /// <returns></returns>
        ISymbolOrderBook Create(SharedSymbol symbol, Action<TapbitOrderBookOptions>? options = null);

                /// <summary>
        /// Create a new UsdtPerpetualFutures local order book instance
        /// </summary>
        ISymbolOrderBook CreateUsdtPerpetualFutures(string symbol, Action<TapbitOrderBookOptions>? options = null);

        /// <summary>
        /// Create a new Spot local order book instance
        /// </summary>
        ISymbolOrderBook CreateSpot(string symbol, Action<TapbitOrderBookOptions>? options = null);

    }
}