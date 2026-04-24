using DifySharp.Cli;
using DifySharp.KnowledgeBase.Chunk;
using JetBrains.Annotations;
using Microsoft.Extensions.Logging;

namespace DifySharp.Test.Apis.CliKnowledgeBaseApiTest;

[TestSubject(typeof(DifyApiClient))]
[TestCaseOrderer("DifySharp.Test.PriorityOrderer", "DifySharp.Test")]
[Collection("CliKnowledgeBase")]
public class CliChunkApiTest(
    CliDocumentTestFixture fixture,
    ILogger<CliChunkApiTest> logger
) : IClassFixture<CliDocumentTestFixture>
{
    private DifyApiClient Client => fixture.Client;
    private KnowledgeBase.Dataset.Dataset Dataset => fixture.Dataset;
    private DifySharp.KnowledgeBase.Document.CreateByText.ResponseBody FixtureDoc => fixture.CreatedDocument;

    private static string? _createdChunkId;

    [Fact, TestPriority(1)]
    public async Task CreateChunk_ShouldReturnCreatedSegment()
    {
        var body = new Create.RequestBody(
            new Create.Segment(Content: "This is a manually created chunk for CLI integration testing.", Answer: null)
        );

        var result = await Client.PostAsync<Create.RequestBody, Create.ResponseBody>(
            $"/v1/datasets/{Dataset.Id}/documents/{FixtureDoc.Document.Id}/segments", body);

        Assert.NotNull(result);
        Assert.NotNull(result.Data);
        Assert.NotEmpty(result.Data);

        var chunk = result.Data.First();
        Assert.NotEmpty(chunk.Id);
        Assert.Equal("This is a manually created chunk for CLI integration testing.", chunk.Content);

        _createdChunkId = chunk.Id;
        logger.LogInformation("Created chunk: {json}", result.ToJson());
    }

    [Fact, TestPriority(2)]
    public async Task ListChunks_ShouldContainCreatedChunk()
    {
        var result = await Client.GetAsync<Get.ResponseBody>(
            $"/v1/datasets/{Dataset.Id}/documents/{FixtureDoc.Document.Id}/segments");

        Assert.NotNull(result);
        Assert.NotNull(result.Data);

        if (_createdChunkId != null)
        {
            Assert.Contains(result.Data, c => c.Id == _createdChunkId);
        }

        logger.LogInformation("Chunks listed: count={count}", result.Data.Count);
    }

    [Fact, TestPriority(3)]
    public async Task CreateMultipleChunks_ShouldReturnAll()
    {
        var body = new Create.RequestBody(
            new Create.Segment(Content: "First chunk content", Answer: null),
            new Create.Segment(Content: "Second chunk content", Answer: null),
            new Create.Segment(Content: "Third chunk content", Answer: "Optional answer")
        );

        var result = await Client.PostAsync<Create.RequestBody, Create.ResponseBody>(
            $"/v1/datasets/{Dataset.Id}/documents/{FixtureDoc.Document.Id}/segments", body);

        Assert.NotNull(result);
        Assert.Equal(3, result.Data.Count);

        logger.LogInformation("Created {count} chunks in batch", result.Data.Count);
    }
}
