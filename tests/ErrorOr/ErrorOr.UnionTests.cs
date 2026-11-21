#if NET11_0_OR_GREATER
namespace Tests;

using ErrorOr;

using FluentAssertions;

public class ErrorOrUnionTests
{
    public static readonly TheoryData<ErrorOr<string?>, string> SwitchCases = new()
    {
        { "Ada", "Value: Ada" },
        {
            ErrorOrFactory.From<string?>(errors: new List<Error>
            {
                Error.Validation(code: "first"),
                Error.Conflict(code: "second"),
            }),
            "Errors: 2 | First: first"
        },
        {
            ErrorOr<string?>.IUnionMembers.Create((Errors)Error.NotFound(code: "wrapped")),
            "Not found: wrapped"
        },
        {
            ErrorOrFactory.From<string?>(errors: new List<Error>
            {
                Error.NotFound(code: "missing"),
                Error.Validation(code: "invalid"),
            }),
            "Not found: missing"
        },
        {
            ErrorOrFactory.From<string?>(errors: new List<Error>
            {
                Error.Validation(code: "first"),
                Error.NotFound(code: "second"),
            }),
            "Errors: 2 | First: first"
        },
        {
            ErrorOrFactory.From<string?>(errors: new List<Error>
            {
                Error.Validation(code: "first"),
                Error.Validation(code: "second"),
            }),
            "Validation errors: 2"
        },
        {
            ErrorOrFactory.From<string?>(errors: new List<Error>
            {
                Error.Conflict(code: "first"),
                Error.Conflict(code: "second"),
            }),
            "Conflict errors: 2"
        },
        { ErrorOrFactory.From<string?>(value: null), "No value" },
    };

    [Theory]
    [MemberData(nameof(SwitchCases))]
    public void SwitchExpression_WhenMatchingSuccessErrorsOrNull_ShouldReturnExpectedString(
        ErrorOr<string?> result,
        string expected)
    {
        string description = result switch
        {
            string value => $"Value: {value}",
            Errors { First: { Type: ErrorType.NotFound } } errors => $"Not found: {errors.First.Code}",
            Errors errors when errors.All.All(error => error.Type == errors.First.Type) =>
                $"{errors.First.Type} errors: {errors.All.Count}",
            Errors errors => $"Errors: {errors.All.Count} | First: {errors.First.Code}",
            null => "No value",
        };

        description.Should().Be(expected);
    }

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
    public void SwitchExpression_WhenFallbackMatchesErrors_ShouldExposeTheFullAllPayload()
    {
        List<Error> expected = [Error.Validation(), Error.NotFound()];
        ErrorOr<int> result = expected;

        object response = result switch
        {
            int value => value,
            Errors { First: { Type: ErrorType.NotFound } } => "Not found",
            Errors errors => errors.All,
        };

        response.Should().BeOfType<List<Error>>().Which.Should().Equal(expected);
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
    public void SwitchExpression_WhenCreatedFromDefaultErrors_ShouldSafelyMatchErrorsArm()
    {
        ErrorOr<int> result = ErrorOr<int>.IUnionMembers.Create(default(Errors));

        string description = result switch
        {
            int value => $"Value: {value}",
            Errors errors => $"Errors: {errors.All.Count} | First: {errors.First.Code}",
        };

        description.Should().Be("Errors: 1 | First: ErrorOr.EmptyInitialErrors");
    }

    [Fact]
    public void UnionMembers_CreateValue_ShouldCreateASuccessValue()
    {
        ErrorOr<int> created = ErrorOr<int>.IUnionMembers.Create(42);

        created.IsError.Should().BeFalse();
        created.Value.Should().Be(42);
    }

    [Fact]
    public void UnionMembers_Value_ShouldReturnValueForSuccessAndErrorsWrapperForFailure()
    {
        ErrorOr<int>.IUnionMembers successMembers = ErrorOrFactory.From<int>(123);
        ErrorOr<int>.IUnionMembers errorMembers = ErrorOrFactory.From<int>(Error.Validation());

        successMembers.Value.Should().Be(123);
        errorMembers.Value.Should().BeOfType<Errors>();
    }

    [Fact]
    public void UnionMembers_TryGetErrors_ShouldReturnSuccessValueWhenTValueIsErrors()
    {
        Errors payload = new[] { Error.Unexpected(), Error.Validation() };
        ErrorOr<Errors> result = payload;
        ErrorOr<Errors>.IUnionMembers members = result;

        bool success = members.TryGetValue(out Errors extracted);

        success.Should().BeTrue();
        extracted.First.Should().Be(payload.First);
        extracted.All.Should().Equal(payload.All);
    }

    [Fact]
    public void GetEnumerator_WhenError_ShouldEnumerateAllErrors()
    {
        ErrorOr<int> result = new[] { Error.Validation(), Error.NotFound() };
        List<Error> collected = [];

        foreach (Error error in result)
        {
            collected.Add(error);
        }

        collected.Should().Equal(result.Errors);
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
    public void SwitchExpression_WhenValueTypeIsListOfErrors_ShouldKeepSuccessAndErrorDistinct()
    {
        List<Error> listValue = [Error.Unexpected(code: "value")];
        List<Error> errorList = [Error.Unexpected(code: "error")];
        ErrorOr<List<Error>> value = ErrorOrFactory.From<List<Error>>(value: listValue);
        ErrorOr<List<Error>> error = ErrorOrFactory.From<List<Error>>(errors: errorList);

        MatchList(value).Should().Be("List: 1");
        MatchList(error).Should().Be("Errors: 1");
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
