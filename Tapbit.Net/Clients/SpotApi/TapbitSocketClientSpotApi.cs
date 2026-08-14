using System;
using System.Net.WebSockets;
using System.Threading;
using System.Threading.Tasks;
using CryptoExchange.Net.Clients;
using CryptoExchange.Net.Converters.MessageParsing.DynamicConverters;
using CryptoExchange.Net.Converters.SystemTextJson;
using CryptoExchange.Net.Interfaces;
using CryptoExchange.Net.Objects;
using CryptoExchange.Net.Objects.Errors;
using CryptoExchange.Net.Objects.Sockets;
using CryptoExchange.Net.SharedApis;
using Microsoft.Extensions.Logging;
using Tapbit.Net.Clients.MessageHandlers;
using Tapbit.Net.Interfaces.Clients.SpotApi;
using Tapbit.Net.Objects.Models;
using Tapbit.Net.Objects.Options;
using Tapbit.Net.Objects.Sockets.Subscriptions;

namespace Tapbit.Net.Clients.SpotApi
{
    /// <summary>
    /// Client providing access to the Tapbit Spot websocket Api
    /// </summary>
    internal partial class TapbitSocketClientSpotApi : SocketApiClient<TapbitEnvironment, TapbitAuthenticationProvider, TapbitCredentials>, ITapbitSocketClientSpotApi
    {
        #region fields
        protected override ErrorMapping ErrorMapping => TapbitErrors.Errors;
        #endregion

        #region constructor/destructor

        /// <summary>
        /// ctor
        /// </summary>
        internal TapbitSocketClientSpotApi(ILoggerFactory? loggerFactory, TapbitSocketOptions options) :
            base(loggerFactory, TapbitExchange.Metadata.Id, options.Environment.SocketClientAddress!, options, options.SpotOptions)
        {
        }
        #endregion

        /// <inheritdoc />
        protected override IMessageSerializer CreateSerializer() => new SystemTextJsonMessageSerializer(TapbitExchange._serializerContext);
        /// <inheritdoc />
        public override ISocketMessageHandler CreateMessageConverter(WebSocketMessageType messageType) => new TapbitSocketSpotMessageHandler();

        /// <inheritdoc />
        protected override TapbitAuthenticationProvider CreateAuthenticationProvider(TapbitCredentials credentials)
            => new TapbitAuthenticationProvider(credentials);

        /// <inheritdoc />
        public async Task<WebSocketResult<UpdateSubscription>> SubscribeToXXXUpdatesAsync(Action<DataEvent<TapbitModel>> onMessage, CancellationToken ct = default)
        {
            var internalHandler = new Action<DateTime, string?, TapbitModel>((receiveTime, originalData, data) =>
            {
                //UpdateTimeOffset(data.Timestamp);

                onMessage(
                    new DataEvent<TapbitModel>(TapbitExchange.Metadata.Id, data, receiveTime, originalData)
                        .WithUpdateType(SocketUpdateType.Update)
                        //.WithStreamId(data.Stream)
                        //.WithSymbol(data.Symbol)
                        //.WithDataTimestamp(data.EventTime, GetTimeOffset())
                    );
            });

            var subscription = new TapbitSubscription<TapbitModel>(_logger, new [] { "XXX" }, internalHandler, false);
            return await SubscribeAsync(subscription, ct).ConfigureAwait(false);
        }

        /// <inheritdoc />
        public ITapbitSocketClientSpotApiShared SharedClient => this;

        /// <inheritdoc />
        public override string FormatSymbol(string baseAsset, string quoteAsset, TradingMode tradingMode, DateTime? deliverDate = null)
            => TapbitExchange.FormatSymbol(baseAsset, quoteAsset, tradingMode, deliverDate);
    }
}
