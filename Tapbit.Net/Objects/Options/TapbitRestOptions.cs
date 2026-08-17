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
        /// Spot API options
        /// </summary>
        public RestApiOptions SpotOptions { get; private set; } = new RestApiOptions();


        internal TapbitRestOptions Set(TapbitRestOptions targetOptions)
        {
            targetOptions = base.Set<TapbitRestOptions>(targetOptions);
            targetOptions.SpotOptions = SpotOptions.Set(targetOptions.SpotOptions);
            return targetOptions;
        }
    }
}
