using Microsoft.AspNetCore.Http;

namespace Tests.AspNetCore;

public class HttpStatusCodeExtensionsTests
{
    [Theory]
    [InlineData(ErrorType.Validation, StatusCodes.Status400BadRequest)]
    [InlineData(ErrorType.Unauthorized, StatusCodes.Status401Unauthorized)]
    [InlineData(ErrorType.Forbidden, StatusCodes.Status403Forbidden)]
    [InlineData(ErrorType.NotFound, StatusCodes.Status404NotFound)]
    [InlineData(ErrorType.Conflict, StatusCodes.Status409Conflict)]
    [InlineData(ErrorType.Failure, StatusCodes.Status500InternalServerError)]
    [InlineData(ErrorType.Unexpected, StatusCodes.Status500InternalServerError)]
    public void ToHttpStatusCode_Error_ReturnsExpectedStatusCode(ErrorType errorType, int expectedStatusCode)
    {
        var error = Error.Custom((int)errorType, "Test.Code", "Test description");

        var result = error.ToHttpStatusCode();

        result.Should().Be(expectedStatusCode);
    }

    [Fact]
    public void ToHttpStatusCode_CustomMapper_WinsOverDefault()
    {
        var options = new ErrorOrAspNetCoreOptions();
        options.ErrorToStatusCodeMappers.Add(_ => StatusCodes.Status418ImATeapot);

        var error = Error.NotFound();

        var result = error.ToHttpStatusCode(options);

        result.Should().Be(StatusCodes.Status418ImATeapot);
    }

    [Fact]
    public void ToHttpStatusCode_CustomMapperReturnsNull_FallsThroughToDefault()
    {
        var options = new ErrorOrAspNetCoreOptions();
        options.ErrorToStatusCodeMappers.Add(_ => null);

        var error = Error.NotFound();

        var result = error.ToHttpStatusCode(options);

        result.Should().Be(StatusCodes.Status404NotFound);
    }

    [Fact]
    public void ToHttpStatusCode_List_ReturnsFirstErrorStatusCode()
    {
        var errors = new List<Error>
        {
            Error.NotFound(),
            Error.Validation(),
        };

        var result = errors.ToHttpStatusCode();

        result.Should().Be(StatusCodes.Status404NotFound);
    }

    [Fact]
    public void ToHttpStatusCode_EmptyList_Returns500()
    {
        var result = new List<Error>().ToHttpStatusCode();

        result.Should().Be(StatusCodes.Status500InternalServerError);
    }
}
