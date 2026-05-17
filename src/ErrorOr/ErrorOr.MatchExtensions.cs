namespace ErrorOr;

public static partial class ErrorOrExtensions
{
    /// <summary>
    /// Executes the appropriate function based on the state of the <see cref="ErrorOr{TValue}"/>.
    /// If the state is a value, the provided function <paramref name="onValue"/> is executed and its result is returned.
    /// If the state is an error, the provided function <paramref name="onError"/> is executed and its result is returned.
    /// </summary>
    /// <typeparam name="TValue">The type of the underlying value in the <paramref name="errorOr"/>.</typeparam>
    /// <typeparam name="TNextValue">The type of the result from invoking the <paramref name="onError"/> and <paramref name="onValue"/> functions.</typeparam>
    /// <param name="errorOr">The <see cref="ErrorOr"/> instance.</param>
    /// <param name="onValue">The function to execute if the state is a value.</param>
    /// <param name="onError">The function to execute if the state is an error.</param>
    /// <returns>The result of the executed function.</returns>
    public static async Task<TNextValue> Match<TValue, TNextValue>(this Task<ErrorOr<TValue>> errorOr, Func<TValue, TNextValue> onValue, Func<ReadOnlyCollection<Error>, TNextValue> onError)
    {
        var result = await errorOr.ConfigureAwait(false);

        return result.Match(onValue, onError);
    }

    /// <summary>
    /// Asynchronously executes the appropriate function based on the state of the <see cref="ErrorOr{TValue}"/>.
    /// If the state is a value, the provided function <paramref name="onValue"/> is executed asynchronously and its result is returned.
    /// If the state is an error, the provided function <paramref name="onError"/> is executed asynchronously and its result is returned.
    /// </summary>
    /// <typeparam name="TValue">The type of the underlying value in the <paramref name="errorOr"/>.</typeparam>
    /// <typeparam name="TNextValue">The type of the result from invoking the <paramref name="onError"/> and <paramref name="onValue"/> functions.</typeparam>
    /// <param name="errorOr">The <see cref="ErrorOr"/> instance.</param>
    /// <param name="onValue">The function to execute if the state is a value.</param>
    /// <param name="onError">The function to execute if the state is an error.</param>
    /// <returns>The result of the executed function.</returns>
    public static async Task<TNextValue> MatchAsync<TValue, TNextValue>(this Task<ErrorOr<TValue>> errorOr, Func<TValue, Task<TNextValue>> onValue, Func<ReadOnlyCollection<Error>, Task<TNextValue>> onError)
    {
        var result = await errorOr.ConfigureAwait(false);

        return await result.MatchAsync(onValue, onError);
    }

    /// <summary>
    /// Executes the appropriate function based on the state of the <see cref="ErrorOr{TValue}"/>.
    /// If the state is a value, the provided function <paramref name="onValue"/> is executed and its result is returned.
    /// If the state is an error, the provided function <paramref name="onError"/> is executed and its result is returned.
    /// </summary>
    /// <typeparam name="TValue">The type of the underlying value in the <paramref name="errorOr"/>.</typeparam>
    /// <typeparam name="TNextValue">The type of the result from invoking the <paramref name="onError"/> and <paramref name="onValue"/> functions.</typeparam>
    /// <param name="errorOr">The <see cref="ErrorOr"/> instance.</param>
    /// <param name="onValue">The function to execute if the state is a value.</param>
    /// <param name="onError">The function to execute if the state is an error.</param>
    /// <returns>The result of the executed function.</returns>
    public static async Task<TNextValue> MatchFirst<TValue, TNextValue>(this Task<ErrorOr<TValue>> errorOr, Func<TValue, TNextValue> onValue, Func<Error, TNextValue> onError)
    {
        var result = await errorOr.ConfigureAwait(false);

        return result.MatchFirst(onValue, onError);
    }

    /// <summary>
    /// Asynchronously executes the appropriate function based on the state of the <see cref="ErrorOr{TValue}"/>.
    /// If the state is a value, the provided function <paramref name="onValue"/> is executed asynchronously and its result is returned.
    /// If the state is an error, the provided function <paramref name="onError"/> is executed asynchronously and its result is returned.
    /// </summary>
    /// <typeparam name="TValue">The type of the underlying value in the <paramref name="errorOr"/>.</typeparam>
    /// <typeparam name="TNextValue">The type of the result from invoking the <paramref name="onError"/> and <paramref name="onValue"/> functions.</typeparam>
    /// <param name="errorOr">The <see cref="ErrorOr"/> instance.</param>
    /// <param name="onValue">The function to execute if the state is a value.</param>
    /// <param name="onError">The function to execute if the state is an error.</param>
    /// <returns>The result of the executed function.</returns>
    public static async Task<TNextValue> MatchFirstAsync<TValue, TNextValue>(this Task<ErrorOr<TValue>> errorOr, Func<TValue, Task<TNextValue>> onValue, Func<Error, Task<TNextValue>> onError)
    {
        var result = await errorOr.ConfigureAwait(false);

        return await result.MatchFirstAsync(onValue, onError);
    }
}
