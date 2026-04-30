using System.Text.Json;

namespace ErrorOr;

public readonly partial record struct ErrorOr<TValue>
{
    /// <inheritdoc/>
    public string GetRecording() => IsError
        ? JsonSerializer.Serialize(Errors, RecordableDefaults.JsonOptions)
        : JsonSerializer.Serialize(Value, RecordableDefaults.JsonOptions);
}
