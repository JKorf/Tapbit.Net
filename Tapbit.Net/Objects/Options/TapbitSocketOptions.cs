using CryptoExchange.Net.Objects.Options;

namespace Tapbit.Net.Objects.Options
{
    /// <summary>
    /// Options for the TapbitSocketClient
    /// </summary>
    public class TapbitSocketOptions : SocketExchangeOptions<TapbitEnvironment, TapbitCredentials>
    {
        /// <summary>
        /// Default options for new clients
        /// </summary>
        internal static TapbitSocketOptions Default { get; set; } = new TapbitSocketOptions()
        {
            Environment = TapbitEnvironment.Live,
            SocketSubscriptionsCombineTarget = 10
        };

        /// <summary>
        /// ctor
        /// </summary>
        public TapbitSocketOptions()
        {
            Default?.Set(this);
        }

        internal TapbitSocketOptions Set(TapbitSocketOptions targetOptions)
        {
            targetOptions = base.Set<TapbitSocketOptions>(targetOptions);
            return targetOptions;
        }
    }
}
