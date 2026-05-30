using System.Net;
using System.Net.Http.Json;
using System.Text.Json;

namespace Tests.AspNetCore.Integration.Tests;

/// <summary>
/// Integration tests verifying that <c>IOptionsSnapshot</c> and <c>IOptionsMonitor</c> are
/// correctly resolved from DI and that registered options (custom mappers, metadata) are
/// applied end-to-end.
/// </summary>
public class MinimalApiOptionsVariantIntegrationTests(TestWebApplicationFactory factory)
    : IClassFixture<TestWebApplicationFactory>
{
    private readonly HttpClient _client = factory.CreateClient();

    [Fact]
    public async Task Snapshot_NotFound_Returns404WithProblemDetailsContentType()
    {
        var response = await _client.GetAsync("/minimal/snapshot/not-found");

        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        response.Content.Headers.ContentType!.MediaType.Should().Be("application/problem+json");
    }

    [Fact]
    public async Task Snapshot_NotFound_Returns404WithCorrectProblemDetailsJson()
    {
        var response = await _client.GetAsync("/minimal/snapshot/not-found");

        var json = await response.Content.ReadFromJsonAsync<JsonElement>();
        json.GetProperty("status").GetInt32().Should().Be(404);
        json.GetProperty("title").GetString().Should().Be("Item.NotFound");
    }

    [Fact]
    public async Task Snapshot_ValidationErrors_Returns400WithErrorsDictionary()
    {
        var response = await _client.GetAsync("/minimal/snapshot/validation");

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        var json = await response.Content.ReadFromJsonAsync<JsonElement>();
        var errors = json.GetProperty("errors");
        errors.TryGetProperty("Email.Invalid", out _).Should().BeTrue();
        errors.TryGetProperty("Name.Required", out _).Should().BeTrue();
    }

    [Fact]
    public async Task Snapshot_ErrorWithMetadata_IncludesMetadataInExtensions()
    {
        var response = await _client.GetAsync("/minimal/snapshot/metadata");

        var json = await response.Content.ReadFromJsonAsync<JsonElement>();
        json.TryGetProperty("hint", out var hint).Should().BeTrue();
        hint.GetString().Should().Be("check the id");
    }

    [Fact]
    public async Task Snapshot_CustomStatusCodeMapper_ReturnsCustomStatusCode()
    {
        var response = await _client.GetAsync("/minimal/snapshot/custom-mapper");

        ((int)response.StatusCode).Should().Be(StatusCodes.Status418ImATeapot);
    }

    [Fact]
    public async Task Monitor_NotFound_Returns404WithProblemDetailsContentType()
    {
        var response = await _client.GetAsync("/minimal/monitor/not-found");

        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        response.Content.Headers.ContentType!.MediaType.Should().Be("application/problem+json");
    }

    [Fact]
    public async Task Monitor_NotFound_Returns404WithCorrectProblemDetailsJson()
    {
        var response = await _client.GetAsync("/minimal/monitor/not-found");

        var json = await response.Content.ReadFromJsonAsync<JsonElement>();
        json.GetProperty("status").GetInt32().Should().Be(404);
        json.GetProperty("title").GetString().Should().Be("Item.NotFound");
    }

    [Fact]
    public async Task Monitor_ValidationErrors_Returns400WithErrorsDictionary()
    {
        var response = await _client.GetAsync("/minimal/monitor/validation");

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        var json = await response.Content.ReadFromJsonAsync<JsonElement>();
        var errors = json.GetProperty("errors");
        errors.TryGetProperty("Email.Invalid", out _).Should().BeTrue();
        errors.TryGetProperty("Name.Required", out _).Should().BeTrue();
    }

    [Fact]
    public async Task Monitor_ErrorWithMetadata_IncludesMetadataInExtensions()
    {
        var response = await _client.GetAsync("/minimal/monitor/metadata");

        var json = await response.Content.ReadFromJsonAsync<JsonElement>();
        json.TryGetProperty("hint", out var hint).Should().BeTrue();
        hint.GetString().Should().Be("check the id");
    }

    [Fact]
    public async Task Monitor_CustomStatusCodeMapper_ReturnsCustomStatusCode()
    {
        var response = await _client.GetAsync("/minimal/monitor/custom-mapper");

        ((int)response.StatusCode).Should().Be(StatusCodes.Status418ImATeapot);
    }
}
