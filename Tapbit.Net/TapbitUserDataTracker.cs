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
            ITapbitSocketClient socketClient,
            string? userIdentifier,
            SpotUserDataTrackerConfig? config = null,
            ExchangeParameters? exchangeParameters = null) : base(
                logger,
#warning TODO
                null,
                null,
                null,
                null,
                null,
                null,
                userIdentifier,
                config ?? new SpotUserDataTrackerConfig(),
                exchangeParameters)
        {

        }
    }

    /// <inheritdoc />
    public class TapbitUserFuturesDataTracker : UserFuturesDataTracker
    {
        /// <inheritdoc />
        protected override bool WebsocketPositionUpdatesAreFullSnapshots => false;

        /// <summary>
        /// ctor
        /// </summary>
        public TapbitUserFuturesDataTracker(
            ILogger<TapbitUserFuturesDataTracker> logger,
            ITapbitRestClient restClient,
            ITapbitSocketClient socketClient,
            string? userIdentifier,
            FuturesUserDataTrackerConfig? config = null,
            ExchangeParameters? exchangeParameters = null) :
            base(logger,
#warning TODO
                null,
                null,
                null,
                null,
                null,
                null,
                null,
                userIdentifier,
                config ?? new FuturesUserDataTrackerConfig(),
                exchangeParameters: exchangeParameters)
        {

        }
    }
}
