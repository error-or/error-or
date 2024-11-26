namespace ErrorOr;

public static partial class ErrorOrExtensions
{
    /// <summary>
    /// Creates an <see cref="ErrorOr{TValue}"/> instance with the given <paramref name="value"/>.
    /// </summary>
    public static ErrorOr<TValue> ToErrorOr<TValue>(this TValue value)
    {
        return value;
    }

    /// <summary>
    /// Creates an <see cref="ErrorOr{TValue}"/> instance with the given <paramref name="error"/>.
    /// </summary>
    public static ErrorOr<TValue> ToErrorOr<TValue>(this Error error)
    {
        return error;
    }

    /// <summary>
    /// Creates an <see cref="ErrorOr{TValue}"/> instance with the given <paramref name="errors"/>.
    /// </summary>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="errors"/> is null.</exception>
    /// <exception cref="ArgumentException">Thrown when <paramref name="errors" /> is an empty list.</exception>
    public static ErrorOr<TValue> ToErrorOr<TValue>(this List<Error> errors)
    {
        return errors;
    }

    /// <summary>
    /// Creates an <see cref="ErrorOr{TValue}"/> instance with the given <paramref name="errors"/>.
    /// </summary>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="errors"/> is null.</exception>
    /// <exception cref="ArgumentException">Thrown when <paramref name="errors" /> is an empty array.</exception>
    public static ErrorOr<TValue> ToErrorOr<TValue>(this Error[] errors)
    {
        return errors;
    }

    /// <summary>
    /// Creates an <see cref="ErrorOr{TValue}"/> instance with the given <paramref name="value"/> and additional metadata.
    /// </summary>
    /// <param name="value">The value to wrap.</param>
    /// <param name="metadata">Additional metadata to include with the errors.</param>
    /// <returns>An instance of <see cref="ErrorOr{TValue}"/> containing the provided value and metadata.</returns>
    public static ErrorOr<TValue> ToErrorOrWithMetadata<TValue>(this TValue value, Dictionary<string, object> metadata)
    {
        return new ErrorOr<TValue>(value) with { Metadata = metadata };
    }

    /// <summary>
    /// Creates an <see cref="ErrorOr{TValue}"/> instance with the given <paramref name="error"/> and additional metadata.
    /// </summary>
    /// <param name="error">The error to wrap.</param>
    /// <param name="metadata">Additional metadata to include with the errors.</param>
    /// <returns>An instance of <see cref="ErrorOr{TValue}"/> containing the provided error and metadata.</returns>
    public static ErrorOr<TValue> ToErrorOrWithMetadata<TValue>(this Error error, Dictionary<string, object> metadata)
    {
        return new ErrorOr<TValue>(error) with { Metadata = metadata };
    }

    /// <summary>
    /// Creates an <see cref="ErrorOr{TValue}"/> instance with the given <paramref name="errors"/> and additional metadata.
    /// </summary>
    /// <param name="errors">The list of errors to wrap.</param>
    /// <param name="metadata">Additional metadata to include with the errors.</param>
    /// <returns>An instance of <see cref="ErrorOr{TValue}"/> containing the provided errors and metadata.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="errors"/> is null.</exception>
    /// <exception cref="ArgumentException">Thrown when <paramref name="errors" /> is an empty list.</exception>
    public static ErrorOr<TValue> ToErrorOrWithMetadata<TValue>(this List<Error> errors, Dictionary<string, object> metadata)
    {
        var enhancedErrors = errors.Select(e => e with { Metadata = metadata }).ToList();
        return new ErrorOr<TValue>(enhancedErrors);
    }

    /// <summary>
    /// Creates an <see cref="ErrorOr{TValue}"/> instance with the given <paramref name="errors"/> and additional metadata.
    /// </summary>
    /// <param name="errors">The array of errors to wrap.</param>
    /// <param name="metadata">Additional metadata to include with the errors.</param>
    /// <returns>An instance of <see cref="ErrorOr{TValue}"/> containing the provided errors and metadata.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="errors"/> is null.</exception>
    /// <exception cref="ArgumentException">Thrown when <paramref name="errors" /> is an empty array.</exception>
    public static ErrorOr<TValue> ToErrorOrWithMetadata<TValue>(this Error[] errors, Dictionary<string, object> metadata)
    {
        var enhancedErrors = errors.Select(e => e with { Metadata = metadata }).ToList();
        return new ErrorOr<TValue>(enhancedErrors);
    }
}
