using CryptoExchange.Net.Authentication;
using CryptoExchange.Net.Clients;
using CryptoExchange.Net.Converters.SystemTextJson;
using CryptoExchange.Net.Objects;
using CryptoExchange.Net.Testing;
using NUnit.Framework;
using System.Collections.Generic;
using System.Net.Http;
using Tapbit.Net.Clients;

namespace Tapbit.Net.UnitTests
{
    [TestFixture()]
    public class TapbitRestClientTests
    {
        [Test]
        public void CheckSignatureExample1()
        {
            var authProvider = new TapbitAuthenticationProvider(new TapbitCredentials("XXX", "XXX"));
            var client = (RestApiClient)new TapbitRestClient().SpotApi;

            CryptoExchange.Net.Testing.TestHelpers.CheckSignature(
                client,
                authProvider,
                HttpMethod.Post,
                "/api/v3/order",
                (uriParams, bodyParams, headers) =>
                {
                    return headers["ACCESS-SIGN"].ToString();
                },
                "929c869e11980c5cb9c38f9583ed61c0f4193684c58ddb49d527f4a6b4414167",
                new Parameters(TapbitExchange._parameterSerializationSettings)
                {
                    { "symbol", "LTCBTC" },
                },
                DateTimeConverter.ParseFromDouble(1499827319559),
                false);
        }

        [Test]
        public void CheckInterfaces()
        {
            CryptoExchange.Net.Testing.TestHelpers.CheckForMissingRestInterfaces<TapbitRestClient>();
        }

        [Test]
        public void TestSpotRestSharedApiDiscoveryMatchesAggregate()
        {
            var (missingOptions, missingInterfaces) = TestHelpers.ValidateSharedApi(new TapbitRestClient().SpotApi.SharedApi);

            Assert.That(missingOptions, Is.Empty);
            Assert.That(missingInterfaces, Is.Empty);
        }

    }
}
