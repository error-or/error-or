namespace ErrorOr;

public static partial class ErrorOrExtensions
{
    /// <summary>
    /// If the state of <paramref name="errorOr"/> is a value, the provided function <paramref name="onValue"/> is executed and its result is returned.
    /// </summary>
    /// <typeparam name="TValue">The type of the underlying value in the <paramref name="errorOr"/>.</typeparam>
    /// <typeparam name="TNextValue">The type of the result from invoking the <paramref name="onValue"/> function.</typeparam>
    /// <param name="errorOr">The <see cref="ErrorOr"/> instance.</param>
    /// <param name="onValue">The function to execute if the state is a value.</param>
    /// <returns>The result from calling <paramref name="onValue"/> if state is value; otherwise the original errors.</returns>
    public static async Task<ErrorOr<TNextValue>> Then<TValue, TNextValue>(this Task<ErrorOr<TValue>> errorOr, Func<TValue, ErrorOr<TNextValue>> onValue)
    {
        var result = await errorOr.ConfigureAwait(false);

        return result.Then(onValue);
    }

    /// <summary>
    /// If the state of <paramref name="errorOr"/> is a value, the provided function <paramref name="onValue"/> is executed and its result is returned.
    /// </summary>
    /// <typeparam name="TValue">The type of the underlying value in the <paramref name="errorOr"/>.</typeparam>
    /// <typeparam name="TNextValue">The type of the result from invoking the <paramref name="onValue"/> function.</typeparam>
    /// <param name="errorOr">The <see cref="ErrorOr"/> instance.</param>
    /// <param name="onValue">The function to execute if the state is a value.</param>
    /// <returns>The result from calling <paramref name="onValue"/> if state is value; otherwise the original errors.</returns>
    public static async Task<ErrorOr<TNextValue>> Then<TValue, TNextValue>(this Task<ErrorOr<TValue>> errorOr, Func<TValue, TNextValue> onValue)
    {
        var result = await errorOr.ConfigureAwait(false);

        return result.Then(onValue);
    }

    /// <summary>
    /// If the state of <paramref name="errorOr"/> is a value, the provided <paramref name="action"/> is invoked.
    /// </summary>
    /// <typeparam name="TValue">The type of the underlying value in the <paramref name="errorOr"/>.</typeparam>
    /// <param name="errorOr">The <see cref="ErrorOr"/> instance.</param>
    /// <param name="action">The action to execute if the state is a value.</param>
    /// <returns>The original <paramref name="errorOr"/>.</returns>
    public static async Task<ErrorOr<TValue>> ThenDo<TValue>(this Task<ErrorOr<TValue>> errorOr, Action<TValue> action)
    {
        var result = await errorOr.ConfigureAwait(false);

        return result.ThenDo(action);
    }

    /// <summary>
    /// If the state of <paramref name="errorOr"/> is a value, the provided function <paramref name="onValue"/> is executed asynchronously and its result is returned.
    /// </summary>
    /// <typeparam name="TValue">The type of the underlying value in the <paramref name="errorOr"/>.</typeparam>
    /// <typeparam name="TNextValue">The type of the result from invoking the <paramref name="onValue"/> function.</typeparam>
    /// <param name="errorOr">The <see cref="ErrorOr"/> instance.</param>
    /// <param name="onValue">The function to execute if the state is a value.</param>
    /// <returns>The result from calling <paramref name="onValue"/> if state is value; otherwise the original errors.</returns>
    public static async Task<ErrorOr<TNextValue>> ThenAsync<TValue, TNextValue>(this Task<ErrorOr<TValue>> errorOr, Func<TValue, Task<ErrorOr<TNextValue>>> onValue)
    {
        var result = await errorOr.ConfigureAwait(false);

        return await result.ThenAsync(onValue).ConfigureAwait(false);
    }

    /// <summary>
    /// If the state of <paramref name="errorOr"/> is a value, the provided function <paramref name="onValue"/> is executed asynchronously and its result is returned.
    /// </summary>
    /// <typeparam name="TValue">The type of the underlying value in the <paramref name="errorOr"/>.</typeparam>
    /// <typeparam name="TNextValue">The type of the result from invoking the <paramref name="onValue"/> function.</typeparam>
    /// <param name="errorOr">The <see cref="ErrorOr"/> instance.</param>
    /// <param name="onValue">The function to execute if the state is a value.</param>
    /// <returns>The result from calling <paramref name="onValue"/> if state is value; otherwise the original errors.</returns>
    public static async Task<ErrorOr<TNextValue>> ThenAsync<TValue, TNextValue>(this Task<ErrorOr<TValue>> errorOr, Func<TValue, Task<TNextValue>> onValue)
    {
        var result = await errorOr.ConfigureAwait(false);

        return await result.ThenAsync(onValue).ConfigureAwait(false);
    }

    /// <summary>
    /// If the state of <paramref name="errorOr"/> is a value, the provided <paramref name="action"/> is executed asynchronously.
    /// </summary>
    /// <typeparam name="TValue">The type of the underlying value in the <paramref name="errorOr"/>.</typeparam>
    /// <param name="errorOr">The <see cref="ErrorOr"/> instance.</param>
    /// <param name="action">The action to execute if the state is a value.</param>
    /// <returns>The original <paramref name="errorOr"/>.</returns>
    public static async Task<ErrorOr<TValue>> ThenDoAsync<TValue>(this Task<ErrorOr<TValue>> errorOr, Func<TValue, Task> action)
    {
        var result = await errorOr.ConfigureAwait(false);

        return await result.ThenDoAsync(action).ConfigureAwait(false);
    }

    /// <summary>
    /// If the state of <paramref name="errorOr"/> is a value, the provided function <paramref name="onValue"/> is executed and its errors are returned.
    /// If no errors are returned, the original value is returned.
    /// </summary>
    /// <typeparam name="TValue">The type of the underlying value in the <paramref name="errorOr"/>.</typeparam>
    /// <param name="errorOr">The <see cref="ErrorOr"/> instance.</param>
    /// <param name="onValue">The function to execute if the state is a value.</param>
    /// <returns>The errors from calling <paramref name="onValue"/> if state is value and errors are returned; otherwise the original <paramref name="errorOr"/>.</returns>
    public static async Task<ErrorOr<TValue>> ThenEnsure<TValue>(this Task<ErrorOr<TValue>> errorOr, Func<TValue, ErrorOr<TValue>> onValue)
    {
        var result = await errorOr.ConfigureAwait(false);

        return result.ThenEnsure(onValue);
    }

    /// <summary>
    /// If the state of <paramref name="errorOr"/> is a value, the provided function <paramref name="onValue"/> is executed asynchronously and its errors are returned.
    /// If no errors are returned, the original value is returned.
    /// </summary>
    /// <typeparam name="TValue">The type of the underlying value in the <paramref name="errorOr"/>.</typeparam>
    /// <param name="errorOr">The <see cref="ErrorOr"/> instance.</param>
    /// <param name="onValue">The function to execute if the state is a value.</param>
    /// <returns>The errors from calling <paramref name="onValue"/> if state is value and errors are returned; otherwise the original <paramref name="errorOr"/>.</returns>
    public static async Task<ErrorOr<TValue>> ThenEnsureAsync<TValue>(this Task<ErrorOr<TValue>> errorOr, Func<TValue, Task<ErrorOr<TValue>>> onValue)
    {
        var result = await errorOr.ConfigureAwait(false);

        return await result.ThenEnsureAsync(onValue).ConfigureAwait(false);
    }
}
