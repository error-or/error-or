namespace ErrorOr;

/// <summary>
/// Provides factory methods for creating instances of <see cref="ErrorOr{TValue}"/>.
/// </summary>
public static class ErrorOrFactory
{
    /// <summary>
    /// Creates a new instance of <see cref="ErrorOr{TValue}"/> with a value.
    /// </summary>
    /// <typeparam name="TValue">The type of the value.</typeparam>
    /// <param name="value">The value to wrap.</param>
    /// <returns>An instance of <see cref="ErrorOr{TValue}"/> containing the provided value.</returns>
    public static ErrorOr<TValue> From<TValue>(TValue value)
    {
        return value;
    }

    /// <summary>
    /// Creates a new awaitable instance of <see cref="ErrorOr{TValue}"/> from value.
    /// </summary>
    /// <typeparam name="TValue">The type of the value.</typeparam>
    /// <param name="value">The value to wrap.</param>
    /// <returns>An awaitable instance of <see cref="ErrorOr{TValue}"/> containing provided value.</returns>
    public static Task<ErrorOr<TValue>> FromAsync<TValue>(TValue value)
    {
        return Task.FromResult(From(value));
    }

    /// <summary>
    /// Creates a new instance of <see cref="ErrorOr{TValue}"/> from single error.
    /// </summary>
    /// <typeparam name="TValue">The type of the value.</typeparam>
    /// <param name="error">Single error instance to wrap.</param>
    /// <returns>An instance of <see cref="ErrorOr{TValue}"/> containing the provided error.</returns>
    public static ErrorOr<TValue> From<TValue>(Error error)
    {
        return error;
    }

    /// <summary>
    /// Creates an <see cref="ErrorOr{TValue}"/> from a list of errors.
    /// </summary>
    /// <typeparam name="TValue">The type of the value.</typeparam>
    /// <param name="errors">List of errors.</param>
    /// <returns>An instance of <see cref="ErrorOr{TValue}"/> containing provided list of errors.</returns>
    public static ErrorOr<TValue> From<TValue>(List<Error> errors)
    {
        return errors;
    }

    /// <summary>
    /// Creates an <see cref="ErrorOr{TValue}"/> from an enumeration of errors.
    /// </summary>
    /// <typeparam name="TValue">The type of the value.</typeparam>
    /// <param name="errors">Enumeration of errors.</param>
    /// <returns>An instance of <see cref="ErrorOr{TValue}"/> containing provided enumeration of errors.</returns>
    public static ErrorOr<TValue> From<TValue>(IEnumerable<Error> errors)
    {
        return errors.ToList();
    }
}
