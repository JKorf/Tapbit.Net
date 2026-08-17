using CryptoExchange.Net.Objects;
using Tapbit.Net.Objects;

namespace Tapbit.Net
{
    /// <summary>
    /// Tapbit environments
    /// </summary>
    public class TapbitEnvironment : TradeEnvironment
    {
        /// <summary>
        /// Rest API address
        /// </summary>
        public string RestClientAddress { get; }

        internal TapbitEnvironment(
            string name,
            string restAddress) :
            base(name)
        {
            RestClientAddress = restAddress;
        }

        /// <summary>
        /// ctor for DI, use <see cref="CreateCustom"/> for creating a custom environment
        /// </summary>
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
        public TapbitEnvironment() : base(TradeEnvironmentNames.Live)
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
        { }

        /// <summary>
        /// Get the Tapbit environment by name
        /// </summary>
        public static TapbitEnvironment? GetEnvironmentByName(string? name)
         => name switch
         {
             TradeEnvironmentNames.Live => Live,
             "" => Live,
             null => Live,
             _ => default
         };

        /// <summary>
        /// Available environment names
        /// </summary>
        /// <returns></returns>
        public static string[] All => [Live.Name];

        /// <summary>
        /// Live environment
        /// </summary>
        public static TapbitEnvironment Live { get; }
            = new TapbitEnvironment(TradeEnvironmentNames.Live,
                                     TapbitApiAddresses.Default.RestClientAddress);

        /// <summary>
        /// Create a custom environment
        /// </summary>
        /// <param name="name"></param>
        /// <param name="spotRestAddress"></param>
        /// <returns></returns>
        public static TapbitEnvironment CreateCustom(
                        string name,
                        string spotRestAddress)
            => new TapbitEnvironment(name, spotRestAddress);
    }
}
