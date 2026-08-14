using System;
using System.Text.Json.Serialization;
using Tapbit.Net.Enums;

namespace Tapbit.Net.Objects.Models;

/// <summary>
/// Symbol info
/// </summary>
public record TapbitSymbol
{
    /// <summary>
    /// ["<c>trade_pair_name</c>"] Symbol name
    /// </summary>
    [JsonPropertyName("trade_pair_name")]
    public string Symbol { get; set; } = string.Empty;
    /// <summary>
    /// ["<c>base_asset</c>"] Base asset
    /// </summary>
    [JsonPropertyName("base_asset")]
    public string BaseAsset { get; set; } = string.Empty;
    /// <summary>
    /// ["<c>quote_asset</c>"] Quote asset
    /// </summary>
    [JsonPropertyName("quote_asset")]
    public string QuoteAsset { get; set; } = string.Empty;
    /// <summary>
    /// ["<c>price_precision</c>"] Price precision
    /// </summary>
    [JsonPropertyName("price_precision")]
    public int PricePrecision { get; set; }
    /// <summary>
    /// ["<c>amount_precision</c>"] Quantity precision
    /// </summary>
    [JsonPropertyName("amount_precision")]
    public int QuantityPrecision { get; set; }
    /// <summary>
    /// ["<c>taker_fee_rate</c>"] Taker fee rate
    /// </summary>
    [JsonPropertyName("taker_fee_rate")]
    public decimal TakerFeeRate { get; set; }
    /// <summary>
    /// ["<c>maker_fee_rate</c>"] Maker fee rate
    /// </summary>
    [JsonPropertyName("maker_fee_rate")]
    public decimal MakerFeeRate { get; set; }
    /// <summary>
    /// ["<c>min_amount</c>"] Min order quantity
    /// </summary>
    [JsonPropertyName("min_amount")]
    public decimal MinQuantity { get; set; }
    /// <summary>
    /// ["<c>price_fluctuation</c>"] Price fluctuation
    /// </summary>
    [JsonPropertyName("price_fluctuation")]
    public decimal PriceFluctuation { get; set; }
    /// <summary>
    /// ["<c>min_notional</c>"] Min notional value
    /// </summary>
    [JsonPropertyName("min_notional")]
    public decimal MinNotional { get; set; }
}

