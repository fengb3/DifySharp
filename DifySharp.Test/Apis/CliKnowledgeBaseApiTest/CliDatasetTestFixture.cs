using DifySharp.Cli;
using DifySharp.KnowledgeBase.Dataset;

namespace DifySharp.Test.Apis.CliKnowledgeBaseApiTest;

public class CliDatasetTestFixture : IDisposable
{
    public DifyApiClient Client { get; }
    public Dataset CreatedDataset { get; }

    public CliDatasetTestFixture()
    {
        Client = CliTestConstants.CreateClient();

        var uuid = Guid.NewGuid().ToString("N")[..6];
        CreatedDataset = Client
            .PostAsync<Create.RequestBody, Dataset>(
                "/v1/datasets",
                new Create.RequestBody(
                    Name: $"CLI Test Dataset {uuid}",
                    Description: $"Integration test dataset {uuid}"
                ))
            .GetAwaiter().GetResult();
    }

    public void Dispose()
    {
        try
        {
            Client.DeleteAsync($"/v1/datasets/{CreatedDataset.Id}")
                .GetAwaiter().GetResult();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Warning: Failed to delete dataset {CreatedDataset.Id}: {ex.Message}");
        }
        finally
        {
            Client.Dispose();
        }
    }
}
