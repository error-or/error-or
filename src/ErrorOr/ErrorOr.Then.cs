namespace ErrorOr;

public readonly partial record struct ErrorOr<TValue> : IErrorOr<TValue>
{
    /// <summary>
    /// If the state is a value, the provided function <paramref name="onValue"/> is executed and its result is returned.
    /// </summary>
    /// <typeparam name="TNextValue">The type of the result.</typeparam>
    /// <param name="onValue">The function to execute if the state is a value.</param>
    /// <returns>The result from calling <paramref name="onValue"/> if state is value; otherwise the original <see cref="Errors"/>.</returns>
    public ErrorOr<TNextValue> Then<TNextValue>(Func<TValue, ErrorOr<TNextValue>> onValue)
    {
        if (IsError)
        {
            return Errors;
        }

        return onValue(Value);
    }

    /// <summary>
    /// If the state is a value, the provided <paramref name="action"/> is invoked.
    /// </summary>
    /// <param name="action">The action to execute if the state is a value.</param>
    /// <returns>The original <see cref="ErrorOr"/> instance.</returns>
    public ErrorOr<TValue> ThenDo(Action<TValue> action)
    {
        if (IsError)
        {
            return Errors;
        }

        action(Value);

        return this;
    }

    /// <summary>
    /// If the state is a value, the provided function <paramref name="onValue"/> is executed and its errors are returned.
    /// If no errors are returned, the original value is returned.
    /// </summary>
    /// <param name="onValue">The function to execute if the state is a value.</param>
    /// <returns>The errors from calling <paramref name="onValue"/> if state is value and errors are returned; otherwise the original <see cref="ErrorOr"/> instance.</returns>
    public ErrorOr<TValue> ThenEnsure(Func<TValue, ErrorOr<TValue>> onValue)
    {
        if (IsError)
        {
            return Errors;
        }

        ErrorOr<TValue> result = onValue(Value);

        return result.IsError ? result.Errors : this;
    }

    /// <summary>
    /// If the state is a value, the provided function <paramref name="onValue"/> is executed and its result is returned.
    /// </summary>
    /// <typeparam name="TNextValue">The type of the result.</typeparam>
    /// <param name="onValue">The function to execute if the state is a value.</param>
    /// <returns>The result from calling <paramref name="onValue"/> if state is value; otherwise the original <see cref="Errors"/>.</returns>
    public ErrorOr<TNextValue> Then<TNextValue>(Func<TValue, TNextValue> onValue)
    {
        if (IsError)
        {
            return Errors;
        }

        return onValue(Value);
    }

    /// <summary>
    /// If the state is a value, the provided function <paramref name="onValue"/> is executed asynchronously and its result is returned.
    /// </summary>
    /// <typeparam name="TNextValue">The type of the result.</typeparam>
    /// <param name="onValue">The function to execute if the state is a value.</param>
    /// <returns>The result from calling <paramref name="onValue"/> if state is value; otherwise the original <see cref="Errors"/>.</returns>
    public async Task<ErrorOr<TNextValue>> ThenAsync<TNextValue>(Func<TValue, Task<ErrorOr<TNextValue>>> onValue)
    {
        if (IsError)
        {
            return Errors;
        }

        return await onValue(Value).ConfigureAwait(false);
    }

    /// <summary>
    /// If the state is a value, the provided <paramref name="action"/> is invoked asynchronously.
    /// </summary>
    /// <param name="action">The action to execute if the state is a value.</param>
    /// <returns>The original <see cref="ErrorOr"/> instance.</returns>
    public async Task<ErrorOr<TValue>> ThenDoAsync(Func<TValue, Task> action)
    {
        if (IsError)
        {
            return Errors;
        }

        await action(Value).ConfigureAwait(false);

        return this;
    }

    /// <summary>
    /// If the state is a value, the provided function <paramref name="onValue"/> is executed asynchronously and its errors are returned.
    /// If no errors are returned, the original value is returned.
    /// </summary>
    /// <param name="onValue">The function to execute if the state is a value.</param>
    /// <returns>The errors from calling <paramref name="onValue"/> if state is value and errors are returned; otherwise the original <see cref="ErrorOr"/> instance.</returns>
    public async Task<ErrorOr<TValue>> ThenEnsureAsync(Func<TValue, Task<ErrorOr<TValue>>> onValue)
    {
        if (IsError)
        {
            return Errors;
        }

        ErrorOr<TValue> result = await onValue(Value).ConfigureAwait(false);

        return result.IsError ? result.Errors : this;
    }

    /// <summary>
    /// If the state is a value, the provided function <paramref name="onValue"/> is executed asynchronously and its result is returned.
    /// </summary>
    /// <typeparam name="TNextValue">The type of the result.</typeparam>
    /// <param name="onValue">The function to execute if the state is a value.</param>
    /// <returns>The result from calling <paramref name="onValue"/> if state is value; otherwise the original <see cref="Errors"/>.</returns>
    public async Task<ErrorOr<TNextValue>> ThenAsync<TNextValue>(Func<TValue, Task<TNextValue>> onValue)
    {
        if (IsError)
        {
            return Errors;
        }

        return await onValue(Value).ConfigureAwait(false);
    }
}
