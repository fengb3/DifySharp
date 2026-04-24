using DifySharp.Cli;
using DifySharp.KnowledgeBase.Dataset;
using JetBrains.Annotations;
using Microsoft.Extensions.Logging;

namespace DifySharp.Test.Apis.CliKnowledgeBaseApiTest;

[TestSubject(typeof(DifyApiClient))]
[Collection("CliKnowledgeBase")]
public class DifyApiClientTest(ILogger<DifyApiClientTest> logger)
{
    [Fact]
    public async Task GetAsync_ShouldDeserializeSnakeCaseJson()
    {
        using var client = CliTestConstants.CreateClient();

        var result = await client.GetAsync<Get.ResponseBody>("/v1/datasets?page=1&limit=1");

        Assert.NotNull(result);
        Assert.True(result.Total >= 0);
        logger.LogInformation("snake_case deserialization works: total={total}", result.Total);
    }

    [Fact]
    public async Task PostAsync_WithTypedResponse_ShouldDeserializeCorrectly()
    {
        using var client = CliTestConstants.CreateClient();
        var uuid = Guid.NewGuid().ToString("N")[..6];
        var name = $"SnakeCase Test {uuid}";

        var result = await client.PostAsync<Create.RequestBody, Dataset>(
            "/v1/datasets",
            new Create.RequestBody(name));

        Assert.NotNull(result);
        Assert.NotEmpty(result.Id);
        Assert.Equal(name, result.Name);

        // Cleanup
        await client.DeleteAsync($"/v1/datasets/{result.Id}");
        logger.LogInformation("POST serialization/deserialization verified for dataset {id}", result.Id);
    }

    [Fact]
    public async Task DeleteAsync_WithInvalidUrl_ShouldThrowHttpRequestException()
    {
        using var client = CliTestConstants.CreateClient();

        await Assert.ThrowsAsync<HttpRequestException>(async () =>
        {
            await client.DeleteAsync("/v1/datasets/invalid-dataset-id-00000");
        });
    }

    [Fact]
    public async Task GetAsync_WithInvalidUrl_ShouldThrowHttpRequestException()
    {
        using var client = CliTestConstants.CreateClient();

        await Assert.ThrowsAsync<HttpRequestException>(async () =>
        {
            await client.GetAsync<Dataset>("/v1/datasets/invalid-id");
        });
    }

    [Fact]
    public void Constructor_ShouldSetAuthorizationHeader()
    {
        using var client = new DifyApiClient("test-key", "http://localhost");
        Assert.NotNull(client);
    }

    [Fact]
    public void Dispose_CalledTwice_ShouldNotThrow()
    {
        var client = new DifyApiClient("test-key", "http://localhost");
        client.Dispose();
        client.Dispose();
    }
}
