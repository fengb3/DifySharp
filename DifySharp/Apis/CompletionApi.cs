using DifySharp.Completion.Application;
using DifySharp.Completion.CompletionMessages;
using DifySharp.Completion.Messages;
using WebApiClientCore.Attributes;

namespace DifySharp.Apis
{
    /// <summary>
    /// Completion App API
    /// The text generation application offers non-session support and is ideal for translation, article writing, summarization AI, and more.
    /// </summary>
    public interface ICompletionApi : IApplicationApi, ICompletionMessagesApi, IMessagesApi
    {
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

        /// <summary>
        /// Text to audio
        /// </summary>
        /// <param name="requestBody"></param>
        /// <returns></returns>
        [HttpPost("/v1/text-to-audio")]
        public Task<HttpResponseMessage> PostTextToAudio(
            [JsonContent] object requestBody
        );
    }
}

namespace DifySharp.Completion.CompletionMessages
{
    public interface ICompletionMessagesApi
    {
        /// <summary>
        /// # Create Completion Message
        /// Send a request to the text generation application.
        /// </summary>
        /// <param name="requestBody"></param>
        /// <returns></returns>
        [HttpPost("/completion-messages")]
        public Task<HttpResponseMessage> PostCompletionMessages(
            [JsonContent] CompletionMessages.RequestBody requestBody
        );

        /// <summary>
        /// # Stop Generate
        /// Only supported in streaming mode.
        /// </summary>
        /// <param name="taskId"></param>
        /// <returns></returns>
        [HttpPost("/completion-messages/taskId/stop")]
        public Task<Stop.ResponseBody> PostCompletionsMessagesStop(string taskId);
    }
}

namespace DifySharp.Completion.Messages
{
    public interface IMessagesApi
    {
        /// <summary>
        /// # Message Feedback
        /// End-users can provide feedback messages, facilitating application developers to optimize expected outputs.
        /// </summary>
        /// <param name="messageId"></param>
        /// <param name="requestBody"></param>
        /// <returns></returns>
        [HttpPost("/v1/messages/{messageId}/feedbacks")]
        public Task<PostFeedback.ResponseBody> PostMessagesFeedbacks(
            string                                 messageId,
            [JsonContent] PostFeedback.RequestBody requestBody
        );
    }
}


namespace DifySharp.Completion.Application
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