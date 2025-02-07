using WebApiClientCore.Attributes;

namespace DifySharp.Apis;

/// <summary>
/// Completion App API
/// The text generation application offers non-session support and is ideal for translation, article writing, summarization AI, and more.
/// </summary>
public interface ICompletionApi
{
    /// <summary>
    /// # Create Completion Message
    /// Send a request to the text generation application.
    /// </summary>
    /// <param name="requestBody"></param>
    /// <returns></returns>
    [HttpPost("/completion-messages")]
    public Task<HttpResponseMessage> PostCompletionMessages(
        [JsonContent] object requestBody
    );

    /// <summary>
    /// # Stop Generate
    /// Only supported in streaming mode.
    /// </summary>
    /// <param name="taskId"></param>
    /// <returns></returns>
    [HttpPost("/completion-messages/taskId/stop")]
    public Task<HttpResponseMessage> PostCompletionsMessagesStop(
        string taskId
    );
}