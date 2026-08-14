using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using CryptoExchange.Net.Objects;
using Microsoft.Extensions.Logging;
using Tapbit.Net.Interfaces.Clients.UsdtPerpetualFuturesApi;
using Tapbit.Net.Objects.Models;

namespace Tapbit.Net.Clients.UsdtPerpetualFuturesApi
{
    /// <inheritdoc />
    internal class TapbitRestClientUsdtPerpetualFuturesApiExchangeData : ITapbitRestClientUsdtPerpetualFuturesApiExchangeData
    {
        private readonly TapbitRestClientUsdtPerpetualFuturesApi _baseClient;
        private static readonly RequestDefinitionCache _definitions = new RequestDefinitionCache();

        internal TapbitRestClientUsdtPerpetualFuturesApiExchangeData(ILogger logger, TapbitRestClientUsdtPerpetualFuturesApi baseClient)
        {
            _baseClient = baseClient;
        }

        #region Get Server Time

        /// <inheritdoc />
        public async Task<HttpResult<DateTime>> GetServerTimeAsync(CancellationToken ct = default)
        {
            var request = _definitions.GetOrCreate(HttpMethod.Get, _baseClient.BaseAddress, "XXX", TapbitExchange.RateLimiter.Tapbit, 1, false);
            var result = await _baseClient.SendAsync<TapbitModel>(request, null, ct).ConfigureAwait(false);
            if (!result.Success)
                return HttpResult.Fail<DateTime>(result);

            throw new NotImplementedException();
        }

        #endregion
    }
}
