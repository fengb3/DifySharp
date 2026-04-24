using DifySharp.Cli;
using DifySharp.KnowledgeBase;
using DifySharp.KnowledgeBase.Document;
using JetBrains.Annotations;
using Microsoft.Extensions.Logging;

namespace DifySharp.Test.Apis.CliKnowledgeBaseApiTest;

[TestSubject(typeof(DifyApiClient))]
[TestCaseOrderer("DifySharp.Test.PriorityOrderer", "DifySharp.Test")]
[Collection("CliKnowledgeBase")]
public class CliDocumentApiTest(
    CliDocumentTestFixture fixture,
    ILogger<CliDocumentApiTest> logger
) : IClassFixture<CliDocumentTestFixture>
{
    private DifyApiClient Client => fixture.Client;
    private KnowledgeBase.Dataset.Dataset Dataset => fixture.Dataset;
    private CreateByText.ResponseBody FixtureDoc => fixture.CreatedDocument;

    private static string? _testDocId;

    [Fact, TestPriority(1)]
    public async Task CreateDocumentByText_ShouldReturnDocumentAndBatch()
    {
        var uuid = Guid.NewGuid().ToString("N")[..6];
        var body = new CreateByText.RequestBody(
            Name: $"CLI Test Doc {uuid}",
            Text: "Test content for document creation via CLI client.",
            IndexingTechnique: IndexingTechnique.Economy,
            ProcessRule: new ProcessRule(
                Mode: "automatic",
                Rules: new Rules(
                    [new PreProcessingRule("remove_extra_spaces", true)],
                    new Segmentation("\n\n", 1000),
                    "paragraph",
                    new SubchunkSegmentation("\n\n", 1000, 200)
                )
            )
        );

        var result = await Client.PostAsync<CreateByText.RequestBody, CreateByText.ResponseBody>(
            $"/v1/datasets/{Dataset.Id}/document/create_by_text", body);

        Assert.NotNull(result);
        Assert.NotNull(result.Document);
        Assert.NotEmpty(result.Document.Id);
        Assert.NotEmpty(result.Batch);

        _testDocId = result.Document.Id;
        logger.LogInformation("Created document: {json}", result.ToJson());
    }

    [Fact, TestPriority(2)]
    public async Task ListDocuments_ShouldContainCreatedDocuments()
    {
        var result = await Client.GetAsync<DifySharp.KnowledgeBase.Document.Get.ResponseBody>(
            $"/v1/datasets/{Dataset.Id}/documents?page=1&limit=20");

        Assert.NotNull(result);
        Assert.NotNull(result.Data);
        Assert.Contains(result.Data, d => d.Id == FixtureDoc.Document.Id);

        logger.LogInformation("Documents in dataset: total={total}", result.Total);
    }

    [Fact, TestPriority(3)]
    public async Task DeleteDocument_ShouldRemoveFromList()
    {
        Assert.NotNull(_testDocId);

        await Client.DeleteAsync($"/v1/datasets/{Dataset.Id}/documents/{_testDocId}");

        var result = await Client.GetAsync<DifySharp.KnowledgeBase.Document.Get.ResponseBody>(
            $"/v1/datasets/{Dataset.Id}/documents?page=1&limit=100");

        Assert.DoesNotContain(result.Data, d => d.Id == _testDocId);
        logger.LogInformation("Document {id} deleted and verified", _testDocId);
    }

    [Fact, TestPriority(4)]
    public async Task CreateDocumentInNonexistentDataset_ShouldThrow()
    {
        var body = new CreateByText.RequestBody(
            Name: "Invalid Doc",
            Text: "Should fail",
            IndexingTechnique: IndexingTechnique.Economy
        );

        await Assert.ThrowsAsync<HttpRequestException>(async () =>
        {
            await Client.PostAsync<CreateByText.RequestBody, CreateByText.ResponseBody>(
                "/v1/datasets/nonexistent-dataset-id/document/create_by_text", body);
        });
    }
}
