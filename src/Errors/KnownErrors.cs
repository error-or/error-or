namespace ErrorOr;

internal static class KnownErrors
{
    public static Error NoFirstError { get; } = Error.Unexpected(
        code: "ErrorOr.NoFirstError",
        description: "First error cannot be retrieved from a successful ErrorOr.");

    public static Error NoErrors { get; } = Error.Unexpected(
        code: "ErrorOr.NoErrors",
        description: "Error list cannot be retrieved from a successful ErrorOr.");

    public static Error EmptyInitialErrors { get; } = Error.Unexpected(
        code: "ErrorOr.EmptyInitialErrors",
        description: "Error list cannot be null or empty when initializing ErrorOr.");

    public static ReadOnlyCollection<Error> CachedNoErrorsList { get; } = new ReadOnlyCollection<Error>([NoErrors]);

    public static ReadOnlyCollection<Error> CachedInvalidInitialErrorsList { get; } = new ReadOnlyCollection<Error>([EmptyInitialErrors]);

    public static ReadOnlyCollection<Error> CachedEmptyErrorsList { get; } = new ReadOnlyCollection<Error>([]);
}
