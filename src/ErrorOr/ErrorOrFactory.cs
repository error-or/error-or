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
    /// Creates a new awaitable instance of <see cref="ErrorOr{TValue}"/> from single error.
    /// </summary>
    /// <typeparam name="TValue">The type of the value.</typeparam>
    /// <param name="error">Single error instance to wrap.</param>
    /// <returns>An awaitable instance of <see cref="ErrorOr{TValue}"/> containing the provided error.</returns>
    public static Task<ErrorOr<TValue>> FromAsync<TValue>(Error error)
    {
        return Task.FromResult(From<TValue>(error));
    }

    /// <summary>
    /// Creates an <see cref="ErrorOr{TValue}"/> from a list of errors.
    /// </summary>
    /// <typeparam name="TValue">The type of the value.</typeparam>
    /// <param name="errors">List of errors.</param>
    /// <returns>An instance of <see cref="ErrorOr{TValue}"/> containing provided list of errors.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="errors"/> is null.</exception>
    /// <exception cref="ArgumentException">Thrown when <paramref name="errors" /> is an empty list.</exception>
    public static ErrorOr<TValue> From<TValue>(List<Error> errors)
    {
        return errors;
    }

    /// <summary>
    /// Creates an awaitable <see cref="ErrorOr{TValue}"/> from a list of errors.
    /// </summary>
    /// <typeparam name="TValue">The type of the value.</typeparam>
    /// <param name="errors">List of errors.</param>
    /// <returns>An awaitable instance of <see cref="ErrorOr{TValue}"/> containing provided list of errors.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="errors"/> is null.</exception>
    /// <exception cref="ArgumentException">Thrown when <paramref name="errors" /> is an empty list.</exception>
    public static Task<ErrorOr<TValue>> FromAsync<TValue>(List<Error> errors)
    {
        return Task.FromResult(From<TValue>(errors));
    }

    /// <summary>
    /// Creates an <see cref="ErrorOr{TValue}"/> from an enumeration of errors.
    /// </summary>
    /// <typeparam name="TValue">The type of the value.</typeparam>
    /// <param name="errors">Enumeration of errors.</param>
    /// <returns>An instance of <see cref="ErrorOr{TValue}"/> containing provided enumeration of errors.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="errors"/> is null.</exception>
    /// <exception cref="ArgumentException">Thrown when <paramref name="errors" /> is an empty enumerable.</exception>
    public static ErrorOr<TValue> From<TValue>(IEnumerable<Error> errors)
    {
        return errors.ToList();
    }

    /// <summary>
    /// Creates an awaitable <see cref="ErrorOr{TValue}"/> from an enumeration of errors.
    /// </summary>
    /// <typeparam name="TValue">The type of the value.</typeparam>
    /// <param name="errors">Enumeration of errors.</param>
    /// <returns>An awaitable instance of <see cref="ErrorOr{TValue}"/> containing provided enumeration of errors.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="errors"/> is null.</exception>
    /// <exception cref="ArgumentException">Thrown when <paramref name="errors" /> is an empty enumerable.</exception>
    public static Task<ErrorOr<TValue>> FromAsync<TValue>(IEnumerable<Error> errors)
    {
        return Task.FromResult(From<TValue>(errors));
    }
}
