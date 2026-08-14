using CryptoExchange.Net.Converters;
using CryptoExchange.Net.Converters.SystemTextJson;
using System;
using System.Text.Json.Serialization;
using Tapbit.Net.Enums;

namespace Tapbit.Net.Objects.Models;

/// <summary>
/// 
/// </summary>
[JsonConverter(typeof(ArrayConverter<TapbitTrade>))]
public record TapbitTrade
{
    /// <summary>
    /// Symbol name
    /// </summary>
    [ArrayProperty(0)]
    public string Symbol { get; set; } = string.Empty;
    /// <summary>
    /// Trade price
    /// </summary>
    [ArrayProperty(1)]
    public decimal Price { get; set; }
    /// <summary>
    /// Trade quantity
    /// </summary>
    [ArrayProperty(2)]
    public decimal Quantity { get; set; }
    /// <summary>
    /// Side
    /// </summary>
    [ArrayProperty(3), JsonConverter(typeof(EnumConverter<OrderSide>))]
    public OrderSide Side { get; set; }
    /// <summary>
    /// Trade time
    /// </summary>
    [ArrayProperty(4), JsonConverter(typeof(DateTimeConverter))]
    public DateTime Timestamp { get; set; }
}

