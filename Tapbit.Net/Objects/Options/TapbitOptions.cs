using CryptoExchange.Net.Authentication;
using CryptoExchange.Net.Objects.Options;
using CryptoExchange.Net.SharedApis;

namespace Tapbit.Net.Objects.Options
{
    /// <summary>
    /// Tapbit options
    /// </summary>
    public class TapbitOptions : LibraryOptions<TapbitRestOptions, TapbitSocketOptions, TapbitCredentials, TapbitEnvironment>
    {
        /// <summary>
        /// Options for Shared API usage
        /// </summary>
        public SharedApiOptions SharedApi { get; set; } = new();
    }
}
