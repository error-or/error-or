namespace ErrorOr;

public readonly partial record struct ErrorOr<TValue>
{
    /// <inheritdoc/>
    public void Accept(IErrorOrVisitor visitor)
    {
        if (visitor is null)
        {
            throw new ArgumentNullException(nameof(visitor));
        }

        if (IsError)
        {
            visitor.VisitErrors(Errors);
        }
        else
        {
            visitor.VisitValue(Value);
        }
    }
}
