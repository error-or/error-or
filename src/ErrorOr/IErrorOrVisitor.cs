namespace ErrorOr;

/// <summary>
/// Defines a visit strategy for one of two states of <see cref="IErrorOr"/> instance.
/// </summary>
/// <remarks>
/// Invokes <see cref="VisitValue{TValue}(TValue)" /> when the state of <see cref="IErrorOr"/>
/// is success and <see cref="VisitErrors(List{Error})" /> when the state is error.
/// </remarks>
public interface IErrorOrVisitor
{
    /// <summary>
    /// Visits the success value.
    /// </summary>
    /// <typeparam name="TValue">The type of the value.</typeparam>
    /// <param name="value">The success value to visit.</param>
    void VisitValue<TValue>(TValue value);

    /// <summary>
    /// Visits a list of errors.
    /// </summary>
    /// <param name="errors">The list of errors to visit.</param>
    void VisitErrors(List<Error> errors);
}
