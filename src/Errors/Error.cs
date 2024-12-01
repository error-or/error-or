using InnerMetadata = System.Collections.Generic.Dictionary<string, object>;

namespace ErrorOr;
/// <summary>
/// Represents an error.
/// </summary>
public readonly record struct Error
{
    private readonly InnerMetadata? _innerMetadata;

    private Error(string code, string description, ErrorType type, IDictionary<string, object>? metadata)
    {
        Code = code;
        Description = description;
        Type = type;
        NumericType = (int)type;
        if (metadata is not null)
        {
            _innerMetadata = new InnerMetadata(metadata);
            Metadata = new ReadOnlyDictionary<string, object>(_innerMetadata);
        }
    }

    /// <summary>
    /// Gets the unique error code.
    /// </summary>
    public string Code { get; }

    /// <summary>
    /// Gets the error description.
    /// </summary>
    public string Description { get; }

    /// <summary>
    /// Gets the error type.
    /// </summary>
    public ErrorType Type { get; }

    /// <summary>
    /// Gets the numeric value of the type.
    /// </summary>
    public int NumericType { get; }

    /// <summary>
    /// Gets the metadata.
    /// </summary>
    public ReadOnlyDictionary<string, object>? Metadata { get; }

    /// <summary>
    /// Creates an <see cref="Error"/> of type <see cref="ErrorType.Failure"/> from a code and description.
    /// </summary>
    /// <param name="code">The unique error code.</param>
    /// <param name="description">The error description.</param>
    /// <param name="metadata">A dictionary which provides optional space for information.</param>
    public static Error Failure(
        string code = "General.Failure",
        string description = "A failure has occurred.",
        IDictionary<string, object>? metadata = null) =>
            new(code, description, ErrorType.Failure, metadata);

    /// <summary>
    /// Creates an <see cref="Error"/> of type <see cref="ErrorType.Unexpected"/> from a code and description.
    /// </summary>
    /// <param name="code">The unique error code.</param>
    /// <param name="description">The error description.</param>
    /// <param name="metadata">A dictionary which provides optional space for information.</param>
    public static Error Unexpected(
        string code = "General.Unexpected",
        string description = "An unexpected error has occurred.",
        IDictionary<string, object>? metadata = null) =>
            new(code, description, ErrorType.Unexpected, metadata);

    /// <summary>
    /// Creates an <see cref="Error"/> of type <see cref="ErrorType.Validation"/> from a code and description.
    /// </summary>
    /// <param name="code">The unique error code.</param>
    /// <param name="description">The error description.</param>
    /// <param name="metadata">A dictionary which provides optional space for information.</param>
    public static Error Validation(
        string code = "General.Validation",
        string description = "A validation error has occurred.",
        IDictionary<string, object>? metadata = null) =>
            new(code, description, ErrorType.Validation, metadata);

    /// <summary>
    /// Creates an <see cref="Error"/> of type <see cref="ErrorType.Conflict"/> from a code and description.
    /// </summary>
    /// <param name="code">The unique error code.</param>
    /// <param name="description">The error description.</param>
    /// <param name="metadata">A dictionary which provides optional space for information.</param>
    public static Error Conflict(
        string code = "General.Conflict",
        string description = "A conflict error has occurred.",
        IDictionary<string, object>? metadata = null) =>
            new(code, description, ErrorType.Conflict, metadata);

    /// <summary>
    /// Creates an <see cref="Error"/> of type <see cref="ErrorType.NotFound"/> from a code and description.
    /// </summary>
    /// <param name="code">The unique error code.</param>
    /// <param name="description">The error description.</param>
    /// <param name="metadata">A dictionary which provides optional space for information.</param>
    public static Error NotFound(
        string code = "General.NotFound",
        string description = "A 'Not Found' error has occurred.",
        IDictionary<string, object>? metadata = null) =>
            new(code, description, ErrorType.NotFound, metadata);

    /// <summary>
    /// Creates an <see cref="Error"/> of type <see cref="ErrorType.Unauthorized"/> from a code and description.
    /// </summary>
    /// <param name="code">The unique error code.</param>
    /// <param name="description">The error description.</param>
    /// <param name="metadata">A dictionary which provides optional space for information.</param>
    public static Error Unauthorized(
        string code = "General.Unauthorized",
        string description = "An 'Unauthorized' error has occurred.",
        IDictionary<string, object>? metadata = null) =>
            new(code, description, ErrorType.Unauthorized, metadata);

    /// <summary>
    /// Creates an <see cref="Error"/> of type <see cref="ErrorType.Forbidden"/> from a code and description.
    /// </summary>
    /// <param name="code">The unique error code.</param>
    /// <param name="description">The error description.</param>
    /// <param name="metadata">A dictionary which provides optional space for information.</param>
    public static Error Forbidden(
        string code = "General.Forbidden",
        string description = "A 'Forbidden' error has occurred.",
        IDictionary<string, object>? metadata = null) =>
        new(code, description, ErrorType.Forbidden, metadata);

    /// <summary>
    /// Creates an <see cref="Error"/> with the given numeric <paramref name="type"/>,
    /// <paramref name="code"/>, and <paramref name="description"/>.
    /// </summary>
    /// <param name="type">An integer value which represents the type of error that occurred.</param>
    /// <param name="code">The unique error code.</param>
    /// <param name="description">The error description.</param>
    /// <param name="metadata">A dictionary which provides optional space for information.</param>
    public static Error Custom(
        int type,
        string code,
        string description,
        IDictionary<string, object>? metadata = null) =>
            new(code, description, (ErrorType)type, metadata);

    public bool Equals(Error other)
    {
        if (Type != other.Type ||
            NumericType != other.NumericType ||
            Code != other.Code ||
            Description != other.Description)
        {
            return false;
        }

        if (Metadata is null)
        {
            return other.Metadata is null;
        }

        return other.Metadata is not null && CompareMetadata(_innerMetadata!, other._innerMetadata!);
    }

    public override int GetHashCode() =>
        _innerMetadata is null ? HashCode.Combine(Code, Description, Type, NumericType) : ComposeHashCode();

    private int ComposeHashCode()
    {
#pragma warning disable SA1129 // HashCode needs to be instantiated this way
        var hashCode = new HashCode();
#pragma warning restore SA1129

        hashCode.Add(Code);
        hashCode.Add(Description);
        hashCode.Add(Type);
        hashCode.Add(NumericType);

        foreach (var keyValuePair in _innerMetadata!)
        {
            hashCode.Add(keyValuePair.Key);
            hashCode.Add(keyValuePair.Value);
        }

        return hashCode.ToHashCode();
    }

    private static bool CompareMetadata(InnerMetadata metadata, InnerMetadata otherMetadata)
    {
        if (ReferenceEquals(metadata, otherMetadata))
        {
            return true;
        }

        if (metadata.Count != otherMetadata.Count)
        {
            return false;
        }

        foreach (var keyValuePair in metadata)
        {
            if (!otherMetadata.TryGetValue(keyValuePair.Key, out var otherValue) ||
                !keyValuePair.Value.Equals(otherValue))
            {
                return false;
            }
        }

        return true;
    }
}
