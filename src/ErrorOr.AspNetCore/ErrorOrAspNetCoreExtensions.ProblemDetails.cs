using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ErrorOr.AspNetCore;

public static partial class ErrorOrAspNetCoreExtensions
{
    extension(Error error)
    {
        /// <summary>
        /// Converts an <see cref="Error"/> to a <see cref="ProblemDetails"/> instance.
        /// </summary>
        /// <param name="options">Optional configuration.</param>
        /// <returns>A <see cref="ProblemDetails"/> representing the error.</returns>
        public ProblemDetails ToProblemDetails(ErrorOrAspNetCoreOptions? options = null)
        {
            var problemDetails = new ProblemDetails
            {
                Title = error.Code,
                Detail = error.Description,
                Status = error.ToHttpStatusCode(options),
            };

            if (options?.IncludeMetadataInProblemDetails == true && error.Metadata is not null)
            {
                foreach (var metadata in error.Metadata)
                {
                    problemDetails.Extensions[metadata.Key] = metadata.Value;
                }
            }

            return problemDetails;
        }
    }

    extension(List<Error> errors)
    {
        /// <summary>
        /// Converts a list of <see cref="Error"/> objects to a <see cref="ProblemDetails"/> instance.
        /// When all errors are of type <see cref="ErrorType.Validation"/>, an <see cref="HttpValidationProblemDetails"/>
        /// is returned with per-field error details. Otherwise, the first error drives the response.
        /// Custom mappers in <paramref name="options"/> are checked first.
        /// </summary>
        /// <param name="options">Optional configuration.</param>
        /// <returns>A <see cref="ProblemDetails"/> (or <see cref="HttpValidationProblemDetails"/>) representing the errors.</returns>
        public ProblemDetails ToProblemDetails(ErrorOrAspNetCoreOptions? options = null)
        {
            if (errors.Count == 0)
            {
                return new ProblemDetails { Status = StatusCodes.Status500InternalServerError };
            }

            if (options is not null)
            {
                foreach (var mapper in options.ErrorsToProblemDetailsMappers)
                {
                    if (mapper(errors) is ProblemDetails problemDetails)
                    {
                        return problemDetails;
                    }
                }
            }

            if (errors.TrueForAll(e => e.Type == ErrorType.Validation))
            {
                return errors.ToValidationProblemDetails(options);
            }

            return errors[0].ToProblemDetails(options);
        }

        private HttpValidationProblemDetails ToValidationProblemDetails(ErrorOrAspNetCoreOptions? options)
        {
            var validationProblemDetails = new HttpValidationProblemDetails
            {
                Status = StatusCodes.Status400BadRequest,
            };

            // Group by code so multiple errors with the same field key accumulate their messages.
            foreach (var group in errors.GroupBy(e => e.Code))
            {
                validationProblemDetails.Errors[group.Key] = group.Select(e => e.Description).ToArray();
            }

            if (options?.IncludeMetadataInProblemDetails == true)
            {
                var metadataPerError = errors
                    .Where(e => e.Metadata is not null)
                    .Select(e => new { code = e.Code, metadata = e.Metadata! })
                    .ToArray();

                if (metadataPerError.Length > 0)
                {
                    // Store structured per-error metadata under a single "errorMetadata" extension key
                    // to avoid collisions with validation error keys.
                    validationProblemDetails.Extensions["errorMetadata"] = metadataPerError;
                }
            }

            return validationProblemDetails;
        }
    }
}
