using CryptoExchange.Net.Converters.MessageParsing.DynamicConverters;
using System.Text.Json;
using CryptoExchange.Net.Converters.SystemTextJson.MessageHandlers;

namespace Tapbit.Net.Clients.MessageHandlers
{
    internal class TapbitSocketSpotMessageHandler : JsonSocketMessageHandler
    {
        public override JsonSerializerOptions Options { get; } = TapbitExchange._serializerContext;

        public TapbitSocketSpotMessageHandler()
        {
        }

        protected override MessageTypeDefinition[] TypeEvaluators { get; } = [

#warning TODO

            //new MessageTypeDefinition {
            //    Fields = [
            //        new PropertyFieldReference("stream"),
            //    ],
            //    TypeIdentifierCallback = x => x.FieldValue("stream")!,
            //},

            //new MessageTypeDefinition {
            //    ForceIfFound = true,
            //    Fields = [
            //        new PropertyFieldReference("id"),
            //    ],
            //    TypeIdentifierCallback = x => x.FieldValue("id")!,
            //}
        ];
    }
}
