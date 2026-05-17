namespace ErrorOr;

public static partial class ErrorOrExtensions
{
    /// <summary>
    /// Executes the appropriate action based on the state of the <see cref="ErrorOr{TValue}"/>.
    /// If the state is an error, the provided action <paramref name="onError"/> is executed.
    /// If the state is a value, the provided action <paramref name="onValue"/> is executed.
    /// </summary>
    /// <typeparam name="TValue">The type of the underlying value in the <paramref name="errorOr"/>.</typeparam>
    /// <param name="errorOr">The <see cref="ErrorOr"/> instance.</param>
    /// <param name="onValue">The action to execute if the state is a value.</param>
    /// <param name="onError">The action to execute if the state is an error.</param>
    /// <returns>The result of the executed function.</returns>
    public static async Task Switch<TValue>(this Task<ErrorOr<TValue>> errorOr, Action<TValue> onValue, Action<ReadOnlyCollection<Error>> onError)
    {
        var result = await errorOr.ConfigureAwait(false);

        result.Switch(onValue, onError);
    }

    /// <summary>
    /// Executes the appropriate action based on the state of the <see cref="ErrorOr{TValue}"/>.
    /// If the state is an error, the provided action <paramref name="onError"/> is executed.
    /// If the state is a value, the provided action <paramref name="onValue"/> is executed.
    /// </summary>
    /// <typeparam name="TValue">The type of the underlying value in the <paramref name="errorOr"/>.</typeparam>
    /// <param name="errorOr">The <see cref="ErrorOr"/> instance.</param>
    /// <param name="onValue">The action to execute if the state is a value.</param>
    /// <param name="onError">The action to execute if the state is an error.</param>
    /// <returns>The result of the executed function.</returns>
    public static async Task SwitchAsync<TValue>(this Task<ErrorOr<TValue>> errorOr, Func<TValue, Task> onValue, Func<ReadOnlyCollection<Error>, Task> onError)
    {
        var result = await errorOr.ConfigureAwait(false);

        await result.SwitchAsync(onValue, onError);
    }

    /// <summary>
    /// Executes the appropriate action based on the state of the <see cref="ErrorOr{TValue}"/>.
    /// If the state is an error, the provided action <paramref name="onError"/> is executed.
    /// If the state is a value, the provided action <paramref name="onValue"/> is executed.
    /// </summary>
    /// <typeparam name="TValue">The type of the underlying value in the <paramref name="errorOr"/>.</typeparam>
    /// <param name="errorOr">The <see cref="ErrorOr"/> instance.</param>
    /// <param name="onValue">The action to execute if the state is a value.</param>
    /// <param name="onError">The action to execute if the state is an error.</param>
    /// <returns>The result of the executed function.</returns>
    public static async Task SwitchFirst<TValue>(this Task<ErrorOr<TValue>> errorOr, Action<TValue> onValue, Action<Error> onError)
    {
        var result = await errorOr.ConfigureAwait(false);

        result.SwitchFirst(onValue, onError);
    }

    /// <summary>
    /// Executes the appropriate action based on the state of the <see cref="ErrorOr{TValue}"/>.
    /// If the state is an error, the provided action <paramref name="onError"/> is executed.
    /// If the state is a value, the provided action <paramref name="onValue"/> is executed.
    /// </summary>
    /// <typeparam name="TValue">The type of the underlying value in the <paramref name="errorOr"/>.</typeparam>
    /// <param name="errorOr">The <see cref="ErrorOr"/> instance.</param>
    /// <param name="onValue">The action to execute if the state is a value.</param>
    /// <param name="onError">The action to execute if the state is an error.</param>
    /// <returns>The result of the executed function.</returns>
    public static async Task SwitchFirstAsync<TValue>(this Task<ErrorOr<TValue>> errorOr, Func<TValue, Task> onValue, Func<Error, Task> onError)
    {
        var result = await errorOr.ConfigureAwait(false);

        await result.SwitchFirstAsync(onValue, onError);
    }
}
