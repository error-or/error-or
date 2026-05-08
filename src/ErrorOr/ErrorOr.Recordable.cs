namespace ErrorOr;

public readonly partial record struct ErrorOr<TValue>
{
    /// <inheritdoc/>
    public TOutput GetRecording<TOutput>(IRecordingSerializer<TOutput> serializer)
    {
        return IsError ? serializer.SerializeErrors(Errors) : serializer.SerializeValue(Value);
    }
}
