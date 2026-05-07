namespace ErrorOr;

/// <summary>
/// Extension methods for <see cref="IErrorOr"/> and <see cref="IErrorOr{TValue}"/> that provide
/// format-agnostic recording without a dependency on <see cref="System.Text.Json"/>.
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
}
