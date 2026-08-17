using CryptoExchange.Net.Authentication;
using CryptoExchange.Net.SharedApis;
using CryptoExchange.Net.Trackers.Klines;
using CryptoExchange.Net.Trackers.Trades;
using CryptoExchange.Net.Trackers.UserData.Interfaces;
using CryptoExchange.Net.Trackers.UserData.Objects;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using System;
using System.Linq;
using Tapbit.Net.Clients;
using Tapbit.Net.Interfaces;
using Tapbit.Net.Interfaces.Clients;

namespace Tapbit.Net
{
    /// <inheritdoc />
    public class TapbitTrackerFactory : ITapbitTrackerFactory
    {
        private readonly IServiceProvider? _serviceProvider;

        /// <summary>
        /// ctor
        /// </summary>
        public TapbitTrackerFactory()
        {
        }

        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="serviceProvider">Service provider for resolving logging and clients</param>
        public TapbitTrackerFactory(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        /// <inheritdoc />
        public bool CanCreateKlineTracker(SharedSymbol symbol, SharedKlineInterval interval) => false;

        /// <inheritdoc />
        public bool CanCreateTradeTracker(SharedSymbol symbol) => false;

        /// <inheritdoc />
        public IKlineTracker CreateKlineTracker(SharedSymbol symbol, SharedKlineInterval interval, int? limit = null, TimeSpan? period = null, ExchangeParameters? exchangeParameters = null)
        {
            throw new InvalidOperationException("Kline tracker not supported for Tapbit");
        }
        /// <inheritdoc />
        public ITradeTracker CreateTradeTracker(SharedSymbol symbol, int? limit = null, TimeSpan? period = null, ExchangeParameters? exchangeParameters = null)
        {
            throw new InvalidOperationException("Trade tracker not supported for Tapbit");
        }

        /// <inheritdoc />
        public IUserSpotDataTracker CreateUserSpotDataTracker(SpotUserDataTrackerConfig? config = null, ExchangeParameters? exchangeParameters = null)
        {
            if (config?.TrackTrades == true)
                throw new InvalidOperationException("User trade tracking not supported for Tapbit, set `TrackTrades` to false in the config to use the tracker");

            if (config?.TrackedSymbols.Any() != true)
                throw new InvalidOperationException("User trade tracking requires the symbols to be specified in `TrackedSymbols` in the configuration");

            var restClient = _serviceProvider?.GetRequiredService<ITapbitRestClient>() ?? new TapbitRestClient();
            return new TapbitUserSpotDataTracker(
                _serviceProvider?.GetRequiredService<ILogger<TapbitUserSpotDataTracker>>() ?? new NullLogger<TapbitUserSpotDataTracker>(),
                restClient,
                null,
                config,
                exchangeParameters
                );
        }

        /// <inheritdoc />
        public IUserSpotDataTracker CreateUserSpotDataTracker(string userIdentifier, TapbitCredentials credentials, SpotUserDataTrackerConfig? config = null, TapbitEnvironment? environment = null, ExchangeParameters? exchangeParameters = null)
        {
            if (config?.TrackTrades == true)
                throw new InvalidOperationException("User trade tracking not supported for Tapbit, set `TrackTrades` to false in the config to use the tracker");

            if (config?.TrackedSymbols.Any() != true)
                throw new InvalidOperationException("User trade tracking requires the symbols to be specified in `TrackedSymbols` in the configuration");

            var clientProvider = _serviceProvider?.GetRequiredService<ITapbitUserClientProvider>() ?? new TapbitUserClientProvider();
            var restClient = clientProvider.GetRestClient(userIdentifier, credentials, environment);
            return new TapbitUserSpotDataTracker(
                _serviceProvider?.GetRequiredService<ILogger<TapbitUserSpotDataTracker>>() ?? new NullLogger<TapbitUserSpotDataTracker>(),
                restClient,
                userIdentifier,
                config,
                exchangeParameters
                );
        }
    }
}
