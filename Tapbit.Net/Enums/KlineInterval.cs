using CryptoExchange.Net.Attributes;
using CryptoExchange.Net.Converters.SystemTextJson;
using System.Text.Json.Serialization;

namespace Tapbit.Net.Enums;

/// <summary>
/// Kline interval
/// </summary>
[JsonConverter(typeof(EnumConverter<KlineInterval>))]
public enum KlineInterval
{
    /// <summary>
    /// ["<c>1</c>"] One minute
    /// </summary>
    [Map("1")]
    OneMinute = 60,
    /// <summary>
    /// ["<c>3</c>"] Three minutes
    /// </summary>
    [Map("3")]
    ThreeMinutes = 60 * 3,
    /// <summary>
    /// ["<c>5</c>"] Five minutes
    /// </summary>
    [Map("5")]
    FiveMinutes = 60 * 5,
    /// <summary>
    /// ["<c>15</c>"] Fifteen minutes
    /// </summary>
    [Map("15")]
    FifteenMinutes = 60 * 15,
    /// <summary>
    /// ["<c>30</c>"] Thirty minutes
    /// </summary>
    [Map("30")]
    ThirtyMinutes = 60 * 30,
    /// <summary>
    /// ["<c>60</c>"] One hour
    /// </summary>
    [Map("60")]
    OneHour = 60 * 60,
    /// <summary>
    /// ["<c>120</c>"] Two hours
    /// </summary>
    [Map("120")]
    TwoHours = 60 * 60 * 2,
    /// <summary>
    /// ["<c>240</c>"] Four hours
    /// </summary>
    [Map("240")]
    FourHours = 60 * 60 * 4,
    /// <summary>
    /// ["<c>360</c>"] Six hours
    /// </summary>
    [Map("360")]
    SixHours = 60 * 60 * 6,
    /// <summary>
    /// ["<c>720</c>"] Twelve hours
    /// </summary>
    [Map("720")]
    TwelveHours = 60 * 60 * 12,
    /// <summary>
    /// ["<c>D</c>"] One day
    /// </summary>
    [Map("D")]
    OneDay = 60 * 60 * 24,
    /// <summary>
    /// ["<c>W</c>"] One week
    /// </summary>
    [Map("W")]
    OneWeek = 60 * 60 * 24 * 7,
    /// <summary>
    /// ["<c>M</c>"] One month
    /// </summary>
    [Map("M")]
    OneMonth = 60 * 60 * 24 * 30,
}
