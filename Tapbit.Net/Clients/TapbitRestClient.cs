using Microsoft.Extensions.Logging;
using System.Net.Http;
using System;
using Tapbit.Net.Interfaces.Clients;
using Tapbit.Net.Objects.Options;
using CryptoExchange.Net.Clients;
using Microsoft.Extensions.Options;
using Tapbit.Net.Interfaces.Clients.SpotApi;
using Tapbit.Net.Clients.SpotApi;

namespace Tapbit.Net.Clients
{
    /// <inheritdoc cref="ITapbitRestClient" />
    public class TapbitRestClient : BaseRestClient<TapbitEnvironment, TapbitCredentials>, ITapbitRestClient
    {
        #region Api clients

         /// <inheritdoc />
        public ITapbitRestClientSpotApi SpotApi { get; }

        #endregion

        #region constructor/destructor

        /// <summary>
        /// Create a new instance of the TapbitRestClient using provided options
        /// </summary>
        /// <param name="optionsDelegate">Option configuration delegate</param>
        public TapbitRestClient(Action<TapbitRestOptions>? optionsDelegate = null)
            : this(null, null, Options.Create(ApplyOptionsDelegate(optionsDelegate)))
        {
        }

        /// <summary>
        /// Create a new instance of the TapbitRestClient using provided options
        /// </summary>
        /// <param name="options">Option configuration</param>
        /// <param name="loggerFactory">The logger factory</param>
        /// <param name="httpClient">Http client for this client</param>
        public TapbitRestClient(HttpClient? httpClient, ILoggerFactory? loggerFactory, IOptions<TapbitRestOptions> options) : base(loggerFactory, "Tapbit")
        {
            Initialize(options.Value);

            SpotApi = AddApiClient(new TapbitRestClientSpotApi(loggerFactory, httpClient, options.Value));
        }

        #endregion

        /// <summary>
        /// Set the default options to be used when creating new clients
        /// </summary>
        /// <param name="optionsDelegate">Option configuration delegate</param>
        public static void SetDefaultOptions(Action<TapbitRestOptions> optionsDelegate)
        {
            TapbitRestOptions.Default = ApplyOptionsDelegate(optionsDelegate);
        }
    }
}
