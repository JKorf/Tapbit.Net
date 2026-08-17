using CryptoExchange.Net.Attributes;
using CryptoExchange.Net.Converters.SystemTextJson;
using System.Text.Json.Serialization;

namespace Tapbit.Net.Enums;

/// <summary>
/// Order status
/// </summary>
[JsonConverter(typeof(EnumConverter<OrderStatus>))]
public enum OrderStatus
{
    /// <summary>
    /// ["<c>Open</c>"] Open
    /// </summary>
    [Map("Open")]
    Open,
    /// <summary>
    /// ["<c>Filled</c>"] Filled
    /// </summary>
    [Map("Complete", "Filled")]
    Filled,
    /// <summary>
    /// ["<c>Cancelled</c>"] Canceled
    /// </summary>
    [Map("Cancelled")]
    Canceled,
    /// <summary>
    /// ["<c>Partially Cancelled</c>"] Partially canceled
    /// </summary>
    [Map("Partially Cancelled")]
    PartiallyCanceled,
}
