namespace Tests;

using ErrorOr;

using FluentAssertions;

public class ErrorsTests
{
    [Fact]
    public void ImplicitConversion_FromSingleError_ShouldExposeItAsFirstAndAll()
    {
        Error expected = Error.NotFound();

        Errors errors = expected;

        errors.First.Should().Be(expected);
        errors.All.Should().ContainSingle().Which.Should().Be(expected);
    }

    [Fact]
    public void ImplicitConversion_FromList_ShouldPreserveTheFirstAndAllErrors()
    {
        List<Error> expected = [Error.NotFound(), Error.Validation()];

        Errors errors = expected;

        errors.First.Should().Be(expected[0]);
        errors.All.Should().Equal(expected);
    }

    [Fact]
    public void ImplicitConversion_FromArray_ShouldPreserveTheFirstAndAllErrors()
    {
        Error[] expected = [Error.NotFound(), Error.Validation()];

        Errors errors = expected;

        errors.First.Should().Be(expected[0]);
        errors.All.Should().Equal(expected);
    }

    [Fact]
    public void ImplicitConversion_FromEmptyCollections_ShouldReturnAnEmptyErrorsValue()
    {
        Errors fromList = new List<Error>();
        Errors fromArray = Array.Empty<Error>();

        fromList.First.Code.Should().Be("ErrorOr.NoFirstError");
        fromList.All.Should().BeEmpty();
        fromArray.First.Code.Should().Be("ErrorOr.NoFirstError");
        fromArray.All.Should().BeEmpty();
    }

    [Fact]
    public void ImplicitConversion_FromNullCollections_ShouldThrow()
    {
        Action convertNullList = () => ConvertList(null!);
        Action convertNullArray = () => ConvertArray(null!);

        convertNullList.Should().Throw<ArgumentNullException>();
        convertNullArray.Should().Throw<ArgumentNullException>();
    }

    private static Errors ConvertList(List<Error> errors) => errors;

    private static Errors ConvertArray(Error[] errors) => errors;
}
