using CryptoExchange.Net.Objects;
using CryptoExchange.Net.RateLimiting.Interfaces;
using CryptoExchange.Net.RateLimiting;
using System;
using CryptoExchange.Net.SharedApis;
using Tapbit.Net.Converters;
using System.Text.Json;
using CryptoExchange.Net.Converters.SystemTextJson;
using CryptoExchange.Net.Converters;

namespace Tapbit.Net
{
    /// <summary>
    /// Tapbit exchange information and configuration
    /// </summary>
    public static class TapbitExchange
    {
        internal static JsonSerializerOptions _serializerContext = SerializerOptions.WithConverters(JsonSerializerContextCache.GetOrCreate<TapbitSourceGenerationContext>());
        internal static ParameterSerializationSettings _parameterSerializationSettings = new ParameterSerializationSettings
        {
            
        };

        /// <summary>
        /// Platform metadata
        /// </summary>
        public static PlatformInfo Metadata { get; } = new PlatformInfo(
                "Tapbit",
                "Tapbit",
                "https://raw.githubusercontent.com/JKorf/Tapbit.Net/master/Tapbit.Net/Icon/icon.png",
                "https://www.tapbit.com",
                ["https://www.tapbit.com/openapi-docs/"],
                PlatformType.CryptoCurrencyExchange,
                CentralizationType.Centralized,
                TapbitEnvironment.All
                );

        /// <summary>
        /// Aliases for Tapbit assets
        /// </summary>
        public static AssetAliasConfiguration AssetAliases { get; } = new AssetAliasConfiguration
        {
            Aliases = [
                new AssetAlias("USDT", SharedSymbol.UsdOrStable.ToUpperInvariant(), AliasType.OnlyToExchange)
            ]
        };

        /// <summary>
        /// Format a base and quote asset to an Tapbit recognized symbol 
        /// </summary>
        /// <param name="baseAsset">Base asset</param>
        /// <param name="quoteAsset">Quote asset</param>
        /// <param name="tradingMode">Trading mode</param>
        /// <param name="deliverTime">Delivery time for delivery futures</param>
        /// <returns></returns>
        public static string FormatSymbol(string baseAsset, string quoteAsset, TradingMode tradingMode, DateTime? deliverTime = null)
        {
            baseAsset = AssetAliases.CommonToExchangeName(baseAsset.ToUpperInvariant());
            quoteAsset = AssetAliases.CommonToExchangeName(quoteAsset.ToUpperInvariant());

            return baseAsset + "/" + quoteAsset;
        }

        /// <summary>
        /// Rate limiter configuration for the Tapbit API
        /// </summary>
        public static TapbitRateLimiters RateLimiter { get; } = new TapbitRateLimiters();
    }

    /// <summary>
    /// Rate limiter configuration for the Tapbit API
    /// </summary>
    public class TapbitRateLimiters
    {
        /// <summary>
        /// Event for when a rate limit is triggered
        /// </summary>
        public event Action<RateLimitEvent> RateLimitTriggered;
        /// <summary>
        /// Event when the rate limit is updated. Note that it's only updated when a request is send, so there are no specific updates when the current usage is decaying.
        /// </summary>
        public event Action<RateLimitUpdateEvent> RateLimitUpdated;

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        internal TapbitRateLimiters()
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        {
            Initialize();
        }

        private void Initialize()
        {
            Tapbit = new RateLimitGate("Tapbit");
            Tapbit.RateLimitTriggered += (x) => RateLimitTriggered?.Invoke(x);
            Tapbit.RateLimitUpdated += (x) => RateLimitUpdated?.Invoke(x);
        }


        internal IRateLimitGate Tapbit { get; private set; }

    }
}
