using CryptoExchange.Net.Attributes;
using CryptoExchange.Net.Converters.SystemTextJson;
using System.Text.Json.Serialization;

namespace Tapbit.Net.Enums;

/// <summary>
/// Order side
/// </summary>
[JsonConverter(typeof(EnumConverter<OrderSide>))]
public enum OrderSide
{
    /// <summary>
    /// ["<c>buy</c>"] Buy
    /// </summary>
    [Map("buy")]
    Buy,
    /// <summary>
    /// ["<c>sell</c>"] Sell
    /// </summary>
    [Map("sell")]
    Sell,
}
