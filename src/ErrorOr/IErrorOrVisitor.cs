namespace ErrorOr;

/// <summary>
/// Defines a visit strategy for one of two states of <see cref="IErrorOr"/> instance.
/// </summary>
/// <remarks>
/// Invokes <see cref="Visit{TValue}(TValue)" /> when the state of <see cref="IErrorOr"/>
/// is success and <see cref="Visit(List{Error})" /> when the state is error.
/// </remarks>
public interface IErrorOrVisitor
{
    /// <summary>
    /// Visits the success value.
    /// </summary>
    /// <typeparam name="TValue">The type of the value.</typeparam>
    /// <param name="value">The success value to visit.</param>
    /// <returns>An <see cref="IErrorOr"/> representing the result of the visit.</returns>
    IErrorOr Visit<TValue>(TValue value);

    /// <summary>
    /// Visits a list of errors.
    /// </summary>
    /// <param name="errors">The list of errors to visit.</param>
    /// <returns>An <see cref="IErrorOr"/> representing the result of the visit.</returns>
    IErrorOr Visit(List<Error> errors);
}
