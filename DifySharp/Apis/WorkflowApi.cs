using WebApiClientCore.Attributes;

namespace DifySharp.Apis;

public interface IWorkflowApi
{
    [HttpPost("/workflows/run")]
    public Task<HttpResponseMessage> PostWorkflowsRun(
        [JsonContent] object requestBody
    );

    [HttpGet("/workflows/run/{workflowId}")]
    public Task<HttpResponseMessage> GetWorkflowsRun(
        string workflowId
    );

    [HttpPost("/workflows/tasks/{taskId}/stop")]
    public Task<HttpResponseMessage> PostWorkflowsTasksStop(
        string taskId
    );

    [HttpGet("/workflows/logs")]
    public Task<HttpResponseMessage> GetWorkflowsLogs(
        [PathQuery] string keyword,
        [PathQuery] string status,
        [PathQuery] int    page,
        [PathQuery] int    limit
    );
}