using CryptoExchange.Net;
using CryptoExchange.Net.Objects;
using CryptoExchange.Net.Objects.Errors;
using CryptoExchange.Net.SharedApis;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Tapbit.Net.Enums;
using Tapbit.Net.Interfaces.Clients.SpotApi;
using Tapbit.Net.Objects.Models;

namespace Tapbit.Net.Clients.SpotApi
{
    internal partial class TapbitRestClientSpotSharedApi
    {
        #region Get Spot Symbols

        async Task<ICallResult<SharedSpotSymbol[]>> IGetSpotSymbols.GetSpotSymbolsAsync(GetSymbolsRequest request, CancellationToken ct)
            => await GetSpotSymbolsAsync(request, ct).ConfigureAwait(false);

        public SharedSymbolCatalog? SpotSymbolCatalog => ExchangeSymbolCache.GetSymbolCatalog(_exchangeName, _topicId, _api.EnvironmentName, null);
        public GetSpotSymbolsOptions GetSpotSymbolsOptions { get; } = new GetSpotSymbolsOptions(_exchangeName, false);

        public async Task<HttpResult<SharedSpotSymbol[]>> GetSpotSymbolsAsync(GetSymbolsRequest request, CancellationToken ct)
        {
            var validationError = GetSpotSymbolsOptions.ValidateRequest(request, this);
            if (validationError != null)
                return HttpResult.Fail<SharedSpotSymbol[]>(Exchange, validationError);

            var result = await _api.ExchangeData.GetSymbolsAsync(ct: ct).ConfigureAwait(false);
            if (!result.Success)
                return HttpResult.Fail<SharedSpotSymbol[]>(result);

            var resultData =
                 result.Data.Select(x => ParseSymbol(x))
                .ToArray();

            ExchangeSymbolCache.UpdateSymbolInfo(_topicId, _api.EnvironmentName, null, resultData);
            return HttpResult.Ok(result, SharedUtils.ApplySymbolFilter(resultData, request));
        }

        #endregion

        private SharedSpotSymbol ParseSymbol(TapbitSymbol s)
        {
            var result = new SharedSpotSymbol(s.BaseAsset, s.QuoteAsset, s.Symbol, true)
            {
                MinTradeQuantity = s.MinQuantity,
                MinNotionalValue = s.MinNotional,
                QuantityDecimals = s.QuantityPrecision,
                PriceDecimals = s.PricePrecision,
                QuoteAssetType = SharedAssetType.Crypto,
                QuoteAssetSubType = SharedAssetSubType.StableCoin,
                MakerFeePercentage = s.MakerFeeRate * 100,
                TakerFeePercentage = s.TakerFeeRate * 100,
                LowerPriceLimitPercentage = -s.PriceFluctuation * 100,
                UpperPriceLimitPercentage = s.PriceFluctuation * 100
            };

            result.BaseAssetType = SharedAssetType.Crypto;
            if (LibraryHelpers.IsStableCoin(s.BaseAsset))
                result.BaseAssetSubType = SharedAssetSubType.StableCoin;

            return result;
        }

        public async Task<ExchangeCallResult<SharedSymbol[]>> GetSpotSymbolsForBaseAssetAsync(string baseAsset)
        {
            if (!ExchangeSymbolCache.HasCached(_topicId, _api.EnvironmentName, null))
            {
                var symbols = await GetSpotSymbolsAsync(new GetSymbolsRequest(), default).ConfigureAwait(false);
                if (!symbols.Success)
                    return ExchangeCallResult<SharedSymbol[]>.Fail(Exchange, symbols.Error!);
            }

            return ExchangeCallResult<SharedSymbol[]>.Ok(Exchange, ExchangeSymbolCache.GetSymbolsForBaseAsset(_topicId, _api.EnvironmentName, null, baseAsset));
        }

        public async Task<ExchangeCallResult<bool>> SupportsSpotSymbolAsync(SharedSymbol symbol)
        {
            if (symbol.TradingMode != TradingMode.Spot)
                throw new ArgumentException(nameof(symbol), "Only Spot symbols allowed");

            if (!ExchangeSymbolCache.HasCached(_topicId, _api.EnvironmentName, null))
            {
                var symbols = await GetSpotSymbolsAsync(new GetSymbolsRequest(), default).ConfigureAwait(false);
                if (!symbols.Success)
                    return ExchangeCallResult<bool>.Fail(Exchange, symbols.Error!);
            }

            return ExchangeCallResult<bool>.Ok(Exchange, ExchangeSymbolCache.SupportsSymbol(_topicId, _api.EnvironmentName, null, symbol));
        }

        public async Task<ExchangeCallResult<bool>> SupportsSpotSymbolAsync(string symbolName)
        {
            if (!ExchangeSymbolCache.HasCached(_topicId, _api.EnvironmentName, null))
            {
                var symbols = await GetSpotSymbolsAsync(new GetSymbolsRequest(), default).ConfigureAwait(false);
                if (!symbols.Success)
                    return ExchangeCallResult<bool>.Fail(Exchange, symbols.Error!);
            }

            return ExchangeCallResult<bool>.Ok(Exchange, ExchangeSymbolCache.SupportsSymbol(_topicId, _api.EnvironmentName, null, symbolName));
        }
    }
}
