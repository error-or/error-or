using System.Net;
using System.Net.Http.Json;
using System.Text.Json;

namespace Tests.AspNetCore.Integration.Tests;

public class MvcIntegrationTests(TestWebApplicationFactory factory)
    : IClassFixture<TestWebApplicationFactory>
{
    private readonly HttpClient _client = factory.CreateClient();

    [Fact]
    public async Task NotFound_Returns404WithTraceId()
    {
        var response = await _client.GetAsync("/mvc/not-found");

        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        response.Content.Headers.ContentType!.MediaType.Should().Be("application/problem+json");
        var json = await response.Content.ReadFromJsonAsync<JsonElement>();
        json.GetProperty("status").GetInt32().Should().Be(404);

        // DefaultProblemDetailsFactory injects traceId into the response.
        json.TryGetProperty("traceId", out _).Should().BeTrue();
    }

    [Fact]
    public async Task ValidationErrors_Returns400WithErrorsDictionaryAndTraceId()
    {
        var response = await _client.GetAsync("/mvc/validation");

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        var json = await response.Content.ReadFromJsonAsync<JsonElement>();
        var errors = json.GetProperty("errors");
        errors.TryGetProperty("Email.Invalid", out _).Should().BeTrue();
        errors.TryGetProperty("Name.Required", out _).Should().BeTrue();

        // DefaultProblemDetailsFactory injects traceId even for validation responses.
        json.TryGetProperty("traceId", out _).Should().BeTrue();
    }

    [Fact]
    public async Task MixedErrors_ReturnsFirstErrorStatusWithProblemDetailsAndTraceId()
    {
        var response = await _client.GetAsync("/mvc/mixed-errors");

        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        response.Content.Headers.ContentType!.MediaType.Should().Be("application/problem+json");
        var json = await response.Content.ReadFromJsonAsync<JsonElement>();
        json.GetProperty("status").GetInt32().Should().Be(404);
        json.TryGetProperty("traceId", out _).Should().BeTrue();
    }
}
