using System;
using System.Text.Json.Serialization;
using Tapbit.Net.Enums;

namespace Tapbit.Net.Objects.Models;

/// <summary>
/// Balance info
/// </summary>
public record TapbitBalance
{
    /// <summary>
    /// ["<c>asset</c>"] Asset
    /// </summary>
    [JsonPropertyName("asset")]
    public string Asset { get; set; } = string.Empty;
    /// <summary>
    /// ["<c>available</c>"] Available
    /// </summary>
    [JsonPropertyName("available")]
    public decimal Available { get; set; }
    /// <summary>
    /// ["<c>frozen_balance</c>"] Frozen balance
    /// </summary>
    [JsonPropertyName("frozen_balance")]
    public decimal FrozenBalance { get; set; }
    /// <summary>
    /// ["<c>total_balance</c>"] Total balance
    /// </summary>
    [JsonPropertyName("total_balance")]
    public decimal TotalBalance { get; set; }
}

