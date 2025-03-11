using System.Text.Json;
using DifySharp.KnowledgeBase;
using DifySharp.KnowledgeBase.Dataset;
using DifySharp.KnowledgeBase.Document;
using JetBrains.Annotations;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using WebApiClientCore.Parameters;

namespace DifySharp.Test.Apis.KnowledgeBaseApiTest;

public class DocumentApiTestFixture : KnowledgeBaseApiTestFixture
{
    public Dataset             Dataset { get; private set; }
    public KnowledgeBaseClient Client  { get; }

    public DocumentApiTestFixture()
    {
        Client = ServiceProvider.GetRequiredKeyedService<KnowledgeBaseClient>("knowledge");

        // create a dataset
        var uuid = Guid.NewGuid().ToString("N")[..6];
        Dataset =
            Client.PostCreateDatasetAsync(new Create.RequestBody(Name: $"Test Dataset {uuid}"))
                .GetAwaiter()
                .GetResult();
    }

    public override void Dispose()
    {
        Client.DeleteDataset(Dataset.Id).GetAwaiter().GetResult();
        Client.Dispose();
        base.Dispose();
    }
}

[TestSubject(typeof(IDocumentApi))]
public class DocumentApiTest(
    DocumentApiTestFixture   fixture,
    ILogger<DocumentApiTest> logger
) : IClassFixture<DocumentApiTestFixture>
{
    private static List<Document> Documents { get; set; } = [];
    private        Dataset        Dataset   => fixture.Dataset;

    private KnowledgeBaseClient Client => fixture.Client;

    private readonly ProcessRule _defaultProcessRule = new(
        "automatic",
        new Rules(
            [
                new PreProcessingRule(
                    "remove_extra_spaces",
                    true
                ),
                new PreProcessingRule(
                    "remove_urls_emails",
                    true
                )
            ],
            new Segmentation(
                "\n\n",
                1000
            ),
            "paragraph",
            new SubChunkSegmentation(
                "\n\n",
                1000,
                200
            )
        )
    );

    private readonly CreateByText.RetrievalModel _defaultRetrievalModel = new(
        CreateByText.SearchMethod.HybridSearch,
        false,
        new CreateByText.RerankingModel(
            "",
            ""
        ),
        4,
        false,
        0.9f
    );

    [Fact, TestPriority(1)]
    public async Task TestCreateDocumentByText_ShouldHaveDocumentInfoInResponse()
    {
        // Assert.Null(Document);

        var uuid = Guid.NewGuid().ToString("N")[..6];

        var response = await Client.PostCreateDocumentByTextAsync(
            Dataset.Id,
            new CreateByText.RequestBody(
                $"Test Document {uuid}",
                "Test Content",
                IndexingTechnique.Economy,
                DocForm.TextModel,
                "",
                _defaultProcessRule,
                _defaultRetrievalModel,
                "",
                ""
            ));


        Assert.NotNull(response.Document);
        Assert.NotEmpty(response.Document.Id);
        Documents.Add(response.Document);
    }


    [Fact, TestPriority(1)]
    public async Task TestCreateDocumentByFile_shouldHaveDocumentInfoInResponse()
    {
        var uuid = Guid.NewGuid().ToString("N")[..6];

        var tempFileName = $"Test Document {uuid}.md";
        var tempFilePath = Path.Combine(Path.GetTempPath(), tempFileName);
        await File.WriteAllTextAsync(tempFilePath, "Test Content");
        await File.WriteAllTextAsync(tempFilePath, "Test Content2");
        await File.WriteAllTextAsync(tempFilePath, "Test Content3");

        var data = new CreateByFile.Data(
            null,
            IndexingTechnique.Economy,
            DocForm.TextModel,
            "",
            _defaultProcessRule
        );
        var response = await Client.PostCreateDocumentByFileAsync( // tmpFilePath
            Dataset.Id, data, new FormDataFile(tempFilePath));

        Assert.NotNull(response.Document);
        Assert.NotEmpty(response.Document.Id);
        Documents.Add(response.Document);
    }

    [Fact, TestPriority(2)]
    public async Task TestListDocument_ShouldContainsDocumentInDataset()
    {
        var response = await Client.GetDocuments(Dataset.Id);

        var documents = response.Data;

        foreach (var doc in Documents)
        {
            Assert.Contains(documents, d => d.Id == doc.Id);
        }
    }

    [Fact, TestPriority(3)]
    public async Task TestDeleteDocument_ShouldNotContainsDocumentInDataset()
    {
        foreach (var doc in Documents)
        {
            var response = await Client.DeleteDocument(Dataset.Id, doc.Id);
            Assert.Equal("success", response.Result);
        }

        var documents = await Client.GetDocuments(Dataset.Id);

        foreach (var doc in Documents)
        {
            Assert.DoesNotContain(documents.Data, d => d.Id == doc.Id);
        }
    }
}