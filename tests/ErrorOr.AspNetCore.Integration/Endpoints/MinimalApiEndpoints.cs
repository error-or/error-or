using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;

namespace Tests.AspNetCore.Integration.Endpoints;

/// <summary>
/// Registers Minimal API test endpoints on <see cref="IEndpointRouteBuilder"/>.
/// </summary>
internal static class MinimalApiEndpoints
{
    internal static void MapEndpoints(this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet("/minimal/not-found", (IServiceProvider sp) =>
        {
            var error = Error.NotFound("Item.NotFound", "Item was not found.");
            return error.ToResult(sp);
        });

        endpoints.MapGet("/minimal/validation", (IServiceProvider sp) =>
        {
            var errors = new List<Error>
            {
                Error.Validation("Email.Invalid", "Email is not valid."),
                Error.Validation("Name.Required", "Name is required."),
            };
            return errors.ToResult(sp);
        });

        endpoints.MapGet("/minimal/metadata", (IServiceProvider sp) =>
        {
            var error = Error.NotFound(
                "Item.NotFound",
                "Not found.",
                new Dictionary<string, object> { ["hint"] = "check the id" });
            return error.ToResult(sp);
        });

        endpoints.MapGet("/minimal/custom-mapper", (IServiceProvider sp) =>
        {
            // NumericType 421 triggers the custom status-code mapper registered in the factory.
            var error = Error.Custom(421, "Custom.Code", "Custom error.");
            return error.ToResult(sp);
        });
    }
}
