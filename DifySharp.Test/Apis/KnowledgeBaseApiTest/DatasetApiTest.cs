using DifySharp.Apis;
using DifySharp.KnowledgeBase.Dataset;
using JetBrains.Annotations;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
// using Xunit.DependencyInjection.Logging;

namespace DifySharp.Test.Apis;

[TestSubject(typeof(IDatasetApi))]
[TestCaseOrderer("DifySharp.Test.TestPriorityOrder", "DifySharp.Test")]
public class DatasetApiTest(IKnowledgeBaseApi api, ILogger<DatasetApiTest> logger)
{
    // public class Startup
    // {
    //     public void ConfigureServices(IServiceCollection services)
    //     {
    //         var knowledgeApiKey = Environment.GetEnvironmentVariable("KNOWLEDGE_BASE_API_KEY");
    //         services.AddDifySdk(option => { option.KnowledgeBaseApiKey = knowledgeApiKey!; });
    //         services.AddLogging(lb => lb.AddXunitOutput());
    //     }
    // }

    private static string  _datasetName = $"Test Dataset {Guid.NewGuid().ToString("N")[..6]}";
    private static string? _datasetId;

    [Fact, TestPriority(1)]
    public async Task TestCreateDataset_ShouldHaveDatasetInResponse()
    {
        var response = await api.PostCreateDatasetAsync(
            new Create.RequestBody(
                _datasetName,
                $"{_datasetName} this is a test dataset created by {GetType().FullName}"
            )
        );

        Assert.NotNull(response);
        Assert.NotNull(response.Id);
        _datasetId = response.Id;

        logger.LogInformation(response.ToJson());
    }

    [Fact, TestPriority(2)]
    public async Task TextListDatasets_ShouldHaveDatasetInResponse()
    {
        var response = await api.GetDatasets();

        Assert.NotNull(_datasetId);
        Assert.Contains(response.Data, d => d.Id == _datasetId);

        logger.LogInformation(response.ToJson());
    }

    [Fact, TestPriority(3)]
    public async Task TestDeleteDataset_ShouldReturn204()
    {
        Assert.NotNull(_datasetId);
        Assert.NotEmpty(_datasetId);

        await api.DeleteDataset(_datasetId);
    }
}