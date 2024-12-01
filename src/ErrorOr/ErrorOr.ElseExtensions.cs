namespace ErrorOr;

public static partial class ErrorOrExtensions
{
    /// <summary>
    /// If the state is error, the provided function <paramref name="onError"/> is executed asynchronously and its result is returned.
    /// </summary>
    /// <typeparam name="TValue">The type of the underlying value in the <paramref name="errorOr"/>.</typeparam>
    /// <param name="errorOr">The <see cref="ErrorOr"/> instance.</param>
    /// <param name="onError">The function to execute if the state is error.</param>
    /// <returns>The result from calling <paramref name="onError"/> if state is error; otherwise the original value.</returns>
    public static async Task<ErrorOr<TValue>> Else<TValue>(this Task<ErrorOr<TValue>> errorOr, Func<ReadOnlyCollection<Error>, TValue> onError)
    {
        var result = await errorOr.ConfigureAwait(false);

        return result.Else(onError);
    }

    /// <summary>
    /// If the state is error, the provided function <paramref name="onError"/> is executed asynchronously and its result is returned.
    /// </summary>
    /// <typeparam name="TValue">The type of the underlying value in the <paramref name="errorOr"/>.</typeparam>
    /// <param name="errorOr">The <see cref="ErrorOr"/> instance.</param>
    /// <param name="onError">The function to execute if the state is error.</param>
    /// <returns>The result from calling <paramref name="onError"/> if state is error; otherwise the original value.</returns>
    public static async Task<ErrorOr<TValue>> Else<TValue>(this Task<ErrorOr<TValue>> errorOr, TValue onError)
    {
        var result = await errorOr.ConfigureAwait(false);

        return result.Else(onError);
    }

    /// <summary>
    /// If the state is error, the provided function <paramref name="onError"/> is executed asynchronously and its result is returned.
    /// </summary>
    /// <typeparam name="TValue">The type of the underlying value in the <paramref name="errorOr"/>.</typeparam>
    /// <param name="errorOr">The <see cref="ErrorOr"/> instance.</param>
    /// <param name="onError">The function to execute if the state is error.</param>
    /// <returns>The result from calling <paramref name="onError"/> if state is error; otherwise the original value.</returns>
    public static async Task<ErrorOr<TValue>> ElseAsync<TValue>(this Task<ErrorOr<TValue>> errorOr, Func<ReadOnlyCollection<Error>, Task<TValue>> onError)
    {
        var result = await errorOr.ConfigureAwait(false);

        return await result.ElseAsync(onError).ConfigureAwait(false);
    }

    /// <summary>
    /// If the state is error, the provided function <paramref name="onError"/> is executed asynchronously and its result is returned.
    /// </summary>
    /// <typeparam name="TValue">The type of the underlying value in the <paramref name="errorOr"/>.</typeparam>
    /// <param name="errorOr">The <see cref="ErrorOr"/> instance.</param>
    /// <param name="onError">The function to execute if the state is error.</param>
    /// <returns>The result from calling <paramref name="onError"/> if state is error; otherwise the original value.</returns>
    public static async Task<ErrorOr<TValue>> ElseAsync<TValue>(this Task<ErrorOr<TValue>> errorOr, Task<TValue> onError)
    {
        var result = await errorOr.ConfigureAwait(false);

        return await result.ElseAsync(onError).ConfigureAwait(false);
    }

    /// <summary>
    /// If the state is error, the provided function <paramref name="onError"/> is executed asynchronously and its result is returned.
    /// </summary>
    /// <typeparam name="TValue">The type of the underlying value in the <paramref name="errorOr"/>.</typeparam>
    /// <param name="errorOr">The <see cref="ErrorOr"/> instance.</param>
    /// <param name="onError">The function to execute if the state is error.</param>
    /// <returns>The result from calling <paramref name="onError"/> if state is error; otherwise the original value.</returns>
    public static async Task<ErrorOr<TValue>> Else<TValue>(this Task<ErrorOr<TValue>> errorOr, Func<ReadOnlyCollection<Error>, Error> onError)
    {
        var result = await errorOr.ConfigureAwait(false);

        return result.Else(onError);
    }

