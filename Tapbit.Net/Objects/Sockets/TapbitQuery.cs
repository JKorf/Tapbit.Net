using CryptoExchange.Net.Objects;
using CryptoExchange.Net.Sockets;
using CryptoExchange.Net.Sockets.Default;
using CryptoExchange.Net.Sockets.Default.Routing;
using System;
using Tapbit.Net.Objects.Models;

namespace Tapbit.Net.Objects.Sockets
{
    internal class TapbitQuery<T> : Query<T>
    {
        public TapbitQuery(TapbitModel request, bool authenticated, int weight = 1) : base(request, authenticated, weight)
        {
            MessageRouter = MessageRouter.CreateForQuery<T>("", HandleMessage);
        }

        public CallResult<T> HandleMessage(SocketConnection connection, DateTime receiveTime, string? originalData, T message)
        {
            return CallResult.Ok(message, originalData);
        }
    }
}
