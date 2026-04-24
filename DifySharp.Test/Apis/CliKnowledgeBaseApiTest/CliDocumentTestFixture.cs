using DifySharp.Cli;
using DifySharp.KnowledgeBase;
using DifySharp.KnowledgeBase.Dataset;
using DifySharp.KnowledgeBase.Document;
using DatasetCreate = DifySharp.KnowledgeBase.Dataset.Create;

namespace DifySharp.Test.Apis.CliKnowledgeBaseApiTest;

public class CliDocumentTestFixture : IDisposable
{
    public DifyApiClient Client { get; }
    public Dataset Dataset { get; }
    public CreateByText.ResponseBody CreatedDocument { get; }

    public CliDocumentTestFixture()
    {
        Client = CliTestConstants.CreateClient();
        var uuid = Guid.NewGuid().ToString("N")[..6];

        // Create dataset
        Dataset = Client
            .PostAsync<DatasetCreate.RequestBody, Dataset>(
                "/v1/datasets",
                new DatasetCreate.RequestBody(Name: $"CLI Doc Test Dataset {uuid}"))
            .GetAwaiter().GetResult();

        // Create document by text — indexing_technique and process_rule are required by the API
        CreatedDocument = Client
            .PostAsync<CreateByText.RequestBody, CreateByText.ResponseBody>(
                $"/v1/datasets/{Dataset.Id}/document/create_by_text",
                new CreateByText.RequestBody(
                    Name: $"CLI Test Document {uuid}",
                    Text: "This is a test document for CLI integration tests. " +
                          "It contains multiple sentences. " +
                          "Each sentence provides context for chunk operations.",
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
                ))
            .GetAwaiter().GetResult();
    }

    public void Dispose()
    {
        try
        {
            Client.DeleteAsync($"/v1/datasets/{Dataset.Id}")
                .GetAwaiter().GetResult();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Warning: Failed to delete dataset {Dataset.Id}: {ex.Message}");
        }
        finally
        {
            Client.Dispose();
        }
    }
}
