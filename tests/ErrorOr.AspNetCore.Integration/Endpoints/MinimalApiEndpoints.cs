using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Options;

namespace Tests.AspNetCore.Integration.Endpoints;

/// <summary>
/// Registers Minimal API test endpoints on <see cref="IEndpointRouteBuilder"/>.
/// </summary>
internal static class MinimalApiEndpoints
{
    internal static void MapEndpoints(this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet("/minimal/not-found", (IOptions<ErrorOrAspNetCoreOptions> options) =>
        {
            var error = Error.NotFound("Item.NotFound", "Item was not found.");
            return error.ToResult(options);
        });

        endpoints.MapGet("/minimal/validation", (IOptions<ErrorOrAspNetCoreOptions> options) =>
        {
            var errors = new List<Error>
            {
                Error.Validation("Email.Invalid", "Email is not valid."),
                Error.Validation("Name.Required", "Name is required."),
            };
            return errors.ToResult(options);
        });

        endpoints.MapGet("/minimal/metadata", (IOptions<ErrorOrAspNetCoreOptions> options) =>
        {
            var error = Error.NotFound(
                "Item.NotFound",
                "Not found.",
                new Dictionary<string, object> { ["hint"] = "check the id" });
            return error.ToResult(options);
        });

        endpoints.MapGet("/minimal/custom-mapper", (IOptions<ErrorOrAspNetCoreOptions> options) =>
        {
            // NumericType 421 triggers the custom status-code mapper registered in the factory.
            var error = Error.Custom(421, "Custom.Code", "Custom error.");
            return error.ToResult(options);
        });
    }
}
