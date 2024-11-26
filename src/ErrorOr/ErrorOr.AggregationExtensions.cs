namespace ErrorOr.ErrorOr
{
    public static partial class AggregationExtensions
    {
        /// <summary>
        /// Appends errors from multiple <see cref="IErrorOr"/> instances into the current <see cref="ErrorOr{TValue}"/> instance.
        /// </summary>
        /// <typeparam name="TValue">The type of the underlying value in the <paramref name="errorOr"/>.</typeparam>
        /// <param name="errorOr">The current <see cref="ErrorOr{TValue}"/> instance.</param>
        /// <param name="errors">The additional <see cref="IErrorOr"/> instances whose errors need to be appended.</param>
        /// <returns>A new <see cref="ErrorOr{TValue}"/> instance containing all combined errors, or the original if no errors are added.</returns>
        public static ErrorOr<TValue> AppendErrors<TValue>(
            this ErrorOr<TValue> errorOr,
            params IErrorOr[]? errors)
        {
            // Guard clause to handle null or empty errors array.
            if (errors is null || errors.Length == 0)
            {
                return errorOr;
            }

            // Start with the errors from the current ErrorOr instance.
            var combinedErrors = new List<Error>(errorOr.ErrorsOrEmptyList);

            // Add errors from the additional ErrorOr instances if they contain errors.
            foreach (var error in errors)
            {
                if (error?.IsError == true)
                {
                    // Add errors from error instances that are in an error state
                    combinedErrors.AddRange(error.Errors ?? Enumerable.Empty<Error>());
                }
            }

            // If there are errors, return a new ErrorOr<TValue> instance with those errors.
            return combinedErrors.Count > 0 ? ErrorOr<TValue>.FromError(combinedErrors) : errorOr;
        }

        /// <summary>
        /// Appends errors from multiple <see cref="IErrorOr"/> instances into the current <see cref="ErrorOr{TValue}"/> instance,
        /// and includes additional metadata for enhanced error handling.
        /// </summary>
        /// <typeparam name="TValue">The type of the underlying value in the <paramref name="errorOr"/>.</typeparam>
        /// <param name="errorOr">The current <see cref="ErrorOr{TValue}"/> instance.</param>
        /// <param name="metadata">Additional metadata to include with the errors.</param>
        /// <param name="errors">The additional <see cref="IErrorOr"/> instances whose errors need to be appended.</param>
        /// <returns>A new <see cref="ErrorOr{TValue}"/> instance containing all combined errors with metadata, or the original if no errors are added.</returns>
        public static ErrorOr<TValue> AppendErrorsWithMetadata<TValue>(
            this ErrorOr<TValue> errorOr,
            Dictionary<string, object> metadata,
            params IErrorOr[]? errors)
        {
            // Guard clause to handle null or empty errors array.
            if (errors is null || errors.Length == 0)
            {
                return errorOr;
            }

            // Start with the errors from the current ErrorOr instance.
            var combinedErrors = new List<Error>(errorOr.ErrorsOrEmptyList);

            // Add errors from the additional ErrorOr instances if they contain errors.
            foreach (var error in errors)
            {
                if (error?.IsError == true)
                {
                    // Add errors from error instances that are in an error state
                    combinedErrors.AddRange(error.Errors ?? Enumerable.Empty<Error>());
                }
            }

            // Add metadata to each error
            var enhancedErrors = combinedErrors.Select(e => e with { Metadata = metadata }).ToList();

            // If there are errors, return a new ErrorOr<TValue> instance with those errors and metadata.
            return enhancedErrors.Count > 0 ? ErrorOr<TValue>.FromError(enhancedErrors) : errorOr;
        }
    }
}
