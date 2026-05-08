using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;

namespace ErrorOr.AspNetCore.Http;

public static class ErrorOrHttpExtensions
{
    extension(Error error)
    {
        /// <summary>
        /// Converts an <see cref="Error"/> to an <see cref="IResult"/> suitable for Minimal API responses.
        /// </summary>
        /// <param name="options">Optional configuration. If null, uses default mapping.</param>
        /// <returns>A problem <see cref="IResult"/>.</returns>
        public IResult ToResult(ErrorOrAspNetCoreOptions? options = null)
            => TypedResults.Problem(error.ToProblemDetails(options));

        /// <summary>
        /// Converts an <see cref="Error"/> to an <see cref="IResult"/>, using the provided
        /// <see cref="IOptions{TOptions}"/>. Prefer this overload in Minimal API handlers where
        /// <see cref="IOptions{TOptions}"/> can be injected directly as a route parameter.
        /// </summary>
        /// <param name="options">The registered options instance.</param>
        /// <returns>A problem <see cref="IResult"/>.</returns>
        public IResult ToResult(IOptions<ErrorOrAspNetCoreOptions> options) => error.ToResult(options.Value);

        /// <summary>
        /// Converts an <see cref="Error"/> to an <see cref="IResult"/>, using the provided
        /// <see cref="IOptionsSnapshot{TOptions}"/>. Use this overload when options may change between
        /// requests (e.g. appsettings.json with <c>reloadOnChange: true</c>).
        /// </summary>
        /// <param name="options">The scoped options snapshot.</param>
        /// <returns>A problem <see cref="IResult"/>.</returns>
        public IResult ToResult(IOptionsSnapshot<ErrorOrAspNetCoreOptions> options) => error.ToResult(options.Value);

        /// <summary>
        /// Converts an <see cref="Error"/> to an <see cref="IResult"/>, using the provided
        /// <see cref="IOptionsMonitor{TOptions}"/>. Use this overload when options may change at runtime
        /// and need to be observed immediately, or when injecting into a singleton.
        /// </summary>
        /// <param name="monitor">The options monitor.</param>
        /// <returns>A problem <see cref="IResult"/>.</returns>
        public IResult ToResult(IOptionsMonitor<ErrorOrAspNetCoreOptions> monitor)
            => error.ToResult(monitor.CurrentValue);
    }

    extension(List<Error> errors)
    {
        /// <summary>
        /// Converts a list of <see cref="Error"/> objects to an <see cref="IResult"/> suitable for Minimal API responses.
        /// When all errors are validation errors, a validation problem result is returned.
        /// </summary>
        /// <param name="options">Optional configuration. If null, uses default mapping.</param>
        /// <returns>A problem <see cref="IResult"/>.</returns>
        public IResult ToResult(ErrorOrAspNetCoreOptions? options = null)
        {
            var problemDetails = errors.ToProblemDetails(options);

            if (problemDetails is HttpValidationProblemDetails validationProblem)
            {
                return TypedResults.ValidationProblem(
                    validationProblem.Errors,
                    type: validationProblem.Type,
                    title: validationProblem.Title,
                    detail: validationProblem.Detail,
                    instance: validationProblem.Instance,
                    extensions: validationProblem.Extensions.Count > 0 ? validationProblem.Extensions : null);
            }

            return TypedResults.Problem(problemDetails);
        }

        /// <summary>
        /// Converts a list of <see cref="Error"/> objects to an <see cref="IResult"/>, using the provided
        /// <see cref="IOptions{TOptions}"/>. Prefer this overload in Minimal API handlers where
        /// <see cref="IOptions{TOptions}"/> can be injected directly as a route parameter.
        /// </summary>
        /// <param name="options">The registered options instance.</param>
        /// <returns>A problem <see cref="IResult"/>.</returns>
        public IResult ToResult(IOptions<ErrorOrAspNetCoreOptions> options) => errors.ToResult(options.Value);

        /// <summary>
        /// Converts a list of <see cref="Error"/> objects to an <see cref="IResult"/>, using the provided
        /// <see cref="IOptionsSnapshot{TOptions}"/>. Use this overload when options may change between
        /// requests (e.g. appsettings.json with <c>reloadOnChange: true</c>).
        /// </summary>
        /// <param name="options">The scoped options snapshot.</param>
        /// <returns>A problem <see cref="IResult"/>.</returns>
        public IResult ToResult(IOptionsSnapshot<ErrorOrAspNetCoreOptions> options) => errors.ToResult(options.Value);

        /// <summary>
        /// Converts a list of <see cref="Error"/> objects to an <see cref="IResult"/>, using the provided
        /// <see cref="IOptionsMonitor{TOptions}"/>. Use this overload when options may change at runtime
        /// and need to be observed immediately, or when injecting into a singleton.
        /// </summary>
        /// <param name="monitor">The options monitor.</param>
        /// <returns>A problem <see cref="IResult"/>.</returns>
        public IResult ToResult(IOptionsMonitor<ErrorOrAspNetCoreOptions> monitor) =>
            errors.ToResult(monitor.CurrentValue);
    }
}
