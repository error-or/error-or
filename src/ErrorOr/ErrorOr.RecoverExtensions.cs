namespace ErrorOr;

public static partial class ErrorOrExtensions
{
    /// <summary>
    /// If the state of <paramref name="errorOr"/> is error, the provided function <paramref name="onError"/> is executed and its result is returned.
    /// </summary>
    /// <typeparam name="TValue">The type of the underlying value in the <paramref name="errorOr"/>.</typeparam>
    /// <param name="errorOr">The <see cref="ErrorOr"/> instance.</param>
    /// <param name="onError">The function to execute if the state is error.</param>
    /// <returns>The result from calling <paramref name="onError"/> if state is error; otherwise the original value.</returns>
    public static async Task<ErrorOr<TValue>> Recover<TValue>(
        this Task<ErrorOr<TValue>> errorOr,
        Func<List<Error>, ErrorOr<TValue>> onError)
    {
        var result = await errorOr.ConfigureAwait(false);

        return result.Recover(onError);
    }

    /// <summary>
    /// If the state of <paramref name="errorOr"/> is error, the provided function <paramref name="onError"/> is executed asynchronously and its result is returned.
    /// </summary>
    /// <typeparam name="TValue">The type of the underlying value in the <paramref name="errorOr"/>.</typeparam>
    /// <param name="errorOr">The <see cref="ErrorOr"/> instance.</param>
    /// <param name="onError">The function to execute if the state is error.</param>
    /// <returns>The result from calling <paramref name="onError"/> if state is error; otherwise the original value.</returns>
    public static async Task<ErrorOr<TValue>> RecoverAsync<TValue>(
        this Task<ErrorOr<TValue>> errorOr,
        Func<List<Error>, Task<ErrorOr<TValue>>> onError)
    {
        var result = await errorOr.ConfigureAwait(false);

        return await result.RecoverAsync(onError).ConfigureAwait(false);
    }
}
