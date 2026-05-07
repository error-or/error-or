namespace ErrorOr;

/// <summary>
/// Provides a base implementation of <see cref="IErrorOrVisitor"/> that can be used to create custom visitors
/// for <see cref="ErrorOr{TValue}"/>. This class implements the visit methods for both value and errors,
/// allowing derived classes to focus on the specific logic for handling each case.
/// </summary>
public abstract class BaseErrorOrVisitor : IErrorOrVisitor
{
    /// <summary>
    /// Processes the specified value and returns a result indicating success or an error.
    /// </summary>
    /// <typeparam name="TValue">The type of the value to process.</typeparam>
    /// <param name="value">The value to be processed.</param>
    /// <returns>An <see cref="ErrorOr{Success}"/> that represents the outcome of processing the value. Returns a success result
    /// if the operation completes successfully; otherwise, returns an error result.</returns>
    public abstract ErrorOr<Success> Visit<TValue>(TValue value);

    /// <summary>
    /// Processes a collection of errors and returns a result indicating success or an error.
    /// </summary>
    /// <param name="errors">The list of errors to process. Cannot be null.</param>
    /// <returns>An <see cref="ErrorOr{Success}"/> value representing the outcome of processing the errors. Returns a success result if the
    /// errors are handled successfully; otherwise, returns an error result.</returns>
    public abstract ErrorOr<Success> Visit(List<Error> errors);

    IErrorOr IErrorOrVisitor.Visit<TValue>(TValue value) => Visit(value);

    IErrorOr IErrorOrVisitor.Visit(List<Error> errors) => Visit(errors);
}
