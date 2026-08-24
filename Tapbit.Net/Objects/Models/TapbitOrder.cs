using System;
using System.Text.Json.Serialization;
using Tapbit.Net.Enums;

namespace Tapbit.Net.Objects.Models;

/// <summary>
/// Order info
/// </summary>
public record TapbitOrder
{
    /// <summary>
    /// ["<c>order_id</c>"] Order id
    /// </summary>
    [JsonPropertyName("order_id")]
    public long OrderId { get; set; }
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
    /// ["<c>direction</c>"] Order side
    /// </summary>
    [JsonPropertyName("direction")]
    public OrderSide Side { get; set; } 
    /// <summary>
    /// ["<c>quantity</c>"] Quantity, 0 for market buy order
    /// </summary>
    [JsonPropertyName("quantity")]
    public decimal Quantity { get; set; }
    /// <summary>
    /// ["<c>filled_quantity</c>"] Quantity filled
    /// </summary>
    [JsonPropertyName("filled_quantity")]
    public decimal QuantityFilled { get; set; }
    /// <summary>
    /// ["<c>amount</c>"] Quantity is quote asset
    /// </summary>
    [JsonPropertyName("amount")]
    public decimal QuoteQuantity { get; set; }
    /// <summary>
    /// ["<c>filled_amount</c>"] Quantity filled in quote asset
    /// </summary>
    [JsonPropertyName("filled_amount")]
    public decimal QuoteQuantityFilled { get; set; }
    /// <summary>
    /// ["<c>average_price</c>"] Average price
    /// </summary>
    [JsonPropertyName("average_price")]
    public decimal? AveragePrice { get; set; }
    /// <summary>
    /// ["<c>status</c>"] Status
    /// </summary>
    [JsonPropertyName("status")]
    public OrderStatus Status { get; set; }
    /// <summary>
    /// ["<c>order_time</c>"] Order timestamp
    /// </summary>
    [JsonPropertyName("order_time")]
    public DateTime Timestamp { get; set; }
    /// <summary>
    /// ["<c>fee</c>"] Fee
    /// </summary>
    [JsonPropertyName("fee")]
    public decimal Fee { get; set; }
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
    /// ["<c>trade_pair_name</c>"] Symbol
    /// </summary>
    [JsonPropertyName("trade_pair_name")]
    public string Symbol { get; set; } = string.Empty;
    /// <summary>
    /// ["<c>price</c>"] Limit price
    /// </summary>
    [JsonPropertyName("price")]
    public decimal Price { get; set; }
    /// <summary>
    /// ["<c>order_type</c>"] Order type
    /// </summary>
    [JsonPropertyName("order_type")]
    public OrderType OrderType { get; set; }
}

