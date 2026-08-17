using CryptoExchange.Net;
using CryptoExchange.Net.Authentication;
using CryptoExchange.Net.Clients;
using CryptoExchange.Net.Converters.SystemTextJson;
using CryptoExchange.Net.Interfaces;
using CryptoExchange.Net.Objects;
using CryptoExchange.Net.Sockets;
using CryptoExchange.Net.Sockets.Default;
using System.Collections.Generic;
using System.Globalization;
using System.Text.Json;

namespace Tapbit.Net
{
    internal class TapbitAuthenticationProvider : AuthenticationProvider<TapbitCredentials, TapbitCredentials>
    {
        private readonly static IMessageSerializer _serializer = new SystemTextJsonMessageSerializer(TapbitExchange._serializerContext);

        public TapbitAuthenticationProvider(TapbitCredentials credentials) : base(credentials, credentials)
        {
        }


        public override void ProcessRequest(RestApiClient apiClient, RestRequestConfiguration requestConfig)
        {
            if (!requestConfig.RequestDefinition.Authenticated)
                return;

            var timestamp = (GetMillisecondTimestampLong(apiClient) / 1000m).ToString(CultureInfo.InvariantCulture);
            requestConfig.Headers ??= new Dictionary<string, string>();
            requestConfig.Headers["ACCESS-KEY"] = Credential.Key;
            requestConfig.Headers["ACCESS-TIMESTAMP"] = timestamp;

            var body = requestConfig.BodyParameters == null || requestConfig.BodyParameters.Empty ? "" : GetSerializedBody(_serializer, requestConfig.BodyParameters);
            var path = requestConfig.RequestDefinition.Path.StartsWith("/spot-v2/") ? requestConfig.RequestDefinition.Path.Substring(8) : requestConfig.RequestDefinition.Path;
            var queryStr = requestConfig.QueryParameters?.CreateParamString(true, requestConfig.ArraySerialization);
            path += string.IsNullOrEmpty(queryStr) ? "" : $"?{queryStr}";
            var signStr = timestamp + requestConfig.RequestDefinition.Method + path + body;
            var signature = SignHMACSHA256(signStr, SignOutputType.Hex);
            requestConfig.Headers["ACCESS-SIGN"] = signature.ToLowerInvariant();
        }
    }
}
