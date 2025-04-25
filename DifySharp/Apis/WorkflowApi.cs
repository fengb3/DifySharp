using DifySharp.Workflow;
using DifySharp.Workflow.Application;
using WebApiClientCore.Attributes;

namespace DifySharp.Apis
{
    public interface IWorkflowApi : IApplicationApi
    {
        /// <summary>
        /// 执行 workflow，没有已发布的 workflow，不可执行。
        /// </summary>
        /// <param name="requestBody"></param>
        /// <returns></returns>
        [HttpPost("/workflows/run")]
        public Task<HttpResponseMessage> PostWorkflowsRun(
            [JsonContent] Run.RequestBody requestBody
        );

        [HttpGet("/workflows/run/{workflowId}")]
        public Task<Run.Data> GetWorkflowsRun(
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
        
        /// <summary>
        /// Upload a file (currently only images are supported) for use when sending messages,
        /// enabling multimodal understanding of images and text. Supports png, jpg, jpeg, webp,
        /// gif formats. Uploaded files are for use by the current end-user only.
        /// </summary>
        /// <param name="user"></param>
        /// <param name="file"></param>
        /// <returns></returns>
        [HttpPost("files/upload")]
        public Task<HttpResponseMessage> PostFilesUpload(
            [FormDataContent] string user,
            FileInfo                 file
        );
    }
}


namespace DifySharp.Workflow.Application
{
    public interface IApplicationApi
    {
        /// <summary>
        /// # Get application basic information
        /// <para>Used to get basic information about this application</para>
        /// </summary>
        /// <returns></returns>
        [HttpGet("/v1/info")]
        public Task<Basic.ResponseBody> GetInfo();

        /// <summary>
        /// # Get Application Parameters Information
        /// <para>Used at the start of entering the page to obtain information such as features, input parameter names, types, and default values.</para>
        /// </summary>
        /// <returns></returns>
        [HttpGet("/v1/parameters")]
        public Task<Parameters.ResponseBody> GetParameters(); 
    }
}