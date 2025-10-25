using ErrorOr;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Extensions.AspNetCore
{
    /// <summary>
    /// Provides extension methods for ASP.NET Core <see cref="ControllerBase"/> to convert
    /// <see cref="ErrorOr{T}"/> results into standardized <see cref="ActionResult"/> or <see cref="IActionResult"/>.
    /// </summary>
    public static class ControllerBaseExtensions
    {
        /// <summary>
        /// Converts an <see cref="ErrorOr{T}"/> to an <see cref="ActionResult{T}"/>.
        /// If the result contains an error, it converts it to a standardized <see cref="ProblemDetails"/> response.
        /// </summary>
        /// <typeparam name="T">Type of the success value.</typeparam>
        /// <param name="controller">The controller instance calling this method.</param>
        /// <param name="result">The <see cref="ErrorOr{T}"/> result to convert.</param>
        /// <returns>An <see cref="ActionResult{T}"/> containing either the value or the problem details.</returns>
        public static ActionResult<T> ConvertToActionResult<T>(
            this ControllerBase controller,
            ErrorOr<T> result) =>
            result.MatchFirst<ActionResult<T>>(
                value => controller.Ok(value),
                error => ConvertError(controller, error));

        /// <summary>
        /// Converts an <see cref="ErrorOr{T}"/> to an <see cref="ActionResult"/> without returning data.
        /// If the result contains an error, it converts it to a standardized <see cref="ProblemDetails"/> response.
        /// </summary>
        /// <typeparam name="T">Type of the success value.</typeparam>
        /// <param name="controller">The controller instance calling this method.</param>
        /// <param name="result">The <see cref="ErrorOr{T}"/> result to convert.</param>
        /// <returns>An <see cref="ActionResult"/> representing success or a problem.</returns>
        public static ActionResult ConvertToActionResultWithoutData<T>(
            this ControllerBase controller,
            ErrorOr<T> result) =>
            result.MatchFirst(
                _ => controller.Ok(),
                error => ConvertError(controller, error));

        /// <summary>
        /// Converts an <see cref="ErrorOr{TIn}"/> to <see cref="ActionResult{TOut}"/> using a mapping function.
        /// </summary>
        /// <typeparam name="TIn">Input type of the result.</typeparam>
        /// <typeparam name="TOut">Output type after mapping.</typeparam>
        /// <param name="controller">The controller instance calling this method.</param>
        /// <param name="result">The <see cref="ErrorOr{TIn}"/> result to convert.</param>
        /// <param name="map">Function to convert <typeparamref name="TIn"/> to <typeparamref name="TOut"/>.</param>
        /// <returns>An <see cref="ActionResult{TOut}"/> containing either the mapped value or problem details.</returns>
        public static ActionResult<TOut> ConvertToActionResult<TIn, TOut>(
            this ControllerBase controller,
            ErrorOr<TIn> result,
            Func<TIn, TOut> map) =>
            result.MatchFirst(
                value => controller.Ok(map(value)),
                error => ConvertError(controller, error));

        /// <summary>
        /// Converts an <see cref="ErrorOr{TIn}"/> asynchronously to <see cref="ActionResult{TOut}"/> using a mapping function.
        /// </summary>
        /// <typeparam name="TIn">Input type of the result.</typeparam>
        /// <typeparam name="TOut">Output type after mapping.</typeparam>
        /// <param name="controller">The controller instance calling this method.</param>
        /// <param name="result">The <see cref="ErrorOr{TIn}"/> result to convert.</param>
        /// <param name="map">Asynchronous function to convert <typeparamref name="TIn"/> to <see cref="ActionResult{TOut}"/>.</param>
        /// <returns>A task producing an <see cref="ActionResult{TOut}"/> containing either the mapped value or problem details.</returns>
        public static async Task<ActionResult<TOut>> ConvertToActionResultAsync<TIn, TOut>(
            this ControllerBase controller,
            ErrorOr<TIn> result,
            Func<TIn, Task<ActionResult<TOut>>> map)
        {
            if (result.IsError)
            {
                return ConvertError(controller, result.FirstError);
            }

            return await map(result.Value);
        }

        /// <summary>
        /// Converts an <see cref="ErrorOr{TIn}"/> to <see cref="IActionResult"/> using a mapping function.
        /// </summary>
        /// <typeparam name="TIn">Input type of the result.</typeparam>
        /// <typeparam name="TOut">Output type after mapping.</typeparam>
        /// <param name="controller">The controller instance calling this method.</param>
        /// <param name="result">The <see cref="ErrorOr{TIn}"/> result to convert.</param>
        /// <param name="map">Function to convert <typeparamref name="TIn"/> to <typeparamref name="TOut"/>.</param>
        /// <returns>An <see cref="IActionResult"/> containing either the mapped value or problem details.</returns>
        public static IActionResult ConvertToIActionResult<TIn, TOut>(
            this ControllerBase controller,
            ErrorOr<TIn> result,
            Func<TIn, TOut> map) =>
            result.MatchFirst(
                value => controller.Ok(map(value)),
                error => ConvertError(controller, error));

        /// <summary>
        /// Converts an <see cref="ErrorOr{TIn}"/> asynchronously to <see cref="IActionResult"/> using a mapping function.
        /// </summary>
        /// <typeparam name="TIn">Input type of the result.</typeparam>
        /// <typeparam name="TOut">Output type after mapping.</typeparam>
        /// <param name="controller">The controller instance calling this method.</param>
        /// <param name="result">The <see cref="ErrorOr{TIn}"/> result to convert.</param>
        /// <param name="map">Asynchronous function to convert <typeparamref name="TIn"/> to <typeparamref name="TOut"/>.</param>
        /// <returns>A task producing an <see cref="IActionResult"/> containing either the mapped value or problem details.</returns>
        public static async Task<IActionResult> ConvertToIActionResultAsync<TIn, TOut>(
            this ControllerBase controller,
            ErrorOr<TIn> result,
            Func<TIn, Task<TOut>> map)
        {
            if (result.IsError)
            {
                return ConvertError(controller, result.FirstError);
            }

            var output = await map(result.Value);
            return controller.Ok(output);
        }

        /// <summary>
        /// Converts an <see cref="Error"/> into a standardized <see cref="ProblemDetails"/> response
        /// following RFC 7807 for HTTP APIs.
        /// </summary>
        /// <param name="controller">The controller instance calling this method.</param>
        /// <param name="error">The <see cref="Error"/> to convert.</param>
        /// <returns>An <see cref="ActionResult"/> containing <see cref="ProblemDetails"/>.</returns>
        public static ActionResult ConvertError(this ControllerBase controller, Error error)
        {
            var statusCode = error.Type switch
            {
                ErrorType.Validation => StatusCodes.Status400BadRequest,
                ErrorType.Forbidden => StatusCodes.Status403Forbidden,
                ErrorType.NotFound => StatusCodes.Status404NotFound,
                ErrorType.Conflict => StatusCodes.Status409Conflict,
                ErrorType.Failure => StatusCodes.Status400BadRequest,
                ErrorType.Unexpected => StatusCodes.Status500InternalServerError,
                _ => StatusCodes.Status500InternalServerError,
            };

            var problem = new ProblemDetails
            {
                Title = error.Type.ToString(),
                Detail = error.Description,
                Status = statusCode,
                Type = $"https://httpstatuses.com/{statusCode}",
            };

            // Include the error code and any additional info in the extensions
            problem.Extensions["code"] = error.Code;

            return new ObjectResult(problem)
            {
                StatusCode = problem.Status,
            };
        }
    }
}
