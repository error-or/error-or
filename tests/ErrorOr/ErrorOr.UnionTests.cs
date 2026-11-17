#if NET11_0_OR_GREATER
namespace Tests;

using ErrorOr;

using FluentAssertions;

public class ErrorOrUnionTests
{
    [Fact]
    public void SwitchExpression_WhenValueOrErrors_ShouldMatchTheContainedCase()
    {
        ErrorOr<int> value = 42;
        ErrorOr<int> singleError = Error.Unexpected();
        ErrorOr<int> errors = new List<Error> { Error.Unexpected() };
        ErrorOr<int> errorArray = [Error.Unexpected(), Error.Validation()];

        Match(value).Should().Be("Value: 42");
        Match(singleError).Should().Be("Errors: 1");
        Match(errors).Should().Be("Errors: 1");
        Match(errorArray).Should().Be("Errors: 2");
    }

    [Fact]
    public void SwitchExpression_WhenValueOrDefaultIsNull_ShouldMatchNullCase()
    {
        ErrorOr<string?> value = (string?)null;
        ErrorOr<string?> defaultValue = default;

        MatchNullable(value).Should().Be("No value");
        MatchNullable(defaultValue).Should().Be("No value");
    }

    [Fact]
    public void SwitchExpression_WhenValueTypeIsListOfErrors_ShouldMatchTheListCase()
    {
        List<Error> errors = [Error.Unexpected()];
        ErrorOr<List<Error>> value = ErrorOrFactory.From<List<Error>>(value: errors);
        ErrorOr<List<Error>> error = ErrorOrFactory.From<List<Error>>(errors: errors);

        MatchList(value).Should().Be("List: 1");
        MatchList(error).Should().Be("List: 1");
    }

    private static string Match(ErrorOr<int> errorOr) =>
        errorOr switch
        {
            int value => $"Value: {value}",
            List<Error> errors => $"Errors: {errors.Count}",
        };

    private static string MatchNullable(ErrorOr<string?> errorOr) =>
        errorOr switch
        {
            string value => $"Value: {value}",
            List<Error> errors => $"Errors: {errors.Count}",
            null => "No value",
        };

    private static string MatchList(ErrorOr<List<Error>> errorOr) =>
        errorOr switch
        {
            List<Error> errors => $"List: {errors.Count}",
            null => "No value",
        };
}
#endif
