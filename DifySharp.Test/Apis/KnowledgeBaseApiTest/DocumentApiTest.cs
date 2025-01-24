using DifySharp.Apis;
using DifySharp.KnowledgeBase;
using DifySharp.KnowledgeBase.Dataset;
using DifySharp.KnowledgeBase.Document;
using JetBrains.Annotations;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace DifySharp.Test.Apis.KnowledgeBaseApiTest;

public class DocumentApiTestFixture : KnowledgeBaseApiTestFixture
{
    public Dataset  Dataset  { get; private set; }
    public Document Document { get; set; }

    public DocumentApiTestFixture() : base()
    {
        var api = Services.GetRequiredService<IKnowledgeBaseApi>();

        // create a dataset
        var uuid = Guid.NewGuid().ToString("N")[..6];
        Dataset =
            api.PostCreateDatasetAsync(new Create.RequestBody(Name: $"Test Dataset {uuid}"))
                .GetAwaiter()
                .GetResult();
    }

    public override void Dispose()
    {
        var api = Services.GetRequiredService<IKnowledgeBaseApi>();
        api.DeleteDataset(Dataset.Id).GetAwaiter().GetResult();
        base.Dispose();
    }
}

[TestSubject(typeof(IDocumentApi))]
public class DocumentApiTest(
    DocumentApiTestFixture   fixture,
    ILogger<DocumentApiTest> logger,
    IServiceProvider         serviceProvider
) : IClassFixture<DocumentApiTestFixture>
{
    
    private Document Document
    {
        get => fixture.Document;
        set => fixture.Document = value;
    }

    private Dataset Dataset => fixture.Dataset;

    private IServiceScope NewScope => serviceProvider.CreateScope();

    [Fact, TestPriority(1)]
    public async Task TestCreateDocumentByText_ShouldHaveDocumentInfoInResponse()
    {
        using var scope = NewScope;
        var       api   = scope.ServiceProvider.GetRequiredService<IKnowledgeBaseApi>();
        var       uuid  = Guid.NewGuid().ToString("N")[..6];

        var response = await api.PostCreateDocumentByTextAsync(
            Dataset.Id,
            new CreateByText.RequestBody(
                $"Test Document {uuid}",
                "Test Content",
                IndexingTechnique.Economy,
                DocForm.TextModel,
                "",
                new ProcessRule(
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
                ),
                new CreateByText.RetrievalModel(
                    CreateByText.SearchMethod.HybridSearch,
                    false,
                    new CreateByText.RerankingModel(
                        "",
                        ""
                    ),
                    4,
                    false,
                    0.9f
                ),
                "",
                ""
            ));


        Assert.NotNull(response.Document);
        Assert.NotEmpty(response.Document.Id);
        Document = response.Document;
    }

    [Fact, TestPriority(2)]
    public async Task TestListDocument_ShouldContainsDocumentInDataset()
    {
        using var scope = NewScope;
        var       api   = scope.ServiceProvider.GetRequiredService<IKnowledgeBaseApi>();

        var response = await api.GetDocuments(Dataset
            .Id);

        var documents = response.Data;

        Assert.Contains(documents, doc => doc.Id == Document.Id);
    }

    /// <summary>
    /// 
    /// </summary>
    [Fact, TestPriority(3)]
    public async Task TestDeleteDocument_ShouldNotContainsDocumentInDataset()
    {
        using var scope = NewScope;
        var       api   = scope.ServiceProvider.GetRequiredService<IKnowledgeBaseApi>();

        var response = await api.DeleteDocument(Dataset
            .Id, Document.Id);
        Assert.Equal("success", response.Result);

        var documents = await api.GetDocuments(Dataset
            .Id);
        Assert.DoesNotContain(documents.Data, doc => doc.Id == Document.Id);
    }
}