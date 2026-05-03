using System.Net;
using System.Net.Http.Json;
using System.Text.Json;

namespace Tests.AspNetCore.Integration.Tests;

public class MinimalApiIntegrationTests(TestWebApplicationFactory factory)
    : IClassFixture<TestWebApplicationFactory>
{
    private readonly HttpClient _client = factory.CreateClient();

    [Fact]
    public async Task NotFound_Returns404WithProblemDetailsContentType()
    {
        var response = await _client.GetAsync("/minimal/not-found");

        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        response.Content.Headers.ContentType!.MediaType.Should().Be("application/problem+json");
    }

    [Fact]
    public async Task NotFound_Returns404WithCorrectProblemDetailsJson()
    {
        var response = await _client.GetAsync("/minimal/not-found");

        var json = await response.Content.ReadFromJsonAsync<JsonElement>();
        json.GetProperty("status").GetInt32().Should().Be(404);
        json.GetProperty("title").GetString().Should().Be("Item.NotFound");
    }

    [Fact]
    public async Task ValidationErrors_Returns400WithErrorsDictionary()
    {
        var response = await _client.GetAsync("/minimal/validation");

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        var json = await response.Content.ReadFromJsonAsync<JsonElement>();
        var errors = json.GetProperty("errors");
        errors.TryGetProperty("Email.Invalid", out _).Should().BeTrue();
        errors.TryGetProperty("Name.Required", out _).Should().BeTrue();
    }

    [Fact]
    public async Task ErrorWithMetadata_IncludesMetadataInExtensions()
    {
        var response = await _client.GetAsync("/minimal/metadata");

        var json = await response.Content.ReadFromJsonAsync<JsonElement>();
        json.TryGetProperty("hint", out var hint).Should().BeTrue();
        hint.GetString().Should().Be("check the id");
    }

    [Fact]
    public async Task CustomStatusCodeMapper_ReturnsCustomStatusCode()
    {
        var response = await _client.GetAsync("/minimal/custom-mapper");

        ((int)response.StatusCode).Should().Be(StatusCodes.Status418ImATeapot);
    }
}
