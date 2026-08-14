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
        public bool CanCreateKlineTracker(SharedSymbol symbol, SharedKlineInterval interval)
        {
            var client = _serviceProvider?.GetRequiredService<ITapbitSocketClient>() ?? new TapbitSocketClient();
#warning TODO
            SubscribeKlineOptions klineOptions = new SubscribeKlineOptions(TapbitExchange.Metadata.Id, true);
            return klineOptions.IsSupported(interval); 
        }

        /// <inheritdoc />
        public bool CanCreateTradeTracker(SharedSymbol symbol) => true;

        /// <inheritdoc />
        public IKlineTracker CreateKlineTracker(SharedSymbol symbol, SharedKlineInterval interval, int? limit = null, TimeSpan? period = null, ExchangeParameters? exchangeParameters = null)
        {
            var restClient = _serviceProvider?.GetRequiredService<ITapbitRestClient>() ?? new TapbitRestClient();
            var socketClient = _serviceProvider?.GetRequiredService<ITapbitSocketClient>() ?? new TapbitSocketClient();

#warning todo
            throw new NotImplementedException();
            //IKlineRestClient sharedRestClient;
            //IKlineSocketClient sharedSocketClient;
            //if (symbol.TradingMode == TradingMode.Spot)
            //{
            //    sharedRestClient = restClient.SpotApi.SharedClient;
            //    sharedSocketClient = socketClient.SpotApi.SharedClient;
            //}
            //else
            //{
            //    sharedRestClient = restClient.FuturesApi.SharedClient;
            //    sharedSocketClient = socketClient.FuturesApi.SharedClient;
            //}

            //return new KlineTracker(
            //    _serviceProvider?.GetRequiredService<ILoggerFactory>().CreateLogger(restClient.Exchange),
            //    sharedRestClient,
            //    sharedSocketClient,
            //    symbol,
            //    interval,
            //    limit,
            //    period,
            //    exchangeParameters
            //    );
        }
        /// <inheritdoc />
        public ITradeTracker CreateTradeTracker(SharedSymbol symbol, int? limit = null, TimeSpan? period = null, ExchangeParameters? exchangeParameters = null)
        {
            var restClient = _serviceProvider?.GetRequiredService<ITapbitRestClient>() ?? new TapbitRestClient();
            var socketClient = _serviceProvider?.GetRequiredService<ITapbitSocketClient>() ?? new TapbitSocketClient();

#warning todo
            throw new NotImplementedException();

            //IRecentTradeRestClient? sharedRestClient;
            //ITradeSocketClient sharedSocketClient;
            //if (symbol.TradingMode == TradingMode.Spot)
            //{
            //    sharedRestClient = restClient.SpotApi.SharedClient;
            //    sharedSocketClient = socketClient.SpotApi.SharedClient;
            //}
            //else
            //{
            //    sharedRestClient = restClient.FuturesApi.SharedClient;
            //    sharedSocketClient = socketClient.FuturesApi.SharedClient;
            //}

            //return new TradeTracker(
            //    _serviceProvider?.GetRequiredService<ILoggerFactory>().CreateLogger(restClient.Exchange),
            //    sharedRestClient,
            //    null,
            //    sharedSocketClient,
            //    symbol,
            //    limit,
            //    period,
            //#warning check
            //    TradeQuantityType.BaseAsset,
            //    exchangeParameters
            //    );
        }

        /// <inheritdoc />
        public IUserSpotDataTracker CreateUserSpotDataTracker(SpotUserDataTrackerConfig? config = null, ExchangeParameters? exchangeParameters = null)
        {
            var restClient = _serviceProvider?.GetRequiredService<ITapbitRestClient>() ?? new TapbitRestClient();
            var socketClient = _serviceProvider?.GetRequiredService<ITapbitSocketClient>() ?? new TapbitSocketClient();
            return new TapbitUserSpotDataTracker(
                _serviceProvider?.GetRequiredService<ILogger<TapbitUserSpotDataTracker>>() ?? new NullLogger<TapbitUserSpotDataTracker>(),
                restClient,
                socketClient,
                null,
                config,
                exchangeParameters
                );
        }

        /// <inheritdoc />
        public IUserSpotDataTracker CreateUserSpotDataTracker(string userIdentifier, TapbitCredentials credentials, SpotUserDataTrackerConfig? config = null, TapbitEnvironment? environment = null, ExchangeParameters? exchangeParameters = null)
        {
            var clientProvider = _serviceProvider?.GetRequiredService<ITapbitUserClientProvider>() ?? new TapbitUserClientProvider();
            var restClient = clientProvider.GetRestClient(userIdentifier, credentials, environment);
            var socketClient = clientProvider.GetSocketClient(userIdentifier, credentials, environment);
            return new TapbitUserSpotDataTracker(
                _serviceProvider?.GetRequiredService<ILogger<TapbitUserSpotDataTracker>>() ?? new NullLogger<TapbitUserSpotDataTracker>(),
                restClient,
                socketClient,
                userIdentifier,
                config,
                exchangeParameters
                );
        }

        /// <inheritdoc />
        public IUserFuturesDataTracker CreateUserFuturesDataTracker(FuturesUserDataTrackerConfig? config = null, ExchangeParameters? exchangeParameters = null)
        {
            var restClient = _serviceProvider?.GetRequiredService<ITapbitRestClient>() ?? new TapbitRestClient();
            var socketClient = _serviceProvider?.GetRequiredService<ITapbitSocketClient>() ?? new TapbitSocketClient();
            return new TapbitUserFuturesDataTracker(
                _serviceProvider?.GetRequiredService<ILogger<TapbitUserFuturesDataTracker>>() ?? new NullLogger<TapbitUserFuturesDataTracker>(),
                restClient,
                socketClient,
                null,
                config,
                exchangeParameters
                );
        }

        /// <inheritdoc />
        public IUserFuturesDataTracker CreateUserFuturesDataTracker(string userIdentifier, TapbitCredentials credentials, FuturesUserDataTrackerConfig? config = null, TapbitEnvironment? environment = null, ExchangeParameters? exchangeParameters = null)
        {
            var clientProvider = _serviceProvider?.GetRequiredService<ITapbitUserClientProvider>() ?? new TapbitUserClientProvider();
            var restClient = clientProvider.GetRestClient(userIdentifier, credentials, environment);
            var socketClient = clientProvider.GetSocketClient(userIdentifier, credentials, environment);
            return new TapbitUserFuturesDataTracker(
                _serviceProvider?.GetRequiredService<ILogger<TapbitUserFuturesDataTracker>>() ?? new NullLogger<TapbitUserFuturesDataTracker>(),
                restClient,
                socketClient,
                userIdentifier,
                config,
                exchangeParameters
                );
        }
    }
}
