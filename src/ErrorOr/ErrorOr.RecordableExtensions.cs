namespace ErrorOr;

/// <summary>
/// Extension methods for <see cref="IErrorOr"/> and <see cref="IErrorOr{TValue}"/> that provide
/// format-agnostic recording without a dependency on <c>System.Text.Json</c>.
/// </summary>
public static partial class ErrorOrExtensions
{
    /// <summary>
    /// Returns a string representation of the current state using the provided <paramref name="recorder"/> function.
    /// </summary>
    /// <param name="errorOr">The <see cref="IErrorOr"/> instance.</param>
    /// <param name="recorder">
    /// A function that receives the current <see cref="IErrorOr"/> instance and returns a string representation.
    /// </param>
    /// <returns>The string produced by <paramref name="recorder"/>.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="recorder"/> is <see langword="null"/>.</exception>
    /// <remarks>
    /// Use this overload when working with a type-erased <see cref="IErrorOr"/> reference.
    /// The caller can inspect <see cref="IErrorOr.IsError"/> and <see cref="IErrorOr.Errors"/>
    /// within the function to produce a recording in any format.
    /// </remarks>
    public static string GetRecording(this IErrorOr errorOr, Func<IErrorOr, string> recorder)
    {
        if (recorder is null)
        {
            throw new ArgumentNullException(nameof(recorder));
        }

        return recorder(errorOr);
    }

    /// <summary>
    /// Returns a string representation of the current state using the provided <paramref name="recorder"/> function.
    /// </summary>
    /// <typeparam name="TValue">The type of the underlying value.</typeparam>
    /// <param name="errorOr">The <see cref="IErrorOr{TValue}"/> instance.</param>
    /// <param name="recorder">
    /// A function that receives the current <see cref="IErrorOr{TValue}"/> instance and returns a string representation.
    /// </param>
    /// <returns>The string produced by <paramref name="recorder"/>.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="recorder"/> is <see langword="null"/>.</exception>
    /// <remarks>
    /// Use this overload when working with a typed <see cref="IErrorOr{TValue}"/> reference.
    /// The caller can inspect <see cref="IErrorOr.IsError"/>, <see cref="IErrorOr{TValue}.Value"/>,
    /// and <see cref="IErrorOr.Errors"/> within the function to produce a recording in any format.
    /// </remarks>
    public static string GetRecording<TValue>(this IErrorOr<TValue> errorOr, Func<IErrorOr<TValue>, string> recorder)
    {
        if (recorder is null)
        {
            throw new ArgumentNullException(nameof(recorder));
        }

        return recorder(errorOr);
    }

    /// <summary>
    /// Returns a string representation of the current state by calling the appropriate delegate
    /// based on whether the instance is in a value or error state.
    /// </summary>
    /// <typeparam name="TValue">The type of the underlying value.</typeparam>
    /// <param name="errorOr">The <see cref="IErrorOr{TValue}"/> instance.</param>
    /// <param name="onValue">A function that receives the value and returns a string representation.</param>
    /// <param name="onError">A function that receives the list of errors and returns a string representation.</param>
    /// <returns>The string produced by <paramref name="onValue"/> or <paramref name="onError"/>.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="onValue"/> or <paramref name="onError"/> is <see langword="null"/>.</exception>
    /// <remarks>
    /// Use this overload when working with a typed <see cref="IErrorOr{TValue}"/> reference.
    /// Mirrors the <c>Match</c> pattern — one delegate per state — giving full typed access to the value.
    /// </remarks>
    public static string GetRecording<TValue>(this IErrorOr<TValue> errorOr, Func<TValue, string> onValue, Func<List<Error>, string> onError)
    {
        if (onValue is null)
        {
            throw new ArgumentNullException(nameof(onValue));
        }

        if (onError is null)
        {
            throw new ArgumentNullException(nameof(onError));
        }

        return errorOr.IsError ? onError(errorOr.Errors!) : onValue(errorOr.Value);
    }
}
