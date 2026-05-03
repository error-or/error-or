# ErrorOr.AspNetCore

[![NuGet](https://img.shields.io/nuget/v/ErrorOr.AspNetCore.svg)](https://www.nuget.org/packages/ErrorOr.AspNetCore)

ASP.NET Core integration for [ErrorOr](https://github.com/error-or/error-or). Converts `ErrorOr<T>` domain results into RFC 7807 `ProblemDetails` HTTP responses for **MVC controllers** and **Minimal APIs**.

```
dotnet add package ErrorOr.AspNetCore
```

---

## Getting Started

### 1. Register services

```csharp
// Program.cs
builder.Services.AddErrorOrAspNetCore();
```

### 2. MVC controllers

```csharp
using ErrorOr.AspNetCore.Mvc;

[ApiController]
[Route("users")]
public class UsersController : ControllerBase
{
    [HttpGet("{id}")]
    public IActionResult GetUser(Guid id)
    {
        ErrorOr<User> result = _userService.GetUser(id);

        return result.Match(
            user => Ok(user),
            errors => errors.ToActionResult(HttpContext));
    }
}
```

### 3. Minimal APIs

```csharp
using ErrorOr.AspNetCore.Http;

app.MapGet("/users/{id}", (Guid id, IServiceProvider sp) =>
{
    ErrorOr<User> result = userService.GetUser(id);

    return result.Match(
        user => Results.Ok(user),
        errors => errors.ToResult(sp));
});
```

---

## Default Error → HTTP Status Code Mapping

| `ErrorType`   | HTTP Status               |
|---------------|---------------------------|
| `Validation`  | 400 Bad Request           |
| `Unauthorized`| 401 Unauthorized          |
| `Forbidden`   | 403 Forbidden             |
| `NotFound`    | 404 Not Found             |
| `Conflict`    | 409 Conflict              |
| All others    | 500 Internal Server Error |

When all errors in a list are `Validation` errors, a `ValidationProblemDetails` with an `errors` dictionary is returned. Otherwise, the first error determines the response.

---

## Configuration

```csharp
builder.Services.AddErrorOrAspNetCore(opts =>
{
    // Include Error.Metadata in ProblemDetails.Extensions (default: false)
    opts.IncludeMetadataInProblemDetails = true;

    // Custom HTTP status-code mapper — evaluated before built-in defaults.
    // Return null to fall through to the next mapper or the default.
    opts.ErrorToStatusCodeMappers.Add(error =>
        error.NumericType == 999 ? StatusCodes.Status418ImATeapot : null);

    // Custom ProblemDetails mapper — evaluated before built-in defaults.
    // Return null to fall through to the next mapper or the default.
    opts.ErrorsToProblemDetailsMappers.Add(errors =>
        errors.All(e => e.Type == ErrorType.NotFound)
            ? new ProblemDetails { Status = 404, Title = "Resources not found" }
            : null);
});
```

## License

[MIT](https://github.com/error-or/error-or/blob/main/LICENSE)
