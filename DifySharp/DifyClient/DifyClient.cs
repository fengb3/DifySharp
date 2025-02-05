using DifySharp.ApiKey;
using DifySharp.Apis;
using DifySharp.KnowledgeBase.Chunk;
using DifySharp.KnowledgeBase.Dataset;
using DifySharp.KnowledgeBase.Document;
using Microsoft.Extensions.DependencyInjection;
using Create = DifySharp.KnowledgeBase.Dataset.Create;
using Delete = DifySharp.KnowledgeBase.Document.Delete;
using Get = DifySharp.KnowledgeBase.Dataset.Get;

namespace DifySharp;

internal class DifyClient<T> : IDisposable where T : notnull
{
	private IServiceScope ServiceScope { get; set; }

	private IServiceProvider ServiceProvider => ServiceScope.ServiceProvider;

	public DifyApiSecret DifyApiSecret { get; set; }

	public T Api => ServiceProvider.GetRequiredService<T>();

	public DifyClient(DifyApiSecret difyApiSecret, string baseUrl, IServiceProvider? sp = null)
	{
		DifyApiSecret = difyApiSecret;

		if (sp == null)
		{
			var services = new ServiceCollection();
			ConfigureServices?.Invoke(services);
			services.AddDifySharp(options =>
			{
				options.BaseUrl = baseUrl;
				options.Secrets =
				[
					difyApiSecret
				];
			});
			sp = services.BuildServiceProvider();
		}

		ServiceScope                                                 = sp.CreateScope();
		ServiceProvider.GetRequiredService<IApiKeyProvider>().ApiKey = difyApiSecret.SecretKey;
	}

	private Action<IServiceCollection>? ConfigureServices { get; set; }

	public void Dispose()
	{
		ServiceScope.Dispose();
		GC.SuppressFinalize(this);
	}
}

public abstract class DifyClientProxy<T> : IDisposable where T : notnull
{
	private DifyClient<T> Client { get; set; }

	internal DifyClientProxy(DifyClient<T> client)
	{
		Client = client;
	}

	internal T Api => Client.Api;

	public void Dispose()
	{
		Client.Dispose();
		GC.SuppressFinalize(this);
	}
}

/// <summary>
/// Dify KnowledgeBase Client
/// handles all the api calls to Dify KnowledgeBase
/// </summary>
public class KnowledgeBaseClient : DifyClientProxy<IKnowledgeBaseApi>, IKnowledgeBaseApi
{
	#region Constructor

	internal KnowledgeBaseClient(DifyClient<IKnowledgeBaseApi> client) : base(client)
	{
	}

	/// <summary>
	/// Create a instance of <see cref="KnowledgeBaseClient"/> with the given api key and base url.
	/// </summary>
	/// <param name="apiKey">api key for dify knowledge base</param>
	/// <param name="baseUrl">base url of a dify server, default value: https://api.dify.ai/v1</param>
	public KnowledgeBaseClient(string apiKey, string baseUrl = "https://api.dify.ai/v1") : base(
		new DifyClient<IKnowledgeBaseApi>(new DifyApiSecret(apiKey), baseUrl))
	{
	}

	#endregion

	#region ApiCalling

	public async Task<Dataset> PostCreateDatasetAsync(Create.RequestBody body)
	{
		return await Api.PostCreateDatasetAsync(body);
	}

	public async Task<Get.ResponseBody> GetDatasets(int page = 1, int limit = 20)
	{
		return await Api.GetDatasets(page, limit);
	}

	public async Task DeleteDataset(string datasetId)
	{
		await Api.DeleteDataset(datasetId);
	}

	public async Task<CreateByText.ResponseBody> PostCreateDocumentByTextAsync(string                   datasetId,
	                                                                           CreateByText.RequestBody body)
	{
		return await Api.PostCreateDocumentByTextAsync(datasetId, body);
	}

	public async Task<CreateByFile.ResponseBody> PostCreateDocumentByFileAsync(string                   datasetId,
	                                                                           CreateByFile.RequestBody body,
	                                                                           FileInfo                 file)
	{
		return await Api.PostCreateDocumentByFileAsync(datasetId, body, file);
	}

	public async Task<UpdateByText.ResponseBody> PostUpdateDocumentByTextAsync(
		string                   datasetId,
		string                   documentId,
		UpdateByText.RequestBody body
	) => await Api.PostUpdateDocumentByTextAsync(datasetId, documentId, body);

	public async Task<UpdateByFile.ResponseBody> PostUpdateDocumentByFileAsync(string datasetId, string documentId,
	                                                                           UpdateByFile.RequestBody body)
	{
		return await Api.PostUpdateDocumentByFileAsync(datasetId, documentId, body);
	}

	public async Task<KnowledgeBase.Document.Get.ResponseBody> GetIndexingStatus(string datasetId, int batch)
	{
		return await Api.GetIndexingStatus(datasetId, batch);
	}

