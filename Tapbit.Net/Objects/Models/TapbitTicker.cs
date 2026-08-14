using System;
using System.Text.Json.Serialization;
using Tapbit.Net.Enums;

namespace Tapbit.Net.Objects.Models;

/// <summary>
/// Price ticker
/// </summary>
public record TapbitTicker
{
    /// <summary>
    /// ["<c>trade_pair_name</c>"] Symbol name
    /// </summary>
    [JsonPropertyName("trade_pair_name")]
    public string Symbol { get; set; } = string.Empty;
    /// <summary>
    /// ["<c>last_price</c>"] Last price
    /// </summary>
    [JsonPropertyName("last_price")]
    public decimal LastPrice { get; set; }
    /// <summary>
    /// ["<c>highest_bid</c>"] BestBid
    /// </summary>
    [JsonPropertyName("highest_bid")]
    public decimal BestBid { get; set; }
    /// <summary>
    /// ["<c>lowest_ask</c>"] BestAsk
    /// </summary>
    [JsonPropertyName("lowest_ask")]
    public decimal BestAsk { get; set; }
    /// <summary>
    /// ["<c>highest_price_24h</c>"] Highest price 24h
    /// </summary>
    [JsonPropertyName("highest_price_24h")]
    public decimal HighPrice24h { get; set; }
    /// <summary>
    /// ["<c>lowest_price_24h</c>"] Lowest price 24h
    /// </summary>
    [JsonPropertyName("lowest_price_24h")]
    public decimal LowPrice24h { get; set; }
    /// <summary>
    /// ["<c>volume24h</c>"] Volume 24h
    /// </summary>
    [JsonPropertyName("volume24h")]
    public decimal Volume24h { get; set; }
    /// <summary>
    /// ["<c>chg24h</c>"] Change 24h
    /// </summary>
    [JsonPropertyName("chg24h")]
    public decimal Change24h { get; set; }
    /// <summary>
    /// ["<c>chg0h</c>"] Change since midnight UTC
    /// </summary>
    [JsonPropertyName("chg0h")]
    public decimal Change0h { get; set; }
    /// <summary>
    /// ["<c>amount24h</c>"] Volume 24h in quote asset
    /// </summary>
    [JsonPropertyName("amount24h")]
    public decimal QuoteVolume24h { get; set; }
}

