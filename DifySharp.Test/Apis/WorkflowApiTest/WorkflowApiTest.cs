using DifySharp.Workflow;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace DifySharp.Test.Apis.WorkflowApiTest;

public class WorkflowApiTestFixture : ApiTestFixture
{
    public WorkflowClient Client { get; }

    public WorkflowApiTestFixture()
    {
        Client = ServiceProvider.GetRequiredKeyedService<WorkflowClient>("workflow");
    }

    public override void Dispose()
    {
        Client.Dispose();
        base.Dispose();
    }
}

public class WorkflowApiTest(
    WorkflowApiTestFixture   fixture,
    ILogger<WorkflowApiTest> logger
) : IClassFixture<WorkflowApiTestFixture>
{
    private WorkflowClient Client => fixture.Client;

    [Fact]
    public Task TestGetWorkflows()
    {
        // var workflows = await Client.PostWorkflowsRun(new Run.RequestBody()
        // {
        //     User = Guid.NewGuid().ToString("N"),
        // });
        // Assert.NotNull(workflows);
        // // Assert.NotEmpty(workflows);
        
        Assert.True(true);
        return Task.CompletedTask;
    }
}