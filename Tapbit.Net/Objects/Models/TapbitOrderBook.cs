using CryptoExchange.Net.Converters;
using CryptoExchange.Net.Converters.SystemTextJson;
using CryptoExchange.Net.Interfaces;
using System;
using System.Text.Json.Serialization;
using Tapbit.Net.Enums;

namespace Tapbit.Net.Objects.Models;

/// <summary>
/// Order book snapshot
/// </summary>
public record TapbitOrderBook
{
    /// <summary>
    /// ["<c>bids</c>"] Bids
    /// </summary>
    [JsonPropertyName("bids")]
    public TapbitOrderBookEntry[] Bids { get; set; } = [];
    /// <summary>
    /// ["<c>asks</c>"] Asks
    /// </summary>
    [JsonPropertyName("asks")]
    public TapbitOrderBookEntry[] Asks { get; set; } = [];
    /// <summary>
    /// ["<c>timestamp</c>"] Timestamp
    /// </summary>
    [JsonPropertyName("timestamp")]
    public DateTime? Timestamp { get; set; }
}

/// <summary>
/// Order book entry
/// </summary>
[JsonConverter(typeof(ArrayConverter<TapbitOrderBookEntry>))]
public record TapbitOrderBookEntry : ISymbolOrderBookEntry
{
    /// <summary>
    /// Price
    /// </summary>
    [ArrayProperty(0)]
    public decimal Price { get; set; }
    /// <summary>
    /// Quantity
    /// </summary>
    [ArrayProperty(1)]
    public decimal Quantity { get; set; }

}