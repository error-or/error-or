namespace ErrorOr;

public readonly partial record struct ErrorOr<TValue> : IErrorOr<TValue>
{
    /// <summary>
    /// Executes the appropriate function based on the state of the <see cref="ErrorOr{TValue}"/>.
    /// If the state is a value, the provided function <paramref name="onValue"/> is executed and its result is returned.
    /// If the state is an error, the provided function <paramref name="onError"/> is executed and its result is returned.
    /// </summary>
    /// <typeparam name="TNextValue">The type of the result.</typeparam>
    /// <param name="onValue">The function to execute if the state is a value.</param>
    /// <param name="onError">The function to execute if the state is an error.</param>
    /// <returns>The result of the executed function.</returns>
    public TNextValue Match<TNextValue>(Func<TValue, TNextValue> onValue, Func<ReadOnlyCollection<Error>, TNextValue> onError)
    {
        if (IsError)
        {
            return onError(Errors);
        }

        return onValue(Value);
    }

    /// <summary>
    /// Asynchronously executes the appropriate function based on the state of the <see cref="ErrorOr{TValue}"/>.
    /// If the state is a value, the provided function <paramref name="onValue"/> is executed asynchronously and its result is returned.
    /// If the state is an error, the provided function <paramref name="onError"/> is executed asynchronously and its result is returned.
    /// </summary>
    /// <typeparam name="TNextValue">The type of the result.</typeparam>
    /// <param name="onValue">The asynchronous function to execute if the state is a value.</param>
    /// <param name="onError">The asynchronous function to execute if the state is an error.</param>
    /// <returns>A task representing the asynchronous operation that yields the result of the executed function.</returns>
    public async Task<TNextValue> MatchAsync<TNextValue>(Func<TValue, Task<TNextValue>> onValue, Func<ReadOnlyCollection<Error>, Task<TNextValue>> onError)
    {
        if (IsError)
        {
            return await onError(Errors).ConfigureAwait(false);
        }

        return await onValue(Value).ConfigureAwait(false);
    }

    /// <summary>
    /// Executes the appropriate function based on the state of the <see cref="ErrorOr{TValue}"/>.
    /// If the state is a value, the provided function <paramref name="onValue"/> is executed and its result is returned.
    /// If the state is an error, the provided function <paramref name="onFirstError"/> is executed using the first error, and its result is returned.
    /// </summary>
    /// <typeparam name="TNextValue">The type of the result.</typeparam>
    /// <param name="onValue">The function to execute if the state is a value.</param>
    /// <param name="onFirstError">The function to execute with the first error if the state is an error.</param>
    /// <returns>The result of the executed function.</returns>
    public TNextValue MatchFirst<TNextValue>(Func<TValue, TNextValue> onValue, Func<Error, TNextValue> onFirstError)
    {
        if (IsError)
        {
            return onFirstError(FirstError);
        }

        return onValue(Value);
    }

    /// <summary>
    /// Asynchronously executes the appropriate function based on the state of the <see cref="ErrorOr{TValue}"/>.
    /// If the state is a value, the provided function <paramref name="onValue"/> is executed asynchronously and its result is returned.
    /// If the state is an error, the provided function <paramref name="onFirstError"/> is executed asynchronously using the first error, and its result is returned.
    /// </summary>
    /// <typeparam name="TNextValue">The type of the result.</typeparam>
    /// <param name="onValue">The asynchronous function to execute if the state is a value.</param>
    /// <param name="onFirstError">The asynchronous function to execute with the first error if the state is an error.</param>
    /// <returns>A task representing the asynchronous operation that yields the result of the executed function.</returns>
    public async Task<TNextValue> MatchFirstAsync<TNextValue>(Func<TValue, Task<TNextValue>> onValue, Func<Error, Task<TNextValue>> onFirstError)
    {
        if (IsError)
        {
            return await onFirstError(FirstError).ConfigureAwait(false);
        }

        return await onValue(Value).ConfigureAwait(false);
    }
}
