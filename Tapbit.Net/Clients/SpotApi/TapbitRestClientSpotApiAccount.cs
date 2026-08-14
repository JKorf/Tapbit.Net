using CryptoExchange.Net.Objects;
using CryptoExchange.Net.RateLimiting.Guards;
using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Tapbit.Net.Interfaces.Clients.SpotApi;
using Tapbit.Net.Objects.Models;

namespace Tapbit.Net.Clients.SpotApi
{
    /// <inheritdoc />
    internal class TapbitRestClientSpotApiAccount : ITapbitRestClientSpotApiAccount
    {
        private static readonly RequestDefinitionCache _definitions = new RequestDefinitionCache();
        private readonly TapbitRestClientSpotApi _baseClient;

        internal TapbitRestClientSpotApiAccount(TapbitRestClientSpotApi baseClient)
        {
            _baseClient = baseClient;
        }

        #region Get Balances

        /// <inheritdoc />
        public async Task<HttpResult<TapbitBalance[]>> GetBalancesAsync(CancellationToken ct = default)
        {
            var request = _definitions.GetOrCreate(HttpMethod.Get, _baseClient.BaseAddress, "/spot-v2/api/v1/spot/account/list", TapbitExchange.RateLimiter.Tapbit, 1, true, limitGuard: new SingleLimitGuard(1, TimeSpan.FromSeconds(1), RateLimitWindowType.Sliding));
            var result = await _baseClient.SendAsync<TapbitBalance[]>(request, null, ct).ConfigureAwait(false);
            return result;
        }

        #endregion

    }
}
