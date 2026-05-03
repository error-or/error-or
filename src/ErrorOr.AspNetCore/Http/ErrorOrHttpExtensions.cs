using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace ErrorOr.AspNetCore.Http;

public static class ErrorOrHttpExtensions
{
    /// <summary>
    /// Converts an <see cref="Error"/> to an <see cref="IResult"/> suitable for Minimal API responses.
    /// </summary>
    /// <param name="error">The error to convert.</param>
    /// <param name="options">Optional configuration. If null, uses default mapping.</param>
    /// <returns>A problem <see cref="IResult"/>.</returns>
    public static IResult ToResult(this Error error, ErrorOrAspNetCoreOptions? options = null)
    {
        var pd = error.ToProblemDetails(options);
        return TypedResults.Problem(pd);
    }

    /// <summary>
    /// Converts an <see cref="Error"/> to an <see cref="IResult"/>, resolving <see cref="ErrorOrAspNetCoreOptions"/>
    /// from the provided <see cref="IServiceProvider"/>.
    /// </summary>
    /// <param name="error">The error to convert.</param>
    /// <param name="services">The service provider used to resolve registered options.</param>
    /// <returns>A problem <see cref="IResult"/>.</returns>
    public static IResult ToResult(this Error error, IServiceProvider services)
        => error.ToResult(services.GetService<IOptions<ErrorOrAspNetCoreOptions>>()?.Value);

    /// <summary>
    /// Converts a list of <see cref="Error"/> objects to an <see cref="IResult"/> suitable for Minimal API responses.
    /// When all errors are validation errors, a validation problem result is returned.
    /// </summary>
    /// <param name="errors">The errors to convert.</param>
    /// <param name="options">Optional configuration. If null, uses default mapping.</param>
    /// <returns>A problem <see cref="IResult"/>.</returns>
    public static IResult ToResult(this List<Error> errors, ErrorOrAspNetCoreOptions? options = null)
    {
        var pd = errors.ToProblemDetails(options);

        if (pd is HttpValidationProblemDetails vpd)
        {
            return TypedResults.ValidationProblem(
                vpd.Errors,
                detail: vpd.Detail,
                instance: vpd.Instance,
                title: vpd.Title,
                type: vpd.Type,
                extensions: vpd.Extensions.Count > 0 ? vpd.Extensions : null);
        }

        return TypedResults.Problem(pd);
    }

    /// <summary>
    /// Converts a list of <see cref="Error"/> objects to an <see cref="IResult"/>, resolving
    /// <see cref="ErrorOrAspNetCoreOptions"/> from the provided <see cref="IServiceProvider"/>.
    /// </summary>
    /// <param name="errors">The errors to convert.</param>
    /// <param name="services">The service provider used to resolve registered options.</param>
    /// <returns>A problem <see cref="IResult"/>.</returns>
    public static IResult ToResult(this List<Error> errors, IServiceProvider services)
        => errors.ToResult(services.GetService<IOptions<ErrorOrAspNetCoreOptions>>()?.Value);
}