    /// <summary>
    /// If the state is error, the provided function <paramref name="onError"/> is executed asynchronously and its result is returned.
    /// </summary>
    /// <typeparam name="TValue">The type of the underlying value in the <paramref name="errorOr"/>.</typeparam>
    /// <param name="errorOr">The <see cref="ErrorOr"/> instance.</param>
    /// <param name="onError">The function to execute if the state is error.</param>
    /// <returns>The result from calling <paramref name="onError"/> if state is error; otherwise the original value.</returns>
    public static async Task<ErrorOr<TValue>> Else<TValue>(this Task<ErrorOr<TValue>> errorOr, Func<ReadOnlyCollection<Error>, List<Error>> onError)
    {
        var result = await errorOr.ConfigureAwait(false);

        return result.Else(onError);
    }

    /// <summary>
    /// If the state is error, the provided <paramref name="error"/> is returned.
    /// </summary>
    /// <typeparam name="TValue">The type of the underlying value in the <paramref name="errorOr"/>.</typeparam>
    /// <param name="errorOr">The <see cref="ErrorOr"/> instance.</param>
    /// <param name="error">The error to return.</param>
    /// <returns>The given <paramref name="error"/>.</returns>
    public static async Task<ErrorOr<TValue>> Else<TValue>(this Task<ErrorOr<TValue>> errorOr, Error error)
    {
        var result = await errorOr.ConfigureAwait(false);

        return result.Else(error);
    }

    /// <summary>
    /// If the state is error, the provided function <paramref name="onError"/> is executed asynchronously and its result is returned.
    /// </summary>
    /// <typeparam name="TValue">The type of the underlying value in the <paramref name="errorOr"/>.</typeparam>
    /// <param name="errorOr">The <see cref="ErrorOr"/> instance.</param>
    /// <param name="onError">The function to execute if the state is error.</param>
    /// <returns>The result from calling <paramref name="onError"/> if state is error; otherwise the original value.</returns>
    public static async Task<ErrorOr<TValue>> ElseAsync<TValue>(this Task<ErrorOr<TValue>> errorOr, Func<ReadOnlyCollection<Error>, Task<Error>> onError)
    {
        var result = await errorOr.ConfigureAwait(false);

        return await result.ElseAsync(onError).ConfigureAwait(false);
    }

    /// <summary>
    /// If the state is error, the provided function <paramref name="onError"/> is executed asynchronously and its result is returned.
    /// </summary>
    /// <typeparam name="TValue">The type of the underlying value in the <paramref name="errorOr"/>.</typeparam>
    /// <param name="errorOr">The <see cref="ErrorOr"/> instance.</param>
    /// <param name="onError">The function to execute if the state is error.</param>
    /// <returns>The result from calling <paramref name="onError"/> if state is error; otherwise the original value.</returns>
    public static async Task<ErrorOr<TValue>> ElseAsync<TValue>(this Task<ErrorOr<TValue>> errorOr, Func<ReadOnlyCollection<Error>, Task<List<Error>>> onError)
    {
        var result = await errorOr.ConfigureAwait(false);

        return await result.ElseAsync(onError).ConfigureAwait(false);
    }

    /// <summary>
    /// If the state is error, the provided function <paramref name="onError"/> is executed asynchronously and its result is returned.
    /// </summary>
    /// <typeparam name="TValue">The type of the underlying value in the <paramref name="errorOr"/>.</typeparam>
    /// <param name="errorOr">The <see cref="ErrorOr"/> instance.</param>
    /// <param name="onError">The function to execute if the state is error.</param>
    /// <returns>The result from calling <paramref name="onError"/> if state is error; otherwise the original value.</returns>
    public static async Task<ErrorOr<TValue>> ElseAsync<TValue>(this Task<ErrorOr<TValue>> errorOr, Task<Error> onError)
    {
        var result = await errorOr.ConfigureAwait(false);

        return await result.ElseAsync(onError).ConfigureAwait(false);
    }
}
