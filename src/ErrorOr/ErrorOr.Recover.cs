namespace ErrorOr;

public readonly partial record struct ErrorOr<TValue> : IErrorOr<TValue>
{
    /// <summary>
    /// If the state is error, the provided function <paramref name="onError"/> is executed and its result is returned.
    /// </summary>
    /// <param name="onError">The function to execute if the state is error.</param>
    /// <returns>The result from calling <paramref name="onError"/> if state is error; otherwise the original <see cref="Value"/>.</returns>
    public ErrorOr<TValue> Recover(Func<List<Error>, ErrorOr<TValue>> onError)
    {
        if (!IsError)
        {
            return Value;
        }

        return onError(Errors);
    }

    /// <summary>
    /// If the state is error, the provided function <paramref name="onError"/> is executed asynchronously and its result is returned.
    /// </summary>
    /// <param name="onError">The function to execute if the state is error.</param>
    /// <returns>The result from calling <paramref name="onError"/> if state is error; otherwise the original <see cref="Value"/>.</returns>
    public async Task<ErrorOr<TValue>> RecoverAsync(Func<List<Error>, Task<ErrorOr<TValue>>> onError)
    {
        if (!IsError)
        {
            return Value;
        }

        return await onError(Errors).ConfigureAwait(false);
    }
}
