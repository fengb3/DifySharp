using WebApiClientCore;
using WebApiClientCore.Attributes;
using WebApiClientCore.Parameters;

namespace DifySharp.Apis;

public interface IApplicationAppApi : ICompletionsAppApi, IChatAppApi
{
	[HttpPost("files/upload")]
	public Task<HttpResponseMessage> PostFilesUpload(
		[FormDataContent] string user,
		FileInfo                 file
	);

	[HttpPost("/messages/{messageId}/feedbacks")]
	public Task<HttpResponseMessage> PostMessagesFeedbacks(
		string               messageId,
		[JsonContent] object requestBody
	);

	[HttpPost("/text-to-audio")]
	public Task<HttpResponseMessage> PostTextToAudio(
		[JsonContent] object requestBody
	);

	[HttpGet("/info")]
	public Task<HttpResponseMessage> GetInfo();

	[HttpGet("/parameters")]
	public Task<HttpResponseMessage> GetParameters();

	[HttpGet("/meta")]
	public Task<HttpResponseMessage> GetMeta();
}

/// <summary>
/// Completion App API
/// The text generation application offers non-session support and is ideal for translation, article writing, summarization AI, and more.
/// </summary>
public interface ICompletionsAppApi
{
	[HttpPost("/completion-messages")]
	public Task<HttpResponseMessage> PostCompletionMessages(
		[JsonContent] object requestBody
	);

	[HttpPost("/completion-messages/taskId/stop")]
	public Task<HttpResponseMessage> PostCompletionsMessagesStop(
		string taskId
	);
}

/// <summary>
/// Chat App API
/// Chat applications support session persistence, allowing previous chat history to be used as context for responses. This can be applicable for chatbot, customer service AI, etc.
/// </summary>
public interface IChatAppApi
{
	[HttpPost("/chat-messages")]
	public Task<HttpResponseMessage> PostChatMessages(
		[JsonContent] object requestBody
	);

	[HttpPost("/chat-messages/{taskId}/stop")]
	public Task<HttpResponseMessage> PostChatMessagesStop(
		string taskId,
		object requestBody
	);


	[HttpGet("/messages/{messageId}/suggested")]
	public Task<HttpResponseMessage> GetMessagesSuggested(
		string             messageId,
		[PathQuery] string user
	);

	[HttpGet("/messages")]
	public Task<HttpResponseMessage> GetMessages(
		[PathQuery, AliasAs("conversation_id")]
		string conversationId,
		[PathQuery]                      string user,
		[PathQuery, AliasAs("first_id")] string firstId,
		[PathQuery]                      int    limit
	);

	[HttpGet("/conversations")]
	public Task<HttpResponseMessage> GetConversations(
		[PathQuery]                     string user,
		[PathQuery, AliasAs("last_id")] string lastId,
		[PathQuery]                     int    limit,
		[PathQuery, AliasAs("sort_by")] string sortBy // created_at, -created_at, updated_at, -updated_at
	);

	[HttpDelete("/conversations/{conversationId}")]
	public Task<HttpResponseMessage> DeleteConversations(
		string conversationId
	);

	[HttpPost("/audio-to-text")]
	public Task<HttpResponseMessage> PostAudioToText(
		[JsonContent] object requestBody
	);
}

public interface IWorkflowAppApi
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