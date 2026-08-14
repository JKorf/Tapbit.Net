using System;
using System.Text.Json.Serialization;
using Tapbit.Net.Enums;

namespace Tapbit.Net.Objects.Models;

/// <summary>
/// Asset info
/// </summary>
public record TapbitAsset
{
    /// <summary>
    /// ["<c>currency</c>"] Asset
    /// </summary>
    [JsonPropertyName("currency")]
    public string Asset { get; set; } = string.Empty;
    /// <summary>
    /// ["<c>full_name</c>"] Full name
    /// </summary>
    [JsonPropertyName("full_name")]
    public string FullName { get; set; } = string.Empty;
    /// <summary>
    /// ["<c>chains</c>"] Networks
    /// </summary>
    [JsonPropertyName("chains")]
    public TapbitAssetNetwork[] Networks { get; set; } = [];
}

/// <summary>
/// Asset network
/// </summary>
public record TapbitAssetNetwork
{
    /// <summary>
    /// ["<c>chain</c>"] Network
    /// </summary>
    [JsonPropertyName("chain")]
    public string Network { get; set; } = string.Empty;
    /// <summary>
    /// ["<c>precision</c>"] Precision
    /// </summary>
    [JsonPropertyName("precision")]
    public int Precision { get; set; }
    /// <summary>
    /// ["<c>fee</c>"] Fee
    /// </summary>
    [JsonPropertyName("fee")]
    public decimal Fee { get; set; }
    /// <summary>
    /// ["<c>is_withdraw_enabled</c>"] Is withdraw enabled
    /// </summary>
    [JsonPropertyName("is_withdraw_enabled")]
    public bool IsWithdrawEnabled { get; set; }
    /// <summary>
    /// ["<c>is_deposit_enabled</c>"] Is deposit enabled
    /// </summary>
    [JsonPropertyName("is_deposit_enabled")]
    public bool IsDepositEnabled { get; set; }
    /// <summary>
    /// ["<c>deposit_min_confirm</c>"] Deposit minimal confirmations
    /// </summary>
    [JsonPropertyName("deposit_min_confirm")]
    public int DepositMinConfirm { get; set; }
    /// <summary>
    /// ["<c>withdraw_limit_min</c>"] Min withdrawal quantity
    /// </summary>
    [JsonPropertyName("withdraw_limit_min")]
    public decimal WithdrawLimitMin { get; set; }
}

