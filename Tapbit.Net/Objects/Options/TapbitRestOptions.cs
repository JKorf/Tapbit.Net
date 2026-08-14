using CryptoExchange.Net.Objects.Options;

namespace Tapbit.Net.Objects.Options
{
    /// <summary>
    /// Options for the TapbitRestClient
    /// </summary>
    public class TapbitRestOptions : RestExchangeOptions<TapbitEnvironment, TapbitCredentials>
    {
        /// <summary>
        /// Default options for new clients
        /// </summary>
        internal static TapbitRestOptions Default { get; set; } = new TapbitRestOptions()
        {
            Environment = TapbitEnvironment.Live,
            AutoTimestamp = true
        };

        /// <summary>
        /// ctor
        /// </summary>
        public TapbitRestOptions()
        {
            Default?.Set(this);
        }

                 /// <summary>
        /// UsdtPerpetualFutures API options
        /// </summary>
        public RestApiOptions UsdtPerpetualFuturesOptions { get; private set; } = new RestApiOptions();

         /// <summary>
        /// Spot API options
        /// </summary>
        public RestApiOptions SpotOptions { get; private set; } = new RestApiOptions();


        internal TapbitRestOptions Set(TapbitRestOptions targetOptions)
        {
            targetOptions = base.Set<TapbitRestOptions>(targetOptions);
                        targetOptions.UsdtPerpetualFuturesOptions = UsdtPerpetualFuturesOptions.Set(targetOptions.UsdtPerpetualFuturesOptions);

            targetOptions.SpotOptions = SpotOptions.Set(targetOptions.SpotOptions);

            return targetOptions;
        }
    }
}
