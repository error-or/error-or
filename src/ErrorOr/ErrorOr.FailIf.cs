namespace ErrorOr;

public readonly partial record struct ErrorOr<TValue> : IErrorOr<TValue>
{
    /// <summary>
    /// If the state is value, the provided function <paramref name="onValue"/> is invoked.
    /// If <paramref name="onValue"/> returns true, the given <paramref name="error"/> will be returned, and the state will be error.
    /// </summary>
    /// <param name="onValue">The function to execute if the state is value.</param>
    /// <param name="error">The <see cref="Error"/> to return if the given <paramref name="onValue"/> function returned true.</param>
    /// <returns>The given <paramref name="error"/> if <paramref name="onValue"/> returns true; otherwise, the original <see cref="ErrorOr"/> instance.</returns>
    public ErrorOr<TValue> FailIf(Func<TValue, bool> onValue, Error error)
    {
        if (IsError)
        {
            return this;
        }

        return onValue(Value) ? error : this;
    }

    /// <summary>
    /// If the state is value, the provided function <paramref name="onValue"/> is invoked.
    /// If <paramref name="onValue"/> returns true, the given <paramref name="errorBuilder"/> function will be executed, and the state will be error.
    /// </summary>
    /// <param name="onValue">The function to execute if the state is value.</param>
    /// <param name="errorBuilder">The error builder function to execute and return if the given <paramref name="onValue"/> function returned true.</param>
    /// <returns>The given <paramref name="errorBuilder"/> functions return value if <paramref name="onValue"/> returns true; otherwise, the original <see cref="ErrorOr"/> instance.</returns>
    public ErrorOr<TValue> FailIf(Func<TValue, bool> onValue, Func<TValue, Error> errorBuilder)
    {
        if (IsError)
        {
            return this;
        }

        return onValue(Value) ? errorBuilder(Value) : this;
    }

    /// <summary>
    /// If the state is value, the provided function <paramref name="onValue"/> is invoked asynchronously.
    /// If <paramref name="onValue"/> returns true, the given <paramref name="error"/> will be returned, and the state will be error.
    /// </summary>
    /// <param name="onValue">The function to execute if the statement is value.</param>
    /// <param name="error">The <see cref="Error"/> to return if the given <paramref name="onValue"/> function returned true.</param>
    /// <returns>The given <paramref name="error"/> if <paramref name="onValue"/> returns true; otherwise, the original <see cref="ErrorOr"/> instance.</returns>
    public async Task<ErrorOr<TValue>> FailIfAsync(Func<TValue, Task<bool>> onValue, Error error)
    {
        if (IsError)
        {
            return this;
        }

        return await onValue(Value).ConfigureAwait(false) ? error : this;
    }

    /// <summary>
    /// If the state is value, the provided function <paramref name="onValue"/> is invoked.
    /// If <paramref name="onValue"/> returns true, the given <paramref name="errorBuilder"/> function will be executed, and the state will be error.
    /// </summary>
    /// <param name="onValue">The function to execute if the state is value.</param>
    /// <param name="errorBuilder">The error builder function to execute and return if the given <paramref name="onValue"/> function returned true.</param>
    /// <returns>The given <paramref name="errorBuilder"/> functions return value if <paramref name="onValue"/> returns true; otherwise, the original <see cref="ErrorOr"/> instance.</returns>
    public async Task<ErrorOr<TValue>> FailIfAsync(Func<TValue, Task<bool>> onValue, Func<TValue, Task<Error>> errorBuilder)
    {
        if (IsError)
        {
            return this;
        }

        return await onValue(Value).ConfigureAwait(false) ? await errorBuilder(Value).ConfigureAwait(false) : this;
    }

    /// <summary>
    /// If the state is value, the provided function <paramref name="onValue"/> is invoked.
    /// If <paramref name="onValue"/> returns true, the given <paramref name="error"/> will be returned, and the state will be error.
    /// </summary>
    /// <param name="onValue">The function to execute if the state is value.</param>
    /// <param name="error">The <see cref="Error"/> to return if the given <paramref name="onValue"/> function returned true.</param>
    /// <param name="metadata">Additional metadata to include with the error.</param>
    /// <returns>The given <paramref name="error"/> with metadata if <paramref name="onValue"/> returns true; otherwise, the original <see cref="ErrorOr"/> instance.</returns>
    public ErrorOr<TValue> FailIfWithMetadata(Func<TValue, bool> onValue, Error error, Dictionary<string, object> metadata)
    {
        if (IsError)
        {
            return this;
        }

        if (onValue(Value))
        {
            var enhancedError = error with { Metadata = metadata };
            return enhancedError;
        }

        return this;
    }

    /// <summary>
    /// If the state is value, the provided function <paramref name="onValue"/> is invoked.
    /// If <paramref name="onValue"/> returns true, the given <paramref name="errorBuilder"/> function will be executed, and the state will be error.
    /// </summary>
    /// <param name="onValue">The function to execute if the state is value.</param>
    /// <param name="errorBuilder">The error builder function to execute and return if the given <paramref name="onValue"/> function returned true.</param>
    /// <param name="metadata">Additional metadata to include with the error.</param>
    /// <returns>The given <paramref name="errorBuilder"/> functions return value with metadata if <paramref name="onValue"/> returns true; otherwise, the original <see cref="ErrorOr"/> instance.</returns>
    public ErrorOr<TValue> FailIfWithMetadata(Func<TValue, bool> onValue, Func<TValue, Error> errorBuilder, Dictionary<string, object> metadata)
    {
        if (IsError)
        {
            return this;
        }

        if (onValue(Value))
        {
            var error = errorBuilder(Value);
            var enhancedError = error with { Metadata = metadata };
            return enhancedError;
        }

        return this;
    }

    /// <summary>
    /// If the state is value, the provided function <paramref name="onValue"/> is invoked asynchronously.
    /// If <paramref name="onValue"/> returns true, the given <paramref name="error"/> will be returned, and the state will be error.
    /// </summary>
    /// <param name="onValue">The function to execute if the statement is value.</param>
    /// <param name="error">The <see cref="Error"/> to return if the given <paramref name="onValue"/> function returned true.</param>
    /// <param name="metadata">Additional metadata to include with the error.</param>
    /// <returns>The given <paramref name="error"/> with metadata if <paramref name="onValue"/> returns true; otherwise, the original <see cref="ErrorOr"/> instance.</returns>
    public async Task<ErrorOr<TValue>> FailIfWithMetadataAsync(Func<TValue, Task<bool>> onValue, Error error, Dictionary<string, object> metadata)
    {
        if (IsError)
        {
            return this;
        }

        if (await onValue(Value).ConfigureAwait(false))
        {
            var enhancedError = error with { Metadata = metadata };
            return enhancedError;
        }

        return this;
    }

    /// <summary>
    /// If the state is value, the provided function <paramref name="onValue"/> is invoked asynchronously.
    /// If <paramref name="onValue"/> returns true, the given <paramref name="errorBuilder"/> function will be executed, and the state will be error.
    /// </summary>
    /// <param name="onValue">The function to execute if the state is value.</param>
    /// <param name="errorBuilder">The error builder function to execute and return if the given <paramref name="onValue"/> function returned true.</param>
    /// <param name="metadata">Additional metadata to include with the error.</param>
    /// <returns>The given <paramref name="errorBuilder"/> functions return value with metadata if <paramref name="onValue"/> returns true; otherwise, the original <see cref="ErrorOr"/> instance.</returns>
    public async Task<ErrorOr<TValue>> FailIfWithMetadataAsync(Func<TValue, Task<bool>> onValue, Func<TValue, Task<Error>> errorBuilder, Dictionary<string, object> metadata)
    {
        if (IsError)
        {
            return this;
        }

        if (await onValue(Value).ConfigureAwait(false))
        {
            var error = await errorBuilder(Value).ConfigureAwait(false);
            var enhancedError = error with { Metadata = metadata };
            return enhancedError;
        }

        return this;
    }
}
