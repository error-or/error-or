namespace ErrorOr;

public static partial class ErrorOrExtensions
{
    /// <summary>
    /// Creates an <see cref="ErrorOr{TValue}"/> instance with the given value.
    /// </summary>
    /// <param name="value">The value to create the <see cref="ErrorOr{TValue}"/> instance with.</param>
    public static ErrorOr<TValue> ToErrorOr<TValue>(this TValue value)
    {
        return value;
    }

    /// <summary>
    /// Creates an awaitable <see cref="ErrorOr{TValue}"/> instance with the result of the given awaitable value.
    /// </summary>
    /// <param name="value">The awaitable value to create the <see cref="ErrorOr{TValue}"/> instance with.</param>
    public static async Task<ErrorOr<TValue>> ToErrorOrAsync<TValue>(this Task<TValue> value)
    {
        return await value.ConfigureAwait(false);
    }

    /// <summary>
    /// Creates an <see cref="ErrorOr{TValue}"/> instance with the given error.
    /// </summary>
    /// <param name="error">The error to create the <see cref="ErrorOr{TValue}"/> instance with.</param>
    public static ErrorOr<TValue> ToErrorOr<TValue>(this Error error)
    {
        return error;
    }

    /// <summary>
    /// Creates an awaitable <see cref="ErrorOr{TValue}"/> instance with the given awaitable error.
    /// </summary>
    /// <param name="error">The awaitable error to create the <see cref="ErrorOr{TValue}"/> instance with.</param>
    public static async Task<ErrorOr<TValue>> ToErrorOrAsync<TValue>(this Task<Error> error)
    {
        return await error.ConfigureAwait(false);
    }

    /// <summary>
    /// Creates an <see cref="ErrorOr{TValue}"/> instance with the given list of errors.
    /// </summary>
    /// <param name="errors">The list of errors to create the <see cref="ErrorOr{TValue}"/> instance with.</param>
    public static ErrorOr<TValue> ToErrorOr<TValue>(this List<Error> errors)
    {
        return errors;
    }

    /// <summary>
    /// Creates an awaitable <see cref="ErrorOr{TValue}"/> instance with the given awaitable list of errors.
    /// </summary>
    /// <param name="errors">The awaitable list of errors to create the <see cref="ErrorOr{TValue}"/> instance with.</param>
    public static async Task<ErrorOr<TValue>> ToErrorOrAsync<TValue>(this Task<List<Error>> errors)
    {
        return await errors.ConfigureAwait(false);
    }

    /// <summary>
    /// Creates an <see cref="ErrorOr{TValue}"/> instance with the given enumeration of errors.
    /// </summary>
    /// <param name="errors">The enumeration of errors to create the <see cref="ErrorOr{TValue}"/> instance with.</param>
    public static ErrorOr<TValue> ToErrorOr<TValue>(this IEnumerable<Error> errors)
    {
        return errors.ToList();
    }

    /// <summary>
    /// Creates an awaitable <see cref="ErrorOr{TValue}"/> instance with the given awaitable array of errors.
    /// </summary>
    /// <param name="errors">The array of errors to create the <see cref="ErrorOr{TValue}"/> instance with.</param>
    public static async Task<ErrorOr<TValue>> ToErrorOrAsync<TValue>(this Task<Error[]> errors)
    {
        var errorArray = await errors.ConfigureAwait(false);
        return errorArray.ToList();
    }

    /// <summary>
    /// Creates an awaitable <see cref="ErrorOr{TValue}"/> instance with the given awaitable enumeration of errors.
    /// </summary>
    /// <param name="errors">The enumeration of errors to create the <see cref="ErrorOr{TValue}"/> instance with.</param>
    public static async Task<ErrorOr<TValue>> ToErrorOrAsync<TValue>(this Task<IEnumerable<Error>> errors)
    {
        var errorEnumeration = await errors.ConfigureAwait(false);
        return errorEnumeration.ToList();
    }
}
