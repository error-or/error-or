using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Tests.AspNetCore;

public class ServiceCollectionExtensionsTests
{
    [Fact]
    public void AddErrorOrAspNetCore_NoConfiguration_RegistersDefaultOptions()
    {
        var services = new ServiceCollection();
        services.AddErrorOrAspNetCore();
        var provider = services.BuildServiceProvider();

        var options = provider.GetRequiredService<IOptions<ErrorOrAspNetCoreOptions>>().Value;

        options.IncludeMetadataInProblemDetails.Should().BeFalse();
        options.ErrorToStatusCodeMappers.Should().BeEmpty();
        options.ErrorsToProblemDetailsMappers.Should().BeEmpty();
    }

    [Fact]
    public void AddErrorOrAspNetCore_WithConfiguration_AppliesConfigureAction()
    {
        var services = new ServiceCollection();
        services.AddErrorOrAspNetCore(opts =>
        {
            opts.IncludeMetadataInProblemDetails = true;
            opts.ErrorToStatusCodeMappers.Add(_ => 418);
        });
        var provider = services.BuildServiceProvider();

        var options = provider.GetRequiredService<IOptions<ErrorOrAspNetCoreOptions>>().Value;

        options.IncludeMetadataInProblemDetails.Should().BeTrue();
        options.ErrorToStatusCodeMappers.Should().HaveCount(1);
    }

    [Fact]
    public void AddErrorOrAspNetCore_ReturnsOptionsBuilder()
    {
        var services = new ServiceCollection();

        var result = services.AddErrorOrAspNetCore();

        result.Should().BeOfType<OptionsBuilder<ErrorOrAspNetCoreOptions>>();
    }

    [Fact]
    public void AddErrorOrAspNetCore_OptionsBuilderAllowsFurtherConfiguration()
    {
        var services = new ServiceCollection();
        services
            .AddErrorOrAspNetCore()
            .Configure(opts => opts.IncludeMetadataInProblemDetails = true);
        var provider = services.BuildServiceProvider();

        var options = provider.GetRequiredService<IOptions<ErrorOrAspNetCoreOptions>>().Value;

        options.IncludeMetadataInProblemDetails.Should().BeTrue();
    }

    [Fact]
    public void AddErrorOrAspNetCore_RegistersIOptionsSnapshot()
    {
        var services = new ServiceCollection();
        services.AddErrorOrAspNetCore(opts => opts.IncludeMetadataInProblemDetails = true);
        var provider = services.BuildServiceProvider();

        using var scope = provider.CreateScope();
        var snapshot = scope.ServiceProvider.GetRequiredService<IOptionsSnapshot<ErrorOrAspNetCoreOptions>>();

        snapshot.Value.IncludeMetadataInProblemDetails.Should().BeTrue();
    }

    [Fact]
    public void AddErrorOrAspNetCore_RegistersIOptionsMonitor()
    {
        var services = new ServiceCollection();
        services.AddErrorOrAspNetCore(opts => opts.IncludeMetadataInProblemDetails = true);
        var provider = services.BuildServiceProvider();

        var monitor = provider.GetRequiredService<IOptionsMonitor<ErrorOrAspNetCoreOptions>>();

        monitor.CurrentValue.IncludeMetadataInProblemDetails.Should().BeTrue();
    }

    [Fact]
    public void AddErrorOrAspNetCore_NullConfiguration_RegistersDefaultOptions()
    {
        var services = new ServiceCollection();
        services.AddErrorOrAspNetCore(configure: null);
        var provider = services.BuildServiceProvider();

        var options = provider.GetRequiredService<IOptions<ErrorOrAspNetCoreOptions>>().Value;

        options.IncludeMetadataInProblemDetails.Should().BeFalse();
    }

    [Fact]
    public void AddErrorOrAspNetCore_CustomProblemDetailsMapper_IsApplied()
    {
        var customProblemDetails = new ProblemDetails { Status = 999, Title = "Custom" };
        var services = new ServiceCollection();
        services.AddErrorOrAspNetCore(opts =>
            opts.ErrorsToProblemDetailsMappers.Add(_ => customProblemDetails));
        var provider = services.BuildServiceProvider();

        var options = provider.GetRequiredService<IOptions<ErrorOrAspNetCoreOptions>>().Value;
        var errors = new List<Error> { Error.Failure() };
        var result = options.ErrorsToProblemDetailsMappers[0](errors);

        result.Should().BeSameAs(customProblemDetails);
    }
}
