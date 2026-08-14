using CryptoExchange.Net.Clients;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using Tapbit.Net.Clients.SpotApi;
using Tapbit.Net.Clients.UsdtPerpetualFuturesApi;
using Tapbit.Net.Interfaces.Clients;
using Tapbit.Net.Interfaces.Clients.SpotApi;
using Tapbit.Net.Interfaces.Clients.UsdtPerpetualFuturesApi;
using Tapbit.Net.Objects.Options;

namespace Tapbit.Net.Clients
{
    /// <inheritdoc cref="ITapbitSocketClient" />
    public class TapbitSocketClient : BaseSocketClient<TapbitEnvironment, TapbitCredentials>, ITapbitSocketClient
    {
        #region fields
        #endregion

        #region Api clients

        /// <inheritdoc />
        public ITapbitSocketClientUsdtPerpetualFuturesApi UsdtPerpetualFuturesApi { get; }

         /// <inheritdoc />
        public ITapbitSocketClientSpotApi SpotApi { get; }

        #endregion

        #region constructor/destructor

        /// <summary>
        /// Create a new instance of TapbitSocketClient
        /// </summary>
        /// <param name="optionsDelegate">Option configuration delegate</param>
        public TapbitSocketClient(Action<TapbitSocketOptions>? optionsDelegate = null)
            : this(Options.Create(ApplyOptionsDelegate(optionsDelegate)), null)
        {
        }

        /// <summary>
        /// Create a new instance of TapbitSocketClient
        /// </summary>
        /// <param name="loggerFactory">The logger factory</param>
        /// <param name="options">Option configuration</param>
        public TapbitSocketClient(IOptions<TapbitSocketOptions> options, ILoggerFactory? loggerFactory = null) : base(loggerFactory, "Tapbit")
        {
            Initialize(options.Value);

            UsdtPerpetualFuturesApi = AddApiClient(new TapbitSocketClientUsdtPerpetualFuturesApi(loggerFactory, options.Value));
            SpotApi = AddApiClient(new TapbitSocketClientSpotApi(loggerFactory, options.Value));

        }
        #endregion

        /// <summary>
        /// Set the default options to be used when creating new clients
        /// </summary>
        /// <param name="optionsDelegate">Option configuration delegate</param>
        public static void SetDefaultOptions(Action<TapbitSocketOptions> optionsDelegate)
        {
            TapbitSocketOptions.Default = ApplyOptionsDelegate(optionsDelegate);
        }
    }
}
