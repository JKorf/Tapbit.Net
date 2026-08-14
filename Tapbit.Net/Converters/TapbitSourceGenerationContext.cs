using System;
using System.Text.Json.Serialization;
using Tapbit.Net.Enums;
using Tapbit.Net.Objects.Models;

namespace Tapbit.Net.Converters
{
    [JsonSerializable(typeof(TapbitResponse<TapbitServerTime>))]
    [JsonSerializable(typeof(TapbitResponse<TapbitSymbol>))]
    [JsonSerializable(typeof(TapbitResponse<TapbitSymbol[]>))]
    [JsonSerializable(typeof(TapbitResponse<TapbitOrderBook>))]
    [JsonSerializable(typeof(TapbitResponse<TapbitTicker>))]
    [JsonSerializable(typeof(TapbitResponse<TapbitTicker[]>))]
    [JsonSerializable(typeof(TapbitResponse<TapbitKline[]>))]
    [JsonSerializable(typeof(TapbitResponse<TapbitTrade[]>))]
    [JsonSerializable(typeof(TapbitResponse<TapbitAsset[]>))]
    [JsonSerializable(typeof(TapbitResponse<TapbitBalance[]>))]
    [JsonSerializable(typeof(OrderSide))]

    [JsonSerializable(typeof(string))]
    [JsonSerializable(typeof(int?))]
    [JsonSerializable(typeof(int))]
    [JsonSerializable(typeof(long?))]
    [JsonSerializable(typeof(long))]
    [JsonSerializable(typeof(decimal))]
    [JsonSerializable(typeof(decimal?))]
    [JsonSerializable(typeof(DateTime))]
    [JsonSerializable(typeof(DateTime?))]
    internal partial class TapbitSourceGenerationContext : JsonSerializerContext
    {
    }
}
