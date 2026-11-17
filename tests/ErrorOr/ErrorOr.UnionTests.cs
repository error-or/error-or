#if NET11_0_OR_GREATER
namespace Tests;

using ErrorOr;

using FluentAssertions;

public class ErrorOrUnionTests
{
    [Fact]
    public void SwitchExpression_WhenSingleError_ShouldExposeErrorsWrapper()
    {
        Error expected = Error.Unexpected();
        ErrorOr<int> result = expected;

        Errors errors = result switch
        {
            int value => throw new InvalidOperationException($"Unexpected value: {value}"),
            Errors errorCase => errorCase,
        };

        errors.First.Should().Be(expected);
        errors.All.Should().ContainSingle().Which.Should().Be(expected);
    }

    [Fact]
    public void SwitchExpression_WhenReturningFirstError_ShouldReturnOnlyTheFirstError()
    {
        Error firstError = Error.Validation(code: "first");
        ErrorOr<int> result = new List<Error> { firstError, Error.Conflict(code: "second") };

        object response = result switch
        {
            int value => value,
            Errors errors => errors.First,
        };

        response.Should().Be((object)firstError);
    }

    [Fact]
    public void SwitchExpression_WhenFirstErrorIsNotFound_ShouldMatchThePropertyPattern()
    {
        ErrorOr<int> notFound = Error.NotFound();
        ErrorOr<int> otherErrors = new List<Error> { Error.Validation(), Error.NotFound() };

        MatchWithNotFound(notFound).Should().Be("Not found");
        MatchWithNotFound(otherErrors).Should().Be("Bad request: 2");
    }

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
    public void SwitchExpression_WhenMultipleErrorsAreListOrArray_ShouldPreserveTheFirstAndAllErrors()
    {
        List<Error> expected =
        [
            Error.Unexpected(code: "first"),
            Error.Validation(code: "second"),
        ];
        ErrorOr<int> result = expected;
        Error[] expectedArray =
        [
            Error.Unauthorized(code: "array-first"),
            Error.Validation(code: "array-second"),
        ];
        ErrorOr<int> arrayResult = expectedArray;

        Errors errors = result switch
        {
            int value => throw new InvalidOperationException($"Unexpected value: {value}"),
            Errors errorCase => errorCase,
        };
        Errors arrayErrors = arrayResult switch
        {
            int value => throw new InvalidOperationException($"Unexpected value: {value}"),
            Errors errorCase => errorCase,
        };

        errors.First.Should().Be(expected[0]);
        errors.All.Should().Equal(expected);
        arrayErrors.First.Should().Be(expectedArray[0]);
        arrayErrors.All.Should().Equal(expectedArray);
    }

    [Fact]
    public void Errors_WhenDefault_ShouldReturnNoFirstErrorAndAnEmptyList()
    {
        Errors errors = default;

        errors.First.Code.Should().Be("ErrorOr.NoFirstError");
        errors.All.Should().BeEmpty();
    }

    [Fact]
    public void SwitchExpression_WhenInitialErrorsAreEmpty_ShouldExposeTheInvalidInitialErrorsError()
    {
        ErrorOr<int> result = ErrorOrFactory.From<int>(new List<Error>());

        Errors errors = result switch
        {
            int value => throw new InvalidOperationException($"Unexpected value: {value}"),
            Errors errorCase => errorCase,
        };

        errors.First.Code.Should().Be("ErrorOr.EmptyInitialErrors");
        errors.All.Should().ContainSingle().Which.Code.Should().Be("ErrorOr.EmptyInitialErrors");
    }

    [Fact]
    public void SwitchExpression_WhenMatchingAFluentResult_ShouldHandleValueErrorsAndNull()
    {
        ErrorOr<string?> value = " Ada ";
        ErrorOr<string?> error = Error.Unexpected();
        ErrorOr<string?> nullValue = (string?)null;

        MatchNullable(value.Then(name => name?.Trim())).Should().Be("Value: Ada");
        MatchNullable(error.Then(name => name?.Trim())).Should().Be("Errors: 1");
        MatchNullable(nullValue.Then(name => name?.Trim())).Should().Be("No value");
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
            Errors errors => $"Errors: {errors.All.Count}",
        };

    private static string MatchNullable(ErrorOr<string?> errorOr) =>
        errorOr switch
        {
            string value => $"Value: {value}",
            Errors errors => $"Errors: {errors.All.Count}",
            null => "No value",
        };

    private static string MatchList(ErrorOr<List<Error>> errorOr) =>
        errorOr switch
        {
            List<Error> errors => $"List: {errors.Count}",
            Errors errors => $"Errors: {errors.All.Count}",
            null => "No value",
        };

    private static string MatchWithNotFound(ErrorOr<int> errorOr) =>
        errorOr switch
        {
            int value => $"Value: {value}",
            Errors { First: { Type: ErrorType.NotFound } } => "Not found",
            Errors errors => $"Bad request: {errors.All.Count}",
        };
}
#endif
