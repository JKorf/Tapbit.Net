using CryptoExchange.Net.Objects;
using Microsoft.Extensions.Logging;
using Tapbit.Net.Interfaces.Clients.SpotApi;

namespace Tapbit.Net.Clients.SpotApi
{
    /// <inheritdoc />
    internal class TapbitRestClientSpotApiTrading : ITapbitRestClientSpotApiTrading
    {
        private static readonly RequestDefinitionCache _definitions = new RequestDefinitionCache();
        private readonly TapbitRestClientSpotApi _baseClient;
        private readonly ILogger _logger;

        internal TapbitRestClientSpotApiTrading(ILogger logger, TapbitRestClientSpotApi baseClient)
        {
            _baseClient = baseClient;
            _logger = logger;
        }
    }
}
