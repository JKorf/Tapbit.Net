using CryptoExchange.Net.Attributes;
using CryptoExchange.Net.Converters.SystemTextJson;
using System.Text.Json.Serialization;

namespace Tapbit.Net.Enums;

/// <summary>
/// Order type
/// </summary>
[JsonConverter(typeof(EnumConverter<OrderType>))]
public enum OrderType
{
    /// <summary>
    /// ["<c>limit</c>"] Limit order
    /// </summary>
    [Map("limit")]
    Limit,
    /// <summary>
    /// ["<c>market</c>"] Market order
    /// </summary>
    [Map("market")]
    Market,
}
