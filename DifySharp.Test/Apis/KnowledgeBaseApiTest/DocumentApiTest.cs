using System.Text.Json;
using DifySharp.KnowledgeBase;
using DifySharp.KnowledgeBase.Dataset;
using DifySharp.KnowledgeBase.Document;
using JetBrains.Annotations;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using WebApiClientCore.Parameters;

namespace DifySharp.Test.Apis.KnowledgeBaseApiTest;

public class DocumentApiTestFixture : ApiTestFixture
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
        try
        {
            // 检测是否在CI环境中运行
            var isCI = !string.IsNullOrEmpty(Environment.GetEnvironmentVariable("CI")) ||
                      !string.IsNullOrEmpty(Environment.GetEnvironmentVariable("GITHUB_ACTIONS")) ||
                      !string.IsNullOrEmpty(Environment.GetEnvironmentVariable("AZURE_PIPELINES")) ||
                      !string.IsNullOrEmpty(Environment.GetEnvironmentVariable("JENKINS_URL"));
            
            if (isCI)
            {
                Console.WriteLine($"Detected CI environment. Attempting to clean up dataset {Dataset.Id}...");
            }
            
            // 尝试删除数据集
            Client.DeleteDataset(Dataset.Id).GetAwaiter().GetResult();
            
            if (isCI)
            {
                Console.WriteLine($"Successfully deleted dataset {Dataset.Id} in CI environment.");
            }
        }
        catch (Exception ex)
        {
            // 在CI环境中，删除操作可能因为权限限制而失败
            // 记录错误但不抛出异常，避免影响测试结果
            Console.WriteLine($"Warning: Failed to delete dataset {Dataset.Id} during test cleanup: {ex.Message}");
            
            // 如果是403错误，这通常是权限问题，不应该影响测试
            if (ex.Message.Contains("403") || ex.Message.Contains("FORBIDDEN"))
            {
                Console.WriteLine("This is likely due to API key permissions in CI environment. Continuing with test cleanup.");
                Console.WriteLine("Suggestion: Check if the API key has delete permissions for datasets.");
            }
        }
        finally
        {
            Client.Dispose();
            base.Dispose();
        }
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
            new SubchunkSegmentation(
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
                Name: $"Test Document {uuid}",
                Text: "Test Content",
                DocType: null,
                DocMetadata: null,
                IndexingTechnique: IndexingTechnique.Economy,
                DocForm: DocForm.TextModel,
                DocLanguage: "",
                ProcessRule: _defaultProcessRule,
                RetrievalModel: _defaultRetrievalModel,
                EmbeddingModel: "",
                EmbeddingModelProvider: ""
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
            Assert.True(response.IsSuccessStatusCode);
        }

        var documents = await Client.GetDocuments(Dataset.Id);

        foreach (var doc in Documents)
        {
            Assert.DoesNotContain(documents.Data, d => d.Id == doc.Id);
        }
    }
}