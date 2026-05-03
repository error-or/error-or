using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Options;

namespace Tests.AspNetCore.Integration.Endpoints;

/// <summary>
/// Registers Minimal API test endpoints that exercise the
/// <see cref="IOptionsSnapshot{TOptions}"/> and <see cref="IOptionsMonitor{TOptions}"/> overloads.
/// </summary>
internal static class MinimalApiOptionsVariantEndpoints
{
    internal static void MapOptionsVariantEndpoints(this IEndpointRouteBuilder endpoints)
    {
        // IOptionsSnapshot overloads
        endpoints.MapGet("/minimal/snapshot/not-found", (IOptionsSnapshot<ErrorOrAspNetCoreOptions> options) =>
        {
            var error = Error.NotFound("Item.NotFound", "Item was not found.");
            return error.ToResult(options);
        });

        endpoints.MapGet("/minimal/snapshot/metadata", (IOptionsSnapshot<ErrorOrAspNetCoreOptions> options) =>
        {
            var error = Error.NotFound(
                "Item.NotFound",
                "Not found.",
                new Dictionary<string, object> { ["hint"] = "check the id" });
            return error.ToResult(options);
        });

        endpoints.MapGet("/minimal/snapshot/custom-mapper", (IOptionsSnapshot<ErrorOrAspNetCoreOptions> options) =>
        {
            var error = Error.Custom(421, "Custom.Code", "Custom error.");
            return error.ToResult(options);
        });

        endpoints.MapGet("/minimal/snapshot/validation", (IOptionsSnapshot<ErrorOrAspNetCoreOptions> options) =>
        {
            var errors = new List<Error>
            {
                Error.Validation("Email.Invalid", "Email is not valid."),
                Error.Validation("Name.Required", "Name is required."),
            };
            return errors.ToResult(options);
        });

        endpoints.MapGet("/minimal/monitor/not-found", (IOptionsMonitor<ErrorOrAspNetCoreOptions> monitor) =>
        {
            var error = Error.NotFound("Item.NotFound", "Item was not found.");
            return error.ToResult(monitor);
        });

        endpoints.MapGet("/minimal/monitor/metadata", (IOptionsMonitor<ErrorOrAspNetCoreOptions> monitor) =>
        {
            var error = Error.NotFound(
                "Item.NotFound",
                "Not found.",
                new Dictionary<string, object> { ["hint"] = "check the id" });
            return error.ToResult(monitor);
        });

        endpoints.MapGet("/minimal/monitor/custom-mapper", (IOptionsMonitor<ErrorOrAspNetCoreOptions> monitor) =>
        {
            var error = Error.Custom(421, "Custom.Code", "Custom error.");
            return error.ToResult(monitor);
        });

        endpoints.MapGet("/minimal/monitor/validation", (IOptionsMonitor<ErrorOrAspNetCoreOptions> monitor) =>
        {
            var errors = new List<Error>
            {
                Error.Validation("Email.Invalid", "Email is not valid."),
                Error.Validation("Name.Required", "Name is required."),
            };
            return errors.ToResult(monitor);
        });
    }
}