	public async Task<Delete.ResponseBody> DeleteDocument(string datasetId, string documentId)
	{
		return await Api.DeleteDocument(datasetId, documentId);
	}

	public async Task<KnowledgeBase.Document.Get.ResponseBody> GetDocuments(string datasetId, int page = 1,
	                                                                        int    limit = 20)
	{
		return await Api.GetDocuments(datasetId, page, limit);
	}

	public async Task<KnowledgeBase.Chunk.Create.ResponseBody> PostCreateSegmentAsync(string datasetId,
		string documentId, KnowledgeBase.Chunk.Create.RequestBody body)
	{
		return await Api.PostCreateSegmentAsync(datasetId, documentId, body);
	}

	public async Task<KnowledgeBase.Chunk.Get.ResponseBody> GetSegments(
		string datasetId,
		string documentId
	)
	{
		return await Api.GetSegments(datasetId, documentId);
	}

	public async Task<KnowledgeBase.Chunk.Delete.ResponseBody> DeleteSegments(string datasetId, string documentId,
	                                                                          string segmentId)
	{
		return await Api.DeleteSegments(datasetId, documentId, segmentId);
	}

	public async Task<HttpResponseMessage> PostUpdateSegment(string datasetId, string documentId, string segmentId,
	                                                         object body)
	{
		return await Api.PostUpdateSegment(datasetId, documentId, segmentId, body);
	}

	public async Task<HttpResponseMessage> GetUpLoadFile(string datasetId, string documentId)
	{
		return await Api.GetUpLoadFile(datasetId, documentId);
	}

	public async Task<HttpResponseMessage> PostRetrieve(string datasetId)
	{
		return await Api.PostRetrieve(datasetId);
	}

	#endregion
}

public class CompletionClient : DifyClientProxy<ICompletionApi>, ICompletionApi
{
	internal CompletionClient(DifyClient<ICompletionApi> client) : base(client)
	{
	}

	#region ApiCalling

	/// <inheritdoc/>
	public async Task<HttpResponseMessage> PostCompletionMessages(object requestBody) =>
		await Api.PostCompletionMessages(requestBody);

	/// <inheritdoc/>
	public async Task<HttpResponseMessage> PostCompletionsMessagesStop(string taskId) =>
		await Api.PostCompletionsMessagesStop(taskId);

	#endregion
}

public class ChatClient : DifyClientProxy<IChatApi>, IChatApi
{
	internal ChatClient(DifyClient<IChatApi> client) : base(client)
	{
	}

	#region ApiCalling

	/// <inheritdoc/>
	public async Task<HttpResponseMessage> PostChatMessages(object requestBody) =>
		await Api.PostChatMessages(requestBody);

	/// <inheritdoc/>
	public async Task<HttpResponseMessage> PostChatMessagesStop(string taskId, object requestBody) =>
		await Api.PostChatMessagesStop(taskId, requestBody);

	/// <inheritdoc/>
	public async Task<HttpResponseMessage> GetMessagesSuggested(string messageId, string user) =>
		await Api.GetMessagesSuggested(messageId, user);

	/// <inheritdoc/>
	public async Task<HttpResponseMessage> GetMessages(string conversationId, string user, string firstId,
	                                                   int    limit) =>
		await Api.GetMessages(conversationId, user, firstId, limit);

	/// <inheritdoc/>
	public async Task<HttpResponseMessage> GetConversations(string user, string lastId, int limit, string sortBy) =>
		await Api.GetConversations(user, lastId, limit, sortBy);

	/// <inheritdoc/>
	public async Task<HttpResponseMessage> DeleteConversations(string conversationId) =>
		await Api.DeleteConversations(conversationId);

	/// <inheritdoc/>
	public async Task<HttpResponseMessage> PostAudioToText(object requestBody) =>
		await Api.PostAudioToText(requestBody);

	#endregion
}

public class WorkflowClient : DifyClientProxy<IWorkflowApi>, IWorkflowApi
{
	internal WorkflowClient(DifyClient<IWorkflowApi> client) : base(client)
	{
	}

	#region ApiCalling

	/// <inheritdoc/>
	public async Task<HttpResponseMessage> PostWorkflowsRun(object requestBody) =>
		await Api.PostWorkflowsRun(requestBody);

	/// <inheritdoc/>
	public async Task<HttpResponseMessage> GetWorkflowsRun(string workflowId) =>
		await Api.GetWorkflowsRun(workflowId);

	/// <inheritdoc/>
	public async Task<HttpResponseMessage> PostWorkflowsTasksStop(string taskId) =>
		await Api.PostWorkflowsTasksStop(taskId);

	/// <inheritdoc/>
	public async Task<HttpResponseMessage> GetWorkflowsLogs(string keyword, string status, int page, int limit) =>
		await Api.GetWorkflowsLogs(keyword, status, page, limit);

	#endregion
}