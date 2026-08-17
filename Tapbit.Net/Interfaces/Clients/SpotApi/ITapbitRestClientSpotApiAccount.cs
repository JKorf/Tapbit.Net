using CryptoExchange.Net.Objects;
using System.Threading;
using System.Threading.Tasks;
using Tapbit.Net.Objects.Models;

namespace Tapbit.Net.Interfaces.Clients.SpotApi
{
    /// <summary>
    /// Tapbit Spot account endpoints. Account endpoints include balance info, withdraw/deposit info and requesting and account settings
    /// </summary>
    public interface ITapbitRestClientSpotApiAccount
    {
        /// <summary>
        /// Get account balances
        /// <para>
        /// Docs:<br />
        /// <a href="https://www.tapbit.com/openapi-docs/spot_v2/private/account_info/" /><br />
        /// Endpoint:<br />
        /// GET /spot-v2/api/v1/spot/account/list<br />
        /// </para>
        /// </summary>
        /// <param name="ct">Cancellation token</param>
        Task<HttpResult<TapbitBalance[]>> GetBalancesAsync(CancellationToken ct = default);

        /// <summary>
        /// Get account balance for an asset
        /// <para>
        /// Docs:<br />
        /// <a href="https://www.tapbit.com/openapi-docs/spot_v2/private/specified_account_info/" /><br />
        /// Endpoint:<br />
        /// GET /spot-v2/api/v1/spot/account/one<br />
        /// </para>
        /// </summary>
        /// <param name="asset">["<c>asset</c>"] The asset, for example `ETH`</param>
        /// <param name="ct">Cancellation token</param>
        Task<HttpResult<TapbitBalance>> GetBalanceAsync(string asset, CancellationToken ct = default);

    }
}
