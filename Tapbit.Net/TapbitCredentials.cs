using CryptoExchange.Net.Authentication;
using System;

namespace Tapbit.Net
{
    /// <summary>
    /// Tapbit API credentials
    /// </summary>
    public class TapbitCredentials : HMACCredential
    {
        /// <summary>
        /// Create new credentials
        /// </summary>
        public TapbitCredentials() { }

        /// <summary>
        /// Create new credentials providing credentials in HMAC format
        /// </summary>
        /// <param name="key">API key</param>
        /// <param name="secret">API secret</param>
        public TapbitCredentials(string key, string secret) : base(key, secret)
        {
        }

        /// <summary>
        /// Create new credentials providing HMAC credentials
        /// </summary>
        /// <param name="credential">HMAC credentials</param>
        public TapbitCredentials(HMACCredential credential) : base(credential.Key, credential.Secret)
        {
        }

        /// <summary>
        /// Specify the HMAC credentials
        /// </summary>
        /// <param name="key">API key</param>
        /// <param name="secret">API secret</param>
        public TapbitCredentials WithHMAC(string key, string secret)
        {
            if (!string.IsNullOrEmpty(Key)) throw new InvalidOperationException("Credentials already set");

            Key = key;
            Secret = secret;
            return this;
        }

        /// <inheritdoc />
        public override ApiCredentials Copy() => new TapbitCredentials(this);
    }
}
