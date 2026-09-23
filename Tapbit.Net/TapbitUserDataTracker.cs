using CryptoExchange.Net.Interfaces.Clients;
using CryptoExchange.Net.SharedApis;
using CryptoExchange.Net.Trackers.UserData;
using CryptoExchange.Net.Trackers.UserData.Objects;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tapbit.Net.Interfaces.Clients;

namespace Tapbit.Net
{
    /// <inheritdoc />
    public class TapbitUserSpotDataTracker : UserSpotDataTracker
    {
        /// <summary>
        /// ctor
        /// </summary>
        public TapbitUserSpotDataTracker(
            ILogger<TapbitUserSpotDataTracker> logger,
            ITapbitRestClient restClient,
            string? userIdentifier,
            SpotUserDataTrackerConfig? config = null,
            ExchangeParameters? exchangeParameters = null) : base(
                logger,
                restClient.SpotApi.SharedApi,

                restClient.SpotApi.SharedApi,
                null,

                restClient.SpotApi.SharedApi,
                restClient.SpotApi.SharedApi,
                null,

                null,
                null,
                userIdentifier,
                (config ?? new SpotUserDataTrackerConfig()),
                exchangeParameters)
        {

        }
    }
}
