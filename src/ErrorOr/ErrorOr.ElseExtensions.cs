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
    public static async Task<ErrorOr<TValue>> Else<TValue>(this Task<ErrorOr<TValue>> errorOr, Func<List<Error>, TValue> onError)
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
    public static async Task<ErrorOr<TValue>> ElseAsync<TValue>(this Task<ErrorOr<TValue>> errorOr, Func<List<Error>, Task<TValue>> onError)
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
    public static async Task<ErrorOr<TValue>> Else<TValue>(this Task<ErrorOr<TValue>> errorOr, Func<List<Error>, Error> onError)
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
    public static async Task<ErrorOr<TValue>> Else<TValue>(this Task<ErrorOr<TValue>> errorOr, Func<List<Error>, List<Error>> onError)
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
    public static async Task<ErrorOr<TValue>> ElseAsync<TValue>(this Task<ErrorOr<TValue>> errorOr, Func<List<Error>, Task<Error>> onError)
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
    public static async Task<ErrorOr<TValue>> ElseAsync<TValue>(this Task<ErrorOr<TValue>> errorOr, Func<List<Error>, Task<List<Error>>> onError)
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

    /// <summary>
    /// If the state of <paramref name="errorOr"/> is error, the provided <paramref name="action"/> is invoked.
    /// </summary>
    /// <typeparam name="TValue">The type of the underlying value in the <paramref name="errorOr"/>.</typeparam>
    /// <param name="errorOr">The <see cref="ErrorOr"/> instance.</param>
    /// <param name="action">The action to execute if the state is error.</param>
    /// <returns>The original <paramref name="errorOr"/>.</returns>
    public static async Task<ErrorOr<TValue>> ElseDo<TValue>(this Task<ErrorOr<TValue>> errorOr, Action<List<Error>> action)
    {
        var result = await errorOr.ConfigureAwait(false);

        return result.ElseDo(action);
    }

    /// <summary>
    /// If the state of <paramref name="errorOr"/> is error, the provided <paramref name="action"/> is executed asynchronously.
    /// </summary>
    /// <typeparam name="TValue">The type of the underlying value in the <paramref name="errorOr"/>.</typeparam>
    /// <param name="errorOr">The <see cref="ErrorOr"/> instance.</param>
    /// <param name="action">The action to execute if the state is error.</param>
    /// <returns>The original <paramref name="errorOr"/>.</returns>
    public static async Task<ErrorOr<TValue>> ElseDoAsync<TValue>(this Task<ErrorOr<TValue>> errorOr, Func<List<Error>, Task> action)
    {
        var result = await errorOr.ConfigureAwait(false);

        return await result.ElseDoAsync(action).ConfigureAwait(false);
    }
}
