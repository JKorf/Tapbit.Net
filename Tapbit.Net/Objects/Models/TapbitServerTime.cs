using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Tapbit.Net.Objects.Models
{
    internal class TapbitServerTime
    {
        [JsonPropertyName("timestamp")]
        public DateTime Timestamp { get; set; }
    }
}
