using DifySharp.Attributes;
using DifySharp.Chat.Application;
using DifySharp.Chat.ChatMessages;
using DifySharp.Chat.Conversations;
using DifySharp.Chat.Messages;
using WebApiClientCore.Attributes;

namespace DifySharp.Apis
{
    /// <summary>
    /// Chat App API
    /// Chat applications support session persistence, allowing previous chat history to be used as context for responses. This can be applicable for chatbot, customer service AI, etc.
    /// </summary>
    [LoggingFilter, DifyAuth]
    public interface IChatApi : IApplicationApi, IConversationApi, IChatMessageApi, IMessagesApi
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
        /// Speech to Text
        /// <para>This endpoint requires a multipart/form-data request.</para>
        /// </summary>
        /// <param name="file">Audio file. Supported formats: ['mp3', 'mp4', 'mpeg', 'mpga', 'm4a', 'wav', 'webm'] File size limit: 15MB</param>
        /// <param name="user">User identifier, defined by the developer's rules, must be unique within the application.</param>
        /// <returns></returns>
        [HttpPost("/v1/audio-to-text")]
        public Task<HttpResponseMessage> PostAudioToText(
            FileInfo                 file,
            [FormDataContent] string user
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

namespace DifySharp.Chat.ChatMessages
{
    public interface IChatMessageApi
    {
        /// <summary>
        /// # Send Chat Message
        /// send a request to the chat application
        /// </summary>
        /// <param name="requestBody"></param>
        /// <returns></returns>
        [HttpPost("/v1/chat-messages")]
        public Task<HttpResponseMessage> PostChatMessages(
            [JsonContent] ChatMessage.RequestBody requestBody
        );


        /// <summary>
        /// # Stop Generate
        /// Only supported in streaming mode.
        /// </summary>
        /// <param name="taskId"> (string) Task ID, can be obtained from the streaming chunk return</param>
        /// <param name="requestBody">request body</param>
        /// <returns></returns>
        [HttpPost("/v1/chat-messages/{taskId}/stop")]
        public Task<Stop.ResponseBody> PostChatMessagesStop(
            string           taskId,
            Stop.RequestBody requestBody
        );
    }
}

namespace DifySharp.Chat.Messages
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


        /// <summary>
        /// # Next Suggested Questions
        /// Get next questions suggestions for the current message
        /// </summary>
        /// <param name="messageId"></param>
        /// <param name="user"></param>
        /// <returns></returns>
        [HttpGet("/v1/messages/{messageId}/suggested")]
        public Task<GetSuggested.ResponseBody> GetMessagesSuggested(
            string             messageId,
            [PathQuery] string user
        );

        /// <summary>
        /// # Get Conversation History Messages
        /// <para>
        /// Returns historical chat records in a scrolling load format, with the first page returning the latest {limit} messages, i.e., in reverse order.
        /// </para>
        /// </summary>
        /// <param name="conversation_id"></param>
        /// <param name="user"></param>
        /// <param name="first_id"></param>
        /// <param name="limit"></param>
        /// <returns></returns>
        [HttpGet("/v1/messages")]
        public Task<Get.ResponseBody> GetMessages(
            [PathQuery] string conversation_id,
            [PathQuery] string user,
            [PathQuery] string first_id,
            [PathQuery] int    limit
        );
    }
}

namespace DifySharp.Chat.Conversations
{
    public interface IConversationApi
    {
        /// <summary>
        /// # Get Conversations
        /// <para>
        /// Retrieve the conversation list for the current user, defaulting to the most recent 20 entries.
        /// </para>
        /// </summary>
        /// <param name="user">User identifier, used to define the identity of the end-user for retrieval and statistics. Should be uniquely defined by the developer within the application.</param>
        /// <param name="last_id">(Optional) The ID of the last record on the current page, default is null.</param>
        /// <param name="limit">(Optional) How many records to return in one request, default is the most recent 20 entries. Maximum 100, minimum 1.</param>
        /// <param name="sort_by">
        /// (Optional) Sorting Field, Default: -updated_at (sorted in descending order by update time)
        /// <list type="bullet">
        /// <item>
        /// Available Values:
        /// <list type="bullet">
        /// <item>created_at</item>
        /// <item>-created_at</item>
        /// <item>updated_at</item>
        /// <item>-updated_at</item>
        /// </list>
        /// </item>
        /// <item>
        /// The symbol before the field represents the order or reverse, "-" represents reverse order.
        /// </item >
        /// </list>
        /// </param>
        /// <returns></returns>
        [HttpGet("/v1/conversations")]
        public Task<Get.ResponseBody> GetConversations(
            [PathQuery] string  user,
            [PathQuery] string? last_id = null,
            [PathQuery] int?    limit   = null,
            [PathQuery] string? sort_by = null
        );

        /// <summary>
        /// # Delete Conversation
        /// <para>Delete a conversation.</para>
        /// </summary>
        /// <param name="conversationId"></param>
        /// <param name="requestBody"></param>
        /// <returns></returns>
        [HttpDelete("/v1/conversations/{conversationId}")]
        public Task<Delete.RequestBody> DeleteConversations(
            string             conversationId,
            Delete.RequestBody requestBody
        );

        /// <summary>
        /// # Conversation Rename
        /// <para> the session, the session name is used for display on clients that support multiple sessions.</para>
        /// </summary>
        /// <param name="conversationId"></param>
        /// <param name="requestBody"></param>
        /// <returns></returns>
        [HttpPost("/v1/conversations/{conversationId}/name")]
        public Task<Conversations.Conversation> PostRenameConversation(
            string                           conversationId,
            [JsonContent] Rename.RequestBody requestBody
        );
    }
}

namespace DifySharp.Chat.Application
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

        /// <summary>
        /// Get Application Meta Information
        /// <para>Used to get icons of tools in this application</para>
        /// </summary>
        /// <returns></returns>
        [HttpGet("/v1/meta")]
        public Task<Meta.ResponseBody> GetMeta();
    }
}