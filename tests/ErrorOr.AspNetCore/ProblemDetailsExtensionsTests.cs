using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Tests.AspNetCore;

public class ProblemDetailsExtensionsTests
{
    [Fact]
    public void ToProblemDetails_SingleError_SetsStatusTitleAndDetail()
    {
        var error = Error.NotFound("User.NotFound", "User was not found.");

        var pd = error.ToProblemDetails();

        pd.Status.Should().Be(StatusCodes.Status404NotFound);
        pd.Title.Should().Be("User.NotFound");
        pd.Detail.Should().Be("User was not found.");
    }

    [Fact]
    public void ToProblemDetails_MetadataExcludedByDefault()
    {
        var error = Error.Validation(metadata: new Dictionary<string, object> { ["field"] = "Email" });

        var pd = error.ToProblemDetails();

        pd.Extensions.Should().NotContainKey("field");
    }

    [Fact]
    public void ToProblemDetails_MetadataIncludedWhenOptionIsSet()
    {
        var options = new ErrorOrAspNetCoreOptions { IncludeMetadataInProblemDetails = true };
        var error = Error.Validation(metadata: new Dictionary<string, object> { ["field"] = "Email" });

        var pd = error.ToProblemDetails(options);

        pd.Extensions.Should().ContainKey("field").WhoseValue.Should().Be("Email");
    }

    [Fact]
    public void ToProblemDetails_AllValidationErrors_ReturnsHttpValidationProblemDetails()
    {
        var errors = new List<Error>
        {
            Error.Validation("Email.Invalid", "Email is invalid."),
            Error.Validation("Name.Required", "Name is required."),
        };

        var pd = errors.ToProblemDetails();

        pd.Should().BeOfType<HttpValidationProblemDetails>();
        pd.Status.Should().Be(StatusCodes.Status400BadRequest);
        var vpd = (HttpValidationProblemDetails)pd;
        vpd.Errors.Should().ContainKey("Email.Invalid").WhoseValue.Should().Contain("Email is invalid.");
        vpd.Errors.Should().ContainKey("Name.Required").WhoseValue.Should().Contain("Name is required.");
    }

    [Fact]
    public void ToProblemDetails_MultipleValidationErrorsSameCode_AccumulatesMessages()
    {
        var errors = new List<Error>
        {
            Error.Validation("Email.Invalid", "Too long."),
            Error.Validation("Email.Invalid", "Invalid format."),
        };

        var pd = (HttpValidationProblemDetails)errors.ToProblemDetails();

        pd.Errors["Email.Invalid"].Should().BeEquivalentTo(new[] { "Too long.", "Invalid format." });
    }

    [Fact]
    public void ToProblemDetails_MixedErrors_ReturnsFirstErrorProblemDetails()
    {
        var errors = new List<Error>
        {
            Error.NotFound("User.NotFound", "User was not found."),
            Error.Validation("Email.Invalid", "Email is invalid."),
        };

        var pd = errors.ToProblemDetails();

        pd.Should().NotBeOfType<HttpValidationProblemDetails>();
        pd.Status.Should().Be(StatusCodes.Status404NotFound);
    }

    [Fact]
    public void ToProblemDetails_EmptyList_Returns500()
    {
        var pd = new List<Error>().ToProblemDetails();

        pd.Status.Should().Be(StatusCodes.Status500InternalServerError);
    }

    [Fact]
    public void ToProblemDetails_CustomMapper_WinsOverDefault()
    {
        var options = new ErrorOrAspNetCoreOptions();
        options.ErrorsToProblemDetailsMappers.Add(_ => new ProblemDetails { Status = StatusCodes.Status418ImATeapot, Title = "Custom" });

        var errors = new List<Error> { Error.NotFound() };

        var pd = errors.ToProblemDetails(options);

        pd.Status.Should().Be(StatusCodes.Status418ImATeapot);
        pd.Title.Should().Be("Custom");
    }

    [Fact]
    public void ToProblemDetails_CustomMapperReturnsNull_FallsThroughToDefault()
    {
        var options = new ErrorOrAspNetCoreOptions();
        options.ErrorsToProblemDetailsMappers.Add(_ => null);

        var errors = new List<Error> { Error.NotFound("X.NotFound", "Not found.") };

        var pd = errors.ToProblemDetails(options);

        pd.Status.Should().Be(StatusCodes.Status404NotFound);
    }

    [Fact]
    public void ToProblemDetails_ValidationWithMetadata_StoresMetadataUnderExtensionKey()
    {
        var options = new ErrorOrAspNetCoreOptions { IncludeMetadataInProblemDetails = true };
        var errors = new List<Error>
        {
            Error.Validation("Email.Invalid", "Bad email.", new Dictionary<string, object> { ["regex"] = @"\w+@\w+" }),
        };

        var pd = (HttpValidationProblemDetails)errors.ToProblemDetails(options);

        pd.Extensions.Should().ContainKey("errorMetadata");
    }
}
