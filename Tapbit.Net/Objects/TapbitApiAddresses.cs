namespace Tapbit.Net.Objects
{
    /// <summary>
    /// Api addresses
    /// </summary>
    public class TapbitApiAddresses
    {
        /// <summary>
        /// The address used by the TapbitRestClient for the API
        /// </summary>
        public string RestClientAddress { get; set; } = "";
        /// <summary>
        /// The address used by the TapbitSocketClient for the websocket API
        /// </summary>
        public string SocketClientAddress { get; set; } = "";

        /// <summary>
        /// The default addresses to connect to the Tapbit API
        /// </summary>
        public static TapbitApiAddresses Default = new TapbitApiAddresses
        {
            RestClientAddress = "https://openapi.tapbit.com",
            SocketClientAddress = "wss://ws-openapi.tapbit.com"
        };
    }
}
