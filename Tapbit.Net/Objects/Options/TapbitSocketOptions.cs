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


                 /// <summary>
        /// UsdtPerpetualFutures API options
        /// </summary>
        public SocketApiOptions UsdtPerpetualFuturesOptions { get; private set; } = new SocketApiOptions();

         /// <summary>
        /// Spot API options
        /// </summary>
        public SocketApiOptions SpotOptions { get; private set; } = new SocketApiOptions();


        internal TapbitSocketOptions Set(TapbitSocketOptions targetOptions)
        {
            targetOptions = base.Set<TapbitSocketOptions>(targetOptions);
                        targetOptions.UsdtPerpetualFuturesOptions = UsdtPerpetualFuturesOptions.Set(targetOptions.UsdtPerpetualFuturesOptions);

            targetOptions.SpotOptions = SpotOptions.Set(targetOptions.SpotOptions);

            return targetOptions;
        }
    }
}
