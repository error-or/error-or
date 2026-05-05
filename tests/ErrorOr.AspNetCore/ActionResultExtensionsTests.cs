using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Options;

namespace Tests.AspNetCore;

public class ActionResultExtensionsTests
{
    [Fact]
    public void ToActionResult_SingleError_ReturnsObjectResultWithProblemDetails()
    {
        var error = Error.NotFound("User.NotFound", "User was not found.");

        var result = error.ToActionResult();

        var objectResult = result.Should().BeOfType<ObjectResult>().Subject;
        objectResult.StatusCode.Should().Be(StatusCodes.Status404NotFound);
        var problemDetails = objectResult.Value.Should().BeOfType<ProblemDetails>().Subject;
        problemDetails.Title.Should().Be("User.NotFound");
        problemDetails.Detail.Should().Be("User was not found.");
    }

    [Fact]
    public void ToActionResult_AllValidationErrors_ReturnsObjectResultWithValidationProblemDetails()
    {
        var errors = new List<Error>
        {
            Error.Validation("Email.Invalid", "Email is invalid."),
            Error.Validation("Name.Required", "Name is required."),
        };

        var result = errors.ToActionResult();

        var objectResult = result.Should().BeOfType<ObjectResult>().Subject;
        objectResult.StatusCode.Should().Be(StatusCodes.Status400BadRequest);
        var validationProblemDetails = objectResult.Value.Should().BeOfType<HttpValidationProblemDetails>().Subject;
        validationProblemDetails.Errors.Should().ContainKey("Email.Invalid");
        validationProblemDetails.Errors.Should().ContainKey("Name.Required");
    }

    [Fact]
    public void ToActionResult_MixedErrors_ReturnsFirstErrorProblemDetails()
    {
        var errors = new List<Error>
        {
            Error.Conflict("Res.Conflict", "Resource conflict."),
            Error.Validation(),
        };

        var result = errors.ToActionResult();

        var objectResult = result.Should().BeOfType<ObjectResult>().Subject;
        objectResult.StatusCode.Should().Be(StatusCodes.Status409Conflict);
        objectResult.Value.Should().NotBeOfType<HttpValidationProblemDetails>();
    }

    [Fact]
    public void ToActionResult_WithProblemDetailsFactory_UsesFactoryEnrichment()
    {
        var error = Error.Conflict("Res.Conflict", "Resource conflict.");
        var httpContext = BuildHttpContext(new FakeProblemDetailsFactory());

        var result = error.ToActionResult(httpContext);

        var objectResult = result.Should().BeOfType<ObjectResult>().Subject;
        var problemDetails = objectResult.Value.Should().BeOfType<ProblemDetails>().Subject;
        problemDetails.Extensions.Should().ContainKey("factory-enriched");
    }

    [Fact]
    public void ToActionResult_ValidationWithProblemDetailsFactory_PreservesValidationErrors()
    {
        var errors = new List<Error>
        {
            Error.Validation("Email.Invalid", "Email is invalid."),
        };
        var httpContext = BuildHttpContext(new FakeProblemDetailsFactory());

        var result = errors.ToActionResult(httpContext);

        var objectResult = result.Should().BeOfType<ObjectResult>().Subject;
        var validationProblemDetails = objectResult.Value.Should().BeOfType<ValidationProblemDetails>().Subject;
        validationProblemDetails.Errors.Should().ContainKey("Email.Invalid");
        validationProblemDetails.Extensions.Should().ContainKey("factory-enriched");
    }

    [Fact]
    public void ToActionResult_MetadataInExtensions_CopiedToFactoryResult()
    {
        var options = new ErrorOrAspNetCoreOptions { IncludeMetadataInProblemDetails = true };
        var error = Error.Conflict("Res.Conflict", "Conflict.", new Dictionary<string, object> { ["orderId"] = 42 });
        var services = new ServiceProviderStub(new FakeProblemDetailsFactory(), options);
        var httpContext = BuildHttpContext(services);

        var result = error.ToActionResult(httpContext);

        var problemDetails = ((ObjectResult)result).Value.Should().BeOfType<ProblemDetails>().Subject;
        problemDetails.Extensions.Should().ContainKey("orderId");
        problemDetails.Extensions.Should().ContainKey("factory-enriched");
    }

    private static DefaultHttpContext BuildHttpContext(ProblemDetailsFactory factory)
    {
        var services = new ServiceProviderStub(factory, null);
        return BuildHttpContext(services);
    }

    private static DefaultHttpContext BuildHttpContext(IServiceProvider services)
    {
        return new DefaultHttpContext { RequestServices = services };
    }

    private sealed class ServiceProviderStub(
        ProblemDetailsFactory? factory = null,
        ErrorOrAspNetCoreOptions? options = null) : IServiceProvider
    {
        public object? GetService(Type serviceType)
        {
            if (serviceType == typeof(ProblemDetailsFactory))
            {
                return factory;
            }

            if (serviceType == typeof(IOptions<ErrorOrAspNetCoreOptions>))
            {
                return options is not null ? Options.Create(options) : null;
            }

            return null;
        }
    }

    /// <summary>Adds a sentinel "factory-enriched" extension to all created ProblemDetails.</summary>
    private sealed class FakeProblemDetailsFactory : ProblemDetailsFactory
    {
        public override ProblemDetails CreateProblemDetails(
            HttpContext httpContext,
            int? statusCode = null,
            string? title = null,
            string? type = null,
            string? detail = null,
            string? instance = null)
        {
            var problemDetails = new ProblemDetails
            {
                Status = statusCode,
                Title = title,
                Type = type,
                Detail = detail,
                Instance = instance,
            };
            problemDetails.Extensions["factory-enriched"] = true;
            return problemDetails;
        }

        public override ValidationProblemDetails CreateValidationProblemDetails(
            HttpContext httpContext,
            ModelStateDictionary modelStateDictionary,
            int? statusCode = null,
            string? title = null,
            string? type = null,
            string? detail = null,
            string? instance = null)
        {
            var validationProblemDetails = new ValidationProblemDetails(modelStateDictionary)
            {
                Status = statusCode,
                Title = title,
                Type = type,
                Detail = detail,
                Instance = instance,
            };
            validationProblemDetails.Extensions["factory-enriched"] = true;
            return validationProblemDetails;
        }
    }
}
