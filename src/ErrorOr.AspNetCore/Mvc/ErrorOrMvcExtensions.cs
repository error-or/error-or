using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace ErrorOr.AspNetCore.Mvc;

public static class ErrorOrMvcExtensions
{
    extension(Error error)
    {
        /// <summary>
        /// Converts an <see cref="Error"/> to an <see cref="IActionResult"/> suitable for MVC controller responses.
        /// When an <see cref="HttpContext"/> is provided, the DI-registered <see cref="ProblemDetailsFactory"/> and
        /// <see cref="ErrorOrAspNetCoreOptions"/> are resolved automatically.
        /// </summary>
        /// <param name="httpContext">
        /// The current <see cref="HttpContext"/>. When provided, <see cref="ProblemDetailsFactory"/> is used
        /// to enrich the response (e.g. adding a traceId extension).
        /// </param>
        /// <returns>An <see cref="ObjectResult"/> containing a <see cref="ProblemDetails"/> body.</returns>
        public IActionResult ToActionResult(HttpContext? httpContext = null)
        {
            var options = httpContext?.RequestServices.GetService<IOptions<ErrorOrAspNetCoreOptions>>()?.Value;
            var problemDetails = error.ToProblemDetails(options);

            if (httpContext?.RequestServices.GetService<ProblemDetailsFactory>() is { } factory)
            {
                var factoryPd = factory.CreateProblemDetails(
                    httpContext,
                    problemDetails.Status,
                    problemDetails.Title,
                    detail: problemDetails.Detail);

                CopyMissingExtensions(problemDetails, factoryPd);

                return new ObjectResult(factoryPd) { StatusCode = factoryPd.Status };
            }

            return new ObjectResult(problemDetails) { StatusCode = problemDetails.Status };
        }
    }

    extension(List<Error> errors)
    {
        /// <summary>
        /// Converts a list of <see cref="Error"/> objects to an <see cref="IActionResult"/> suitable for MVC controller responses.
        /// When all errors are validation errors, a <see cref="ValidationProblemDetails"/> result is returned.
        /// When an <see cref="HttpContext"/> is provided, the DI-registered <see cref="ProblemDetailsFactory"/> and
        /// <see cref="ErrorOrAspNetCoreOptions"/> are resolved automatically.
        /// </summary>
        /// <param name="httpContext">
        /// The current <see cref="HttpContext"/>. When provided, <see cref="ProblemDetailsFactory"/> is used
        /// to enrich the response (e.g. adding a traceId extension) while preserving validation error details.
        /// </param>
        /// <returns>An <see cref="ObjectResult"/> containing a <see cref="ProblemDetails"/> body.</returns>
        public IActionResult ToActionResult(HttpContext? httpContext = null)
        {
            var options = httpContext?.RequestServices.GetService<IOptions<ErrorOrAspNetCoreOptions>>()?.Value;
            var problemDetails = errors.ToProblemDetails(options);

            if (httpContext?.RequestServices.GetService<ProblemDetailsFactory>() is { } factory)
            {
                ProblemDetails factoryPd;

                if (problemDetails is HttpValidationProblemDetails)
                {
                    // Project errors into a ModelStateDictionary so ProblemDetailsFactory can apply its
                    // customizations (e.g. traceId) WITHOUT losing the per-field validation detail.
                    var modelState = new ModelStateDictionary();
                    foreach (var error in errors)
                    {
                        modelState.AddModelError(error.Code, error.Description);
                    }

                    factoryPd = factory.CreateValidationProblemDetails(
                        httpContext, modelState, statusCode: StatusCodes.Status400BadRequest);
                }
                else
                {
                    factoryPd = factory.CreateProblemDetails(httpContext, problemDetails.Status, problemDetails.Title, detail: problemDetails.Detail);
                }

                CopyMissingExtensions(problemDetails, factoryPd);
                return new ObjectResult(factoryPd) { StatusCode = factoryPd.Status };
            }

            return new ObjectResult(problemDetails) { StatusCode = problemDetails.Status };
        }
    }

    private static void CopyMissingExtensions(ProblemDetails source, ProblemDetails destination)
    {
        foreach (var kv in source.Extensions)
        {
            destination.Extensions.TryAdd(kv.Key, kv.Value);
        }
    }
}
