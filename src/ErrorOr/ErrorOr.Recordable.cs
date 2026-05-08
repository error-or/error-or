namespace ErrorOr;

public readonly partial record struct ErrorOr<TValue>
{
    /// <inheritdoc/>
    public TOutput GetRecording<TOutput>(IRecordingSerializer<TOutput> serializer)
    {
        if (serializer is null)
        {
            throw new ArgumentNullException(nameof(serializer));
        }

        return IsError ? serializer.SerializeErrors(Errors) : serializer.SerializeValue(Value);
    }
}
