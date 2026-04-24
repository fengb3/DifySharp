using DifySharp.Cli;
using DifySharp.KnowledgeBase.Dataset;
using JetBrains.Annotations;
using Microsoft.Extensions.Logging;

namespace DifySharp.Test.Apis.CliKnowledgeBaseApiTest;

[TestSubject(typeof(DifyApiClient))]
[TestCaseOrderer("DifySharp.Test.PriorityOrderer", "DifySharp.Test")]
[Collection("CliKnowledgeBase")]
public class CliDatasetApiTest(
    CliDatasetTestFixture fixture,
    ILogger<CliDatasetApiTest> logger
) : IClassFixture<CliDatasetTestFixture>
{
    private DifyApiClient Client => fixture.Client;
    private Dataset FixtureDataset => fixture.CreatedDataset;

    private static string _testDatasetName = $"CLI Test {Guid.NewGuid().ToString("N")[..6]}";
    private static string? _createdDatasetId;

    [Fact, TestPriority(1)]
    public async Task CreateDataset_ShouldReturnDatasetWithId()
    {
        var body = new Create.RequestBody(_testDatasetName, $"Description for {_testDatasetName}");

        var result = await Client.PostAsync<Create.RequestBody, Dataset>("/v1/datasets", body);

        Assert.NotNull(result);
        Assert.NotEmpty(result.Id);
        Assert.Equal(_testDatasetName, result.Name);

        _createdDatasetId = result.Id;
        logger.LogInformation("Created dataset: {json}", result.ToJson());
    }

    [Fact, TestPriority(2)]
    public async Task ListDatasets_ShouldContainCreatedDataset()
    {
        var result = await Client.GetAsync<Get.ResponseBody>("/v1/datasets?page=1&limit=20");

        Assert.NotNull(result);
        Assert.NotNull(result.Data);
        Assert.Contains(result.Data, d => d.Id == FixtureDataset.Id);

        logger.LogInformation("Datasets listed: total={total}, page={page}", result.Total, result.Page);
    }

    [Fact, TestPriority(3)]
    public async Task ListDatasets_WithPagination_ShouldRespectLimit()
    {
        var result = await Client.GetAsync<Get.ResponseBody>("/v1/datasets?page=1&limit=1");

        Assert.NotNull(result);
        Assert.Equal(1, result.Limit);
        Assert.True(result.Data.Count <= 1);

        logger.LogInformation("Paginated datasets: {json}", result.ToJson());
    }

    [Fact, TestPriority(4)]
    public async Task DeleteDataset_ShouldSucceed()
    {
        Assert.NotNull(_createdDatasetId);

        await Client.DeleteAsync($"/v1/datasets/{_createdDatasetId}");

        // Verify it's gone
        var result = await Client.GetAsync<Get.ResponseBody>("/v1/datasets?page=1&limit=100");
        Assert.DoesNotContain(result.Data, d => d.Id == _createdDatasetId);

        logger.LogInformation("Dataset {id} deleted and verified", _createdDatasetId);
    }

    [Fact, TestPriority(5)]
    public async Task DeleteDataset_WithInvalidId_ShouldThrow()
    {
        await Assert.ThrowsAsync<HttpRequestException>(async () =>
        {
            await Client.DeleteAsync("/v1/datasets/nonexistent-id-12345");
        });
    }
}
