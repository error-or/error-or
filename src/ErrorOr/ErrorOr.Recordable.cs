using System.Text.Json;
using System.Text.Json.Serialization;

namespace ErrorOr;

public readonly partial record struct ErrorOr<TValue>
{
    /// <inheritdoc/>
    public string GetRecording() => GetRecording(RecordableDefaults.JsonOptions);

    /// <inheritdoc/>
    public string GetRecording(JsonSerializerOptions options)
    {
        if (IsError)
        {
            return JsonSerializer.Serialize(Errors, options);
        }

        return JsonSerializer.Serialize(Value, options);
    }
}

internal static class RecordableDefaults
{
    internal static readonly JsonSerializerOptions JsonOptions = new()
    {
        WriteIndented = true,
        Converters = { new JsonStringEnumConverter() },
        DefaultIgnoreCondition = JsonIgnoreCondition.Never,
    };
}
