using CryptoExchange.Net.Converters.SystemTextJson.MessageHandlers;
using CryptoExchange.Net.Objects;
using CryptoExchange.Net.Objects.Errors;
using System.IO;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Threading.Tasks;
using Tapbit.Net.Objects.Models;

namespace Tapbit.Net.Clients.MessageHandlers
{
    internal class TapbitRestMessageHandler : JsonRestMessageHandler
    {
        private readonly ErrorMapping _errorMapping;

        public override JsonSerializerOptions Options { get; } = TapbitExchange._serializerContext;

        public TapbitRestMessageHandler(ErrorMapping errorMapping)
        {
            _errorMapping = errorMapping;
        }

        public override async ValueTask<Error> ParseErrorResponse(
            int httpStatusCode,
            HttpResponseHeaders responseHeaders,
            Stream responseStream)
        {
            var (error, document) = await GetJsonDocument(responseStream).ConfigureAwait(false);
            if (error != null)
                return error;

            int? code = document!.RootElement.TryGetProperty("code", out var codeProp) ? codeProp.GetInt32() : null;
            string? msg = document.RootElement.TryGetProperty("msg", out var msgProp) ? msgProp.GetString() : null;
            if (msg == null)
                return new ServerError(ErrorInfo.Unknown);

            if (code == null)
                return new ServerError(new ErrorInfo(ErrorType.Unknown, false, msg));

            var errorInfo = _errorMapping.GetErrorInfo(code.ToString()!, msg);
            return new ServerError(code.Value.ToString(), errorInfo);
        }

        public override Error? CheckDeserializedResponse<T>(HttpResponseHeaders responseHeaders, T result)
        {
            if (result is not TapbitResponse restResult)
                return base.CheckDeserializedResponse(responseHeaders, result);

            if (restResult.Code == 0 || restResult.Code == 200)
                return null;

            var code = restResult.Code.ToString();
            var errorInfo = _errorMapping.GetErrorInfo(code, restResult.Message);
            return new ServerError(restResult.Code, errorInfo);
        }
    }
}
