using ErrorOr.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc;

namespace Tests.AspNetCore.Integration.Controllers;

[ApiController]
[Route("mvc")]
public class TestController : ControllerBase
{
    [HttpGet("not-found")]
    public IActionResult GetNotFound()
    {
        var error = Error.NotFound("Item.NotFound", "Item was not found.");
        return error.ToActionResult(HttpContext);
    }

    [HttpGet("validation")]
    public IActionResult GetValidation()
    {
        var errors = new List<Error>
        {
            Error.Validation("Email.Invalid", "Email is not valid."),
            Error.Validation("Name.Required", "Name is required."),
        };
        return errors.ToActionResult(HttpContext);
    }

    [HttpGet("mixed-errors")]
    public IActionResult GetMixedErrors()
    {
        // Mix of non-validation errors — exercises the non-validation list branch
        // that flows through ProblemDetailsFactory.CreateProblemDetails.
        var errors = new List<Error>
        {
            Error.NotFound("Item.NotFound", "Item was not found."),
            Error.Failure("Item.Failed", "Processing failed."),
        };
        return errors.ToActionResult(HttpContext);
    }
}
