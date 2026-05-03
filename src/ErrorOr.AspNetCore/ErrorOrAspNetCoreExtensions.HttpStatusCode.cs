using Microsoft.AspNetCore.Http;

namespace ErrorOr.AspNetCore;

public static partial class ErrorOrAspNetCoreExtensions
{
    /// <summary>
    /// Converts an <see cref="Error"/> to an HTTP status code based on its <see cref="ErrorType"/>.
    /// Custom mappers in <paramref name="options"/> are checked first; the first non-null result wins.
    /// </summary>
    /// <param name="error">The error to convert.</param>
    /// <param name="options">Optional configuration. If null, uses the default mapping.</param>
    /// <returns>The HTTP status code.</returns>
    public static int ToHttpStatusCode(this Error error, ErrorOrAspNetCoreOptions? options = null)
    {
        if (options is not null)
        {
            foreach (var mapper in options.ErrorToStatusCodeMappers)
            {
                if (mapper(error) is int code)
                {
                    return code;
                }
            }
        }

        return error.Type switch
        {
            ErrorType.Validation => StatusCodes.Status400BadRequest,
            ErrorType.Unauthorized => StatusCodes.Status401Unauthorized,
            ErrorType.Forbidden => StatusCodes.Status403Forbidden,
            ErrorType.NotFound => StatusCodes.Status404NotFound,
            ErrorType.Conflict => StatusCodes.Status409Conflict,
            _ => StatusCodes.Status500InternalServerError,
        };
    }

    /// <summary>
    /// Converts a list of <see cref="Error"/> objects to an HTTP status code.
    /// The status code is determined by the first error in the list.
    /// Returns 500 if the list is empty.
    /// </summary>
    /// <param name="errors">The errors to convert.</param>
    /// <param name="options">Optional configuration.</param>
    /// <returns>The HTTP status code.</returns>
    public static int ToHttpStatusCode(this List<Error> errors, ErrorOrAspNetCoreOptions? options = null)
        => errors.Count == 0 ? StatusCodes.Status500InternalServerError : errors[0].ToHttpStatusCode(options);
}
