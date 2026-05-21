namespace ErrorOr;

/// <summary>
/// Provides factory methods for creating instances of <see cref="ErrorOr{TValue}"/>.
/// </summary>
public static class ErrorOrFactory
{
    /// <summary>
    /// Creates a new instance of <see cref="ErrorOr{TValue}"/> from a value.
    /// </summary>
    /// <typeparam name="TValue">The type of the value.</typeparam>
    /// <param name="value">The value to wrap.</param>
    /// <returns>An instance of <see cref="ErrorOr{TValue}"/> containing the provided value.</returns>
    public static ErrorOr<TValue> From<TValue>(TValue value) => value;

    /// <summary>
    /// Creates a new awaitable instance of <see cref="ErrorOr{TValue}"/> from value.
    /// </summary>
    /// <typeparam name="TValue">The type of the value.</typeparam>
    /// <param name="value">The value to wrap.</param>
    /// <returns>An awaitable instance of <see cref="ErrorOr{TValue}"/> containing provided value.</returns>
    public static Task<ErrorOr<TValue>> FromAsync<TValue>(TValue value) => Task.FromResult(From(value));

    /// <summary>
    /// Creates a new instance of <see cref="ErrorOr{TValue}"/> from single error.
    /// </summary>
    /// <typeparam name="TValue">The type of the value.</typeparam>
    /// <param name="error">Single error instance to wrap.</param>
    /// <returns>An instance of <see cref="ErrorOr{TValue}"/> containing the provided error.</returns>
    public static ErrorOr<TValue> From<TValue>(Error error) => error;

    /// <summary>
    /// Creates a new awaitable instance of <see cref="ErrorOr{TValue}"/> from single error.
    /// </summary>
    /// <typeparam name="TValue">The type of the value.</typeparam>
    /// <param name="error">Single error instance to wrap.</param>
    /// <returns>An awaitable instance of <see cref="ErrorOr{TValue}"/> containing the provided error.</returns>
    public static Task<ErrorOr<TValue>> FromAsync<TValue>(Error error) => Task.FromResult(From<TValue>(error));

    /// <summary>
    /// Creates an <see cref="ErrorOr{TValue}"/> from a read-only span of errors.
    /// </summary>
    /// <typeparam name="TValue">The type of the value.</typeparam>
    /// <param name="errors">Read-only span of errors.</param>
    /// <returns>An instance of <see cref="ErrorOr{TValue}"/> containing provided read-only span of errors.</returns>
    public static ErrorOr<TValue> From<TValue>(ReadOnlySpan<Error> errors) => errors.ToArray();

    /// <summary>
    /// Creates an awaitable instance of<see cref="ErrorOr{TValue}"/> from a read-only span of errors.
    /// </summary>
    /// <typeparam name="TValue">The type of the value.</typeparam>
    /// <param name="errors">Read-only span of errors.</param>
    /// <returns>An awaitable instance of <see cref="ErrorOr{TValue}"/> containing provided read-only span of errors.</returns>
    public static Task<ErrorOr<TValue>> FromAsync<TValue>(ReadOnlySpan<Error> errors) => Task.FromResult(From<TValue>(errors));
}
