using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Tapbit.Net.Enums;

namespace Tapbit.Net.Objects.Models
{
    /// <summary>
    /// Order request
    /// </summary>
    public record TapbitOrderRequest
    {
        /// <summary>
        /// ["<c>instrument_id</c>"] Symbol
        /// </summary>
        [JsonPropertyName("instrument_id")]
        public string Symbol { get; set; } = string.Empty;
        /// <summary>
        /// ["<c>direction</c>"] Side
        /// </summary>
        [JsonPropertyName("direction")]
        public OrderSide Side { get; set; }
        /// <summary>
        /// ["<c>price</c>"] Limit price
        /// </summary>
        [JsonPropertyName("price")]
        public decimal Price { get; set; }
        /// <summary>
        /// ["<c>quantity</c>"] Quantity
        /// </summary>
        [JsonPropertyName("quantity")]
        public decimal Quantity { get; set; }
    }
}
