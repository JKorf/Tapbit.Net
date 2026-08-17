using CryptoExchange.Net.Objects.Errors;

namespace Tapbit.Net
{
    internal static class TapbitErrors
    {
        public static ErrorMapping Errors { get; } = new ErrorMapping(
            [
            new ErrorInfo(ErrorType.SystemError, true, "System busy", "10000"),

            new ErrorInfo(ErrorType.UnknownSymbol, false, "Invalid symbol", "51817"),
            new ErrorInfo(ErrorType.UnknownOrder, false, "Unknown order", "51801", "11014"),

            new ErrorInfo(ErrorType.RateLimitOrder, false, "Too many orders", "51856"),

            new ErrorInfo(ErrorType.InvalidQuantity, false, "Invalid quantity precision", "51806", "11005"),
            new ErrorInfo(ErrorType.InvalidQuantity, false, "Quantity min amount not reached", "51858"),

            new ErrorInfo(ErrorType.InvalidPrice, false, "Invalid limit price", "51824"),
            new ErrorInfo(ErrorType.InvalidPrice, false, "Invalid price precision", "51805", "11004"),
            new ErrorInfo(ErrorType.InvalidPrice, false, "Limit price offset from current price too large", "51804", "51803"),

            new ErrorInfo(ErrorType.InsufficientBalance, false, "Insufficient balance", "51809"),

            new ErrorInfo(ErrorType.MissingParameter, false, "Parameter value empty", "11000"),
            new ErrorInfo(ErrorType.InvalidParameter, false, "Invalid parameter value", "11001", "11002"),
            ]
            );
    }
}
