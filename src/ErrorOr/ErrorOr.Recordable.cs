namespace ErrorOr;

public readonly partial record struct ErrorOr<TValue>
{
    /// <summary>
    /// Returns a string representation of the current state using the provided <paramref name="recorder"/> function.
    /// </summary>
    /// <param name="recorder">
    /// A function that receives the current <see cref="ErrorOr{TValue}"/> instance and returns a string representation.
    /// </param>
    /// <returns>The string produced by <paramref name="recorder"/>.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="recorder"/> is <see langword="null"/>.</exception>
    /// <remarks>
    /// Use this overload to produce recordings in any format without taking a dependency on
    /// <c>System.Text.Json</c>. The caller can inspect <see cref="ErrorOr{TValue}.IsError"/>,
    /// <see cref="ErrorOr{TValue}.Value"/>, and <see cref="ErrorOr{TValue}.Errors"/> within the function.
    /// </remarks>
    public string GetRecording(Func<ErrorOr<TValue>, string> recorder)
    {
        if (recorder is null)
        {
            throw new ArgumentNullException(nameof(recorder));
        }

        return recorder(this);
    }

    /// <summary>
    /// Returns a string representation of the current state by calling the appropriate delegate
    /// based on whether the instance is in a value or error state.
    /// </summary>
    /// <param name="onValue">A function that receives the value and returns a string representation.</param>
    /// <param name="onError">A function that receives the list of errors and returns a string representation.</param>
    /// <returns>The string produced by <paramref name="onValue"/> or <paramref name="onError"/>.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="onValue"/> or <paramref name="onError"/> is <see langword="null"/>.</exception>
    /// <remarks>
    /// Use this overload to produce recordings in any format without taking a dependency on
    /// <c>System.Text.Json</c>. Mirrors the <c>Match</c> pattern — one delegate per state.
    /// </remarks>
    public string GetRecording(Func<TValue, string> onValue, Func<List<Error>, string> onError)
    {
        if (onValue is null)
        {
            throw new ArgumentNullException(nameof(onValue));
        }

        if (onError is null)
        {
            throw new ArgumentNullException(nameof(onError));
        }

        return IsError ? onError(Errors!) : onValue(Value);
    }

    /// <inheritdoc/>
    public string GetRecording(IRecordingSerializer serializer)
    {
        if (serializer is null)
        {
            throw new ArgumentNullException(nameof(serializer));
        }

        return IsError ? serializer.SerializeErrors(Errors!) : serializer.SerializeValue(Value);
    }
}
