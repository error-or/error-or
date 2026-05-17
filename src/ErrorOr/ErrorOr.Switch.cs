namespace ErrorOr;

public readonly partial record struct ErrorOr<TValue> : IErrorOr<TValue>
{
    /// <summary>
    /// Executes the appropriate action based on the state of the <see cref="ErrorOr{TValue}"/>.
    /// If the state is an error, the provided action <paramref name="onError"/> is executed.
    /// If the state is a value, the provided action <paramref name="onValue"/> is executed.
    /// </summary>
    /// <param name="onValue">The action to execute if the state is a value.</param>
    /// <param name="onError">The action to execute if the state is an error.</param>
    public void Switch(Action<TValue> onValue, Action<ReadOnlyCollection<Error>> onError)
    {
        if (IsError)
        {
            onError(Errors);
            return;
        }

        onValue(Value);
    }

    /// <summary>
    /// Asynchronously executes the appropriate action based on the state of the <see cref="ErrorOr{TValue}"/>.
    /// If the state is an error, the provided action <paramref name="onError"/> is executed asynchronously.
    /// If the state is a value, the provided action <paramref name="onValue"/> is executed asynchronously.
    /// </summary>
    /// <param name="onValue">The asynchronous action to execute if the state is a value.</param>
    /// <param name="onError">The asynchronous action to execute if the state is an error.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public async Task SwitchAsync(Func<TValue, Task> onValue, Func<ReadOnlyCollection<Error>, Task> onError)
    {
        if (IsError)
        {
            await onError(Errors).ConfigureAwait(false);
            return;
        }

        await onValue(Value).ConfigureAwait(false);
    }

    /// <summary>
    /// Executes the appropriate action based on the state of the <see cref="ErrorOr{TValue}"/>.
    /// If the state is an error, the provided action <paramref name="onFirstError"/> is executed using the first error as input.
    /// If the state is a value, the provided action <paramref name="onValue"/> is executed.
    /// </summary>
    /// <param name="onValue">The action to execute if the state is a value.</param>
    /// <param name="onFirstError">The action to execute with the first error if the state is an error.</param>
    public void SwitchFirst(Action<TValue> onValue, Action<Error> onFirstError)
    {
        if (IsError)
        {
            onFirstError(FirstError);
            return;
        }

        onValue(Value);
    }

    /// <summary>
    /// Asynchronously executes the appropriate action based on the state of the <see cref="ErrorOr{TValue}"/>.
    /// If the state is an error, the provided action <paramref name="onFirstError"/> is executed asynchronously using the first error as input.
    /// If the state is a value, the provided action <paramref name="onValue"/> is executed asynchronously.
    /// </summary>
    /// <param name="onValue">The asynchronous action to execute if the state is a value.</param>
    /// <param name="onFirstError">The asynchronous action to execute with the first error if the state is an error.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public async Task SwitchFirstAsync(Func<TValue, Task> onValue, Func<Error, Task> onFirstError)
    {
        if (IsError)
        {
            await onFirstError(FirstError).ConfigureAwait(false);
            return;
        }

        await onValue(Value).ConfigureAwait(false);
    }
}
