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
    public void Switch(Action<TValue> onValue, Action<List<Error>> onError)
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
    public async Task SwitchAsync(Func<TValue, Task> onValue, Func<List<Error>, Task> onError)
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

    /// <summary>
    /// Executes the appropriate action based on the state of the <see cref="ErrorOr{TValue}"/>.
    /// If the state is an error, the provided action <paramref name="onError"/> is executed with metadata.
    /// If the state is a value, the provided action <paramref name="onValue"/> is executed.
    /// </summary>
    /// <param name="onValue">The action to execute if the state is a value.</param>
    /// <param name="onError">The action to execute if the state is an error.</param>
    /// <param name="metadata">Additional metadata to include with the errors.</param>
    public void SwitchWithMetadata(Action<TValue> onValue, Action<List<Error>> onError, Dictionary<string, object> metadata)
    {
        if (IsError)
        {
            var enhancedErrors = Errors.Select(e => e with { Metadata = metadata }).ToList();
            onError(enhancedErrors);
            return;
        }

        onValue(Value);
    }

    /// <summary>
    /// Asynchronously executes the appropriate action based on the state of the <see cref="ErrorOr{TValue}"/>.
    /// If the state is an error, the provided action <paramref name="onError"/> is executed asynchronously with metadata.
    /// If the state is a value, the provided action <paramref name="onValue"/> is executed asynchronously.
    /// </summary>
    /// <param name="onValue">The asynchronous action to execute if the state is a value.</param>
    /// <param name="onError">The asynchronous action to execute if the state is an error.</param>
    /// <param name="metadata">Additional metadata to include with the errors.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public async Task SwitchWithMetadataAsync(Func<TValue, Task> onValue, Func<List<Error>, Task> onError, Dictionary<string, object> metadata)
    {
        if (IsError)
        {
            var enhancedErrors = Errors.Select(e => e with { Metadata = metadata }).ToList();
            await onError(enhancedErrors).ConfigureAwait(false);
            return;
        }

        await onValue(Value).ConfigureAwait(false);
    }

    /// <summary>
    /// Executes the appropriate action based on the state of the <see cref="ErrorOr{TValue}"/>.
    /// If the state is an error, the provided action <paramref name="onFirstError"/> is executed with metadata using the first error as input.
    /// If the state is a value, the provided action <paramref name="onValue"/> is executed.
    /// </summary>
    /// <param name="onValue">The action to execute if the state is a value.</param>
    /// <param name="onFirstError">The action to execute with the first error if the state is an error.</param>
    /// <param name="metadata">Additional metadata to include with the errors.</param>
    public void SwitchFirstWithMetadata(Action<TValue> onValue, Action<Error> onFirstError, Dictionary<string, object> metadata)
    {
        if (IsError)
        {
            var enhancedError = FirstError with { Metadata = metadata };
            onFirstError(enhancedError);
            return;
        }

        onValue(Value);
    }

    /// <summary>
    /// Asynchronously executes the appropriate action based on the state of the <see cref="ErrorOr{TValue}"/>.
    /// If the state is an error, the provided action <paramref name="onFirstError"/> is executed asynchronously with metadata using the first error as input.
    /// If the state is a value, the provided action <paramref name="onValue"/> is executed asynchronously.
    /// </summary>
    /// <param name="onValue">The asynchronous action to execute if the state is a value.</param>
    /// <param name="onFirstError">The asynchronous action to execute with the first error if the state is an error.</param>
    /// <param name="metadata">Additional metadata to include with the errors.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public async Task SwitchFirstWithMetadataAsync(Func<TValue, Task> onValue, Func<Error, Task> onFirstError, Dictionary<string, object> metadata)
    {
        if (IsError)
        {
            var enhancedError = FirstError with { Metadata = metadata };
            await onFirstError(enhancedError).ConfigureAwait(false);
            return;
        }

        await onValue(Value).ConfigureAwait(false);
    }
}
