namespace ErrorOr;

public readonly partial record struct ErrorOr<TValue>
{
    /// <summary>
    /// Accepts the provided <paramref name="visitor"/> and invokes visit method appropriate to current state of <see cref="IErrorOr" />.
    /// </summary>
    /// <param name="visitor">An <see cref="IErrorOrVisitor"/> implementing visit methods for value and errors.</param>
    /// <returns>Unexpected error when visitor is null; otherwise success.</returns>
    public ErrorOr<Success> Accept(IErrorOrVisitor visitor)
    {
        if (visitor is null)
        {
            return KnownErrors.NullVisitor;
        }

        var result = IsError ? visitor.VisitErrors(Errors) : visitor.VisitValue(Value);
        if (result.IsError)
        {
            return result.Errors!;
        }
        else
        {
            return Result.Success;
        }
    }

    IErrorOr IErrorOr.Accept(IErrorOrVisitor visitor)
    {
        return Accept(visitor);
    }
}
