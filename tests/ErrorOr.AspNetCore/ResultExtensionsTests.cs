using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.Extensions.Options;

namespace Tests.AspNetCore;

public class ResultExtensionsTests
{
    [Fact]
    public void ToResult_SingleError_ReturnsProblemHttpResult()
    {
        var error = Error.NotFound("User.NotFound", "User was not found.");

        var result = error.ToResult();

        var problem = result.Should().BeOfType<ProblemHttpResult>().Subject;
        problem.ProblemDetails.Status.Should().Be(StatusCodes.Status404NotFound);
        problem.ProblemDetails.Title.Should().Be("User.NotFound");
    }

    [Fact]
    public void ToResult_AllValidationErrors_ReturnsValidationProblem()
    {
        var errors = new List<Error>
        {
            Error.Validation("Email.Invalid", "Email is invalid."),
            Error.Validation("Name.Required", "Name is required."),
        };

        var result = errors.ToResult();

        var validationProblem = result.Should().BeOfType<ValidationProblem>().Subject;
        validationProblem.ProblemDetails.Errors.Should().ContainKey("Email.Invalid");
        validationProblem.ProblemDetails.Errors.Should().ContainKey("Name.Required");
    }

    [Fact]
    public void ToResult_MixedErrors_ReturnsProblemHttpResult()
    {
        var errors = new List<Error>
        {
            Error.NotFound(),
            Error.Validation(),
        };

        var result = errors.ToResult();

        result.Should().BeOfType<ProblemHttpResult>();
    }

    [Fact]
    public void ToResult_WithIOptions_ResolvesOptions()
    {
        var options = Options.Create(new ErrorOrAspNetCoreOptions { IncludeMetadataInProblemDetails = true });
        var error = Error.NotFound(metadata: new Dictionary<string, object> { ["hint"] = "check id" });

        var result = error.ToResult(options);

        var problem = result.Should().BeOfType<ProblemHttpResult>().Subject;
        problem.ProblemDetails.Extensions.Should().ContainKey("hint");
    }

    [Fact]
    public void ToResult_WithIOptionsSnapshot_ResolvesOptions()
    {
        var snapshot = new OptionsSnapshotStub(new ErrorOrAspNetCoreOptions { IncludeMetadataInProblemDetails = true });
        var error = Error.NotFound(metadata: new Dictionary<string, object> { ["hint"] = "from-snapshot" });

        var result = error.ToResult(snapshot);

        var problem = result.Should().BeOfType<ProblemHttpResult>().Subject;
        problem.ProblemDetails.Extensions.Should().ContainKey("hint");
    }

    [Fact]
    public void ToResult_ListWithIOptionsSnapshot_ResolvesOptions()
    {
        var snapshot = new OptionsSnapshotStub(new ErrorOrAspNetCoreOptions { IncludeMetadataInProblemDetails = true });
        var errors = new List<Error>
        {
            Error.NotFound(metadata: new Dictionary<string, object> { ["detail"] = "snapshot-list" }),
        };

        var result = errors.ToResult(snapshot);

        var problem = result.Should().BeOfType<ProblemHttpResult>().Subject;
        problem.ProblemDetails.Extensions.Should().ContainKey("detail");
    }

    [Fact]
    public void ToResult_WithIOptionsMonitor_ResolvesCurrentValue()
    {
        var monitor = new OptionsMonitorStub(new ErrorOrAspNetCoreOptions { IncludeMetadataInProblemDetails = true });
        var error = Error.NotFound(metadata: new Dictionary<string, object> { ["hint"] = "from-monitor" });

        var result = error.ToResult(monitor);

        var problem = result.Should().BeOfType<ProblemHttpResult>().Subject;
        problem.ProblemDetails.Extensions.Should().ContainKey("hint");
    }

    [Fact]
    public void ToResult_ListWithIOptionsMonitor_ResolvesCurrentValue()
    {
        var monitor = new OptionsMonitorStub(new ErrorOrAspNetCoreOptions { IncludeMetadataInProblemDetails = true });
        var errors = new List<Error>
        {
            Error.NotFound(metadata: new Dictionary<string, object> { ["detail"] = "monitor-list" }),
        };

        var result = errors.ToResult(monitor);

        var problem = result.Should().BeOfType<ProblemHttpResult>().Subject;
        problem.ProblemDetails.Extensions.Should().ContainKey("detail");
    }

    [Fact]
    public void ToResult_EmptyErrors_ReturnsProblemHttpResult500()
    {
        var result = new List<Error>().ToResult();

        var problem = result.Should().BeOfType<ProblemHttpResult>().Subject;
        problem.ProblemDetails.Status.Should().Be(StatusCodes.Status500InternalServerError);
    }

    private sealed class OptionsSnapshotStub(ErrorOrAspNetCoreOptions value) : IOptionsSnapshot<ErrorOrAspNetCoreOptions>
    {
        public ErrorOrAspNetCoreOptions Value => value;

        public ErrorOrAspNetCoreOptions Get(string? name) => value;
    }

    private sealed class OptionsMonitorStub(ErrorOrAspNetCoreOptions currentValue) : IOptionsMonitor<ErrorOrAspNetCoreOptions>
    {
        public ErrorOrAspNetCoreOptions CurrentValue => currentValue;

        public ErrorOrAspNetCoreOptions Get(string? name) => currentValue;

        public IDisposable? OnChange(Action<ErrorOrAspNetCoreOptions, string?> listener) => null;
    }
}
