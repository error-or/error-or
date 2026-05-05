using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Tests.AspNetCore.Integration.Endpoints;

namespace Tests.AspNetCore.Integration;

/// <summary>
/// Configures a minimal test web application for integration testing.
/// Overrides <see cref="WebApplicationFactory{TEntryPoint}.CreateHostBuilder"/> to build the host
/// entirely in-process — no <c>Program.cs</c> or web-SDK entry point required.
/// </summary>
public sealed class TestWebApplicationFactory : WebApplicationFactory<TestWebApplicationFactory>
{
    protected override IHostBuilder CreateHostBuilder() => Host.CreateDefaultBuilder()
        .ConfigureWebHostDefaults(webBuilder =>
        {
            webBuilder.ConfigureServices(services =>
            {
                services
                    .AddProblemDetails()
                    .AddControllers()
                    .AddApplicationPart(typeof(TestWebApplicationFactory).Assembly);

                services.AddErrorOrAspNetCore(opts =>
                {
                    opts.IncludeMetadataInProblemDetails = true;
                    opts.ErrorToStatusCodeMappers.Add(error => error.NumericType == 421
                        ? StatusCodes.Status418ImATeapot
                        : null);
                });
            });

            webBuilder.Configure(app =>
            {
                app.UseRouting();
                app.UseEndpoints(endpoints =>
                {
                    endpoints.MapEndpoints();
                    endpoints.MapOptionsVariantEndpoints();
                    endpoints.MapControllers();
                });
            });
        });

    protected override IHost CreateHost(IHostBuilder builder)
    {
        builder.UseContentRoot(AppContext.BaseDirectory);
        return base.CreateHost(builder);
    }
}
