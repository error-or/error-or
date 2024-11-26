namespace ErrorOr;

public readonly partial record struct ErrorOr<TValue> : IErrorOr<TValue>
{
    /// <summary>
    /// If the state is error, the provided function <paramref name="onError"/> is executed and its result is returned.
    /// </summary>
    /// <param name="onError">The function to execute if the state is error.</param>
    /// <returns>The result from calling <paramref name="onError"/> if state is error; otherwise the original <see cref="Value"/>.</returns>
    public ErrorOr<TValue> Else(Func<List<Error>, Error> onError)
    {
        if (!IsError)
        {
            return Value;
        }

        return onError(Errors);
    }

    /// <summary>
    /// If the state is error, the provided function <paramref name="onError"/> is executed and its result is returned.
    /// </summary>
    /// <param name="onError">The function to execute if the state is error.</param>
    /// <returns>The result from calling <paramref name="onError"/> if state is error; otherwise the original <see cref="Value"/>.</returns>
    public ErrorOr<TValue> Else(Func<List<Error>, List<Error>> onError)
    {
        if (!IsError)
        {
            return Value;
        }

        return onError(Errors);
    }

    /// <summary>
    /// If the state is error, the provided <paramref name="error"/> is returned.
    /// </summary>
    /// <param name="error">The error to return.</param>
    /// <returns>The given <paramref name="error"/>.</returns>
    public ErrorOr<TValue> Else(Error error)
    {
        if (!IsError)
        {
            return Value;
        }

        return error;
    }

    /// <summary>
    /// If the state is error, the provided function <paramref name="onError"/> is executed and its result is returned.
    /// </summary>
    /// <param name="onError">The function to execute if the state is error.</param>
    /// <returns>The result from calling <paramref name="onError"/> if state is error; otherwise the original <see cref="Value"/>.</returns>
    public ErrorOr<TValue> Else(Func<List<Error>, TValue> onError)
    {
        if (!IsError)
        {
            return Value;
        }

        return onError(Errors);
    }

    /// <summary>
    /// If the state is error, the provided function <paramref name="onError"/> is executed and its result is returned.
    /// </summary>
    /// <param name="onError">The value to return if the state is error.</param>
    /// <returns>The result from calling <paramref name="onError"/> if state is error; otherwise the original <see cref="Value"/>.</returns>
    public ErrorOr<TValue> Else(TValue onError)
    {
        if (!IsError)
        {
            return Value;
        }

        return onError;
    }

    /// <summary>
    /// If the state is error, the provided function <paramref name="onError"/> is executed asynchronously and its result is returned.
    /// </summary>
    /// <param name="onError">The function to execute if the state is error.</param>
    /// <returns>The result from calling <paramref name="onError"/> if state is error; otherwise the original <see cref="Value"/>.</returns>
    public async Task<ErrorOr<TValue>> ElseAsync(Func<List<Error>, Task<TValue>> onError)
    {
        if (!IsError)
        {
            return Value;
        }

        return await onError(Errors).ConfigureAwait(false);
    }

    /// <summary>
    /// If the state is error, the provided function <paramref name="onError"/> is executed asynchronously and its result is returned.
    /// </summary>
    /// <param name="onError">The function to execute if the state is error.</param>
    /// <returns>The result from calling <paramref name="onError"/> if state is error; otherwise the original <see cref="Value"/>.</returns>
    public async Task<ErrorOr<TValue>> ElseAsync(Func<List<Error>, Task<Error>> onError)
    {
        if (!IsError)
        {
            return Value;
        }

        return await onError(Errors).ConfigureAwait(false);
    }

    /// <summary>
    /// If the state is error, the provided function <paramref name="onError"/> is executed asynchronously and its result is returned.
    /// </summary>
    /// <param name="onError">The function to execute if the state is error.</param>
    /// <returns>The result from calling <paramref name="onError"/> if state is error; otherwise the original <see cref="Value"/>.</returns>
    public async Task<ErrorOr<TValue>> ElseAsync(Func<List<Error>, Task<List<Error>>> onError)
    {
        if (!IsError)
        {
            return Value;
        }

        return await onError(Errors).ConfigureAwait(false);
    }

    /// <summary>
    /// If the state is error, the provided <paramref name="error"/> is awaited and returned.
    /// </summary>
    /// <param name="error">The error to return if the state is error.</param>
    /// <returns>The result from awaiting the given <paramref name="error"/>.</returns>
    public async Task<ErrorOr<TValue>> ElseAsync(Task<Error> error)
    {
        if (!IsError)
        {
            return Value;
        }

        return await error.ConfigureAwait(false);
    }

    /// <summary>
    /// If the state is error, the provided function <paramref name="onError"/> is executed asynchronously and its result is returned.
    /// </summary>
    /// <param name="onError">The function to execute if the state is error.</param>
    /// <returns>The result from calling <paramref name="onError"/> if state is error; otherwise the original <see cref="Value"/>.</returns>
    public async Task<ErrorOr<TValue>> ElseAsync(Task<TValue> onError)
    {
        if (!IsError)
        {
            return Value;
        }

        return await onError.ConfigureAwait(false);
    }

    /// <summary>
    /// If the state is error, the provided function <paramref name="onError"/> is executed and its result is returned.
    /// </summary>
    /// <param name="onError">The function to execute if the state is error.</param>
    /// <param name="metadata">Additional metadata to include with the errors.</param>
    /// <returns>The result from calling <paramref name="onError"/> if state is error; otherwise the original <see cref="Value"/>.</returns>
    public ErrorOr<TValue> ElseWithMetadata(Func<List<Error>, Error> onError, Dictionary<string, object> metadata)
    {
        if (!IsError)
        {
            return Value;
        }

        var enhancedErrors = Errors.Select(e => e with { Metadata = metadata }).ToList();
        return onError(enhancedErrors);
    }

    /// <summary>
    /// If the state is error, the provided function <paramref name="onError"/> is executed and its result is returned.
    /// </summary>
    /// <param name="onError">The function to execute if the state is error.</param>
    /// <param name="metadata">Additional metadata to include with the errors.</param>
    /// <returns>The result from calling <paramref name="onError"/> if state is error; otherwise the original <see cref="Value"/>.</returns>
    public ErrorOr<TValue> ElseWithMetadata(Func<List<Error>, List<Error>> onError, Dictionary<string, object> metadata)
    {
        if (!IsError)
        {
            return Value;
        }

        var enhancedErrors = Errors.Select(e => e with { Metadata = metadata }).ToList();
        return onError(enhancedErrors);
    }

    /// <summary>
    /// If the state is error, the provided function <paramref name="onError"/> is executed asynchronously and its result is returned.
    /// </summary>
    /// <param name="onError">The function to execute if the state is error.</param>
    /// <param name="metadata">Additional metadata to include with the errors.</param>
    /// <returns>The result from calling <paramref name="onError"/> if state is error; otherwise the original <see cref="Value"/>.</returns>
    public async Task<ErrorOr<TValue>> ElseWithMetadataAsync(Func<List<Error>, Task<TValue>> onError, Dictionary<string, object> metadata)
    {
        if (!IsError)
        {
            return Value;
        }

        var enhancedErrors = Errors.Select(e => e with { Metadata = metadata }).ToList();
        return await onError(enhancedErrors).ConfigureAwait(false);
    }

    /// <summary>
    /// If the state is error, the provided function <paramref name="onError"/> is executed asynchronously and its result is returned.
    /// </summary>
    /// <param name="onError">The function to execute if the state is error.</param>
    /// <param name="metadata">Additional metadata to include with the errors.</param>
    /// <returns>The result from calling <paramref name="onError"/> if state is error; otherwise the original <see cref="Value"/>.</returns>
    public async Task<ErrorOr<TValue>> ElseWithMetadataAsync(Func<List<Error>, Task<Error>> onError, Dictionary<string, object> metadata)
    {
        if (!IsError)
        {
            return Value;
        }

        var enhancedErrors = Errors.Select(e => e with { Metadata = metadata }).ToList();
        return await onError(enhancedErrors).ConfigureAwait(false);
    }

    /// <summary>
    /// If the state is error, the provided function <paramref name="onError"/> is executed asynchronously and its result is returned.
    /// </summary>
    /// <param name="onError">The function to execute if the state is error.</param>
    /// <param name="metadata">Additional metadata to include with the errors.</param>
    /// <returns>The result from calling <paramref name="onError"/> if state is error; otherwise the original <see cref="Value"/>.</returns>
    public async Task<ErrorOr<TValue>> ElseWithMetadataAsync(Func<List<Error>, Task<List<Error>>> onError, Dictionary<string, object> metadata)
    {
        if (!IsError)
        {
            return Value;
        }

        var enhancedErrors = Errors.Select(e => e with { Metadata = metadata }).ToList();
        return await onError(enhancedErrors).ConfigureAwait(false);
    }
}
