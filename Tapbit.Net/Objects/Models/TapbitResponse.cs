using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Tapbit.Net.Objects.Models
{
    internal class TapbitResponse
    {
        [JsonPropertyName("code")]
        public int Code { get; set; }
        [JsonPropertyName("message")]
        public string? Message { get; set; }
    }

    internal class TapbitResponse<T> : TapbitResponse
    {
        [JsonPropertyName("data")]
        public T Data { get; set; } = default!;
    }
}
