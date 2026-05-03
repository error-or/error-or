using Microsoft.AspNetCore.Mvc;

namespace ErrorOr.AspNetCore;

/// <summary>
/// Options for customizing ErrorOr ASP.NET Core behavior.
/// </summary>
public sealed class ErrorOrAspNetCoreOptions
{
    /// <summary>
    /// Gets or sets a value indicating whether <see cref="Error.Metadata"/> is included in <see cref="ProblemDetails.Extensions"/>.
    /// Defaults to <see langword="false"/>.
    /// </summary>
    public bool IncludeMetadataInProblemDetails { get; set; } = false;

    /// <summary>
    /// Gets the list of custom problem details mappers.
    /// Each mapper receives the full <see cref="List{T}"/> of <see cref="Error"/> objects and returns a
    /// <see cref="ProblemDetails"/> instance, or <see langword="null"/> to fall through to the next mapper or the default.
    /// The first mapper that returns a non-null value wins.
    /// </summary>
    public List<Func<List<Error>, ProblemDetails?>> ErrorsToProblemDetailsMappers { get; } = [];

    /// <summary>
    /// Gets the list of custom HTTP status code mappers for a single <see cref="Error"/>.
    /// Each mapper returns an <see cref="int"/> status code, or <see langword="null"/> to fall through to the next mapper or the default.
    /// The first mapper that returns a non-null value wins.
    /// </summary>
    public List<Func<Error, int?>> ErrorToStatusCodeMappers { get; } = [];
}
