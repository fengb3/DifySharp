using DifySharp.ApiKey;
using DifySharp.Apis;
using DifySharp.Chat.Application;
using DifySharp.Chat.ChatMessages;
using DifySharp.Chat.Conversations;
using DifySharp.Chat.Messages;
using DifySharp.Completion.CompletionMessages;
using DifySharp.KnowledgeBase.Dataset;
using DifySharp.KnowledgeBase.Document;
using DifySharp.Workflow.Run;
using Microsoft.Extensions.DependencyInjection;
using WebApiClientCore.Parameters;
using Basic = DifySharp.Workflow.Application.Basic;
using Create = DifySharp.KnowledgeBase.Dataset.Create;
using Delete = DifySharp.KnowledgeBase.Document.Delete;
using Get = DifySharp.KnowledgeBase.Dataset.Get;
using Parameters = DifySharp.Completion.Application.Parameters;
using PostFeedback = DifySharp.Completion.Messages.PostFeedback;
using Stop = DifySharp.Chat.ChatMessages.Stop;

// ReSharper disable InconsistentNaming

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
    internal KnowledgeBaseClient(DifyClient<IKnowledgeBaseApi> client) : base(client)
    {
    }

    /// <summary>
    /// Create a instance of Dify <see cref="KnowledgeBaseClient"/> with the given api key and base url.
    /// </summary>
    /// <param name="apiKey">api key for dify knowledge base</param>
    /// <param name="baseUrl">base url of a dify server, default value: https://api.dify.ai/v1</param>
    public KnowledgeBaseClient(string apiKey, string baseUrl = "https://api.dify.ai/v1") : base(
        new DifyClient<IKnowledgeBaseApi>(new DifyApiSecret(apiKey), baseUrl))
    {
    }

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

    public async Task<CreateByText.ResponseBody> PostCreateDocumentByTextAsync(
        string                   datasetId,
        CreateByText.RequestBody body)
    {
        return await Api.PostCreateDocumentByTextAsync(datasetId, body);
    }

    public async Task<CreateByFile.ResponseBody> PostCreateDocumentByFileAsync(
        string            datasetId,
        CreateByFile.Data data,
        FormDataFile      file)
    {
        return await Api.PostCreateDocumentByFileAsync(datasetId, data, file);
    }

    public async Task<UpdateByText.ResponseBody> PostUpdateDocumentByTextAsync(
        string                   datasetId,
        string                   documentId,
        UpdateByText.RequestBody body
    ) => await Api.PostUpdateDocumentByTextAsync(datasetId, documentId, body);

    public async Task<UpdateByFile.ResponseBody> PostUpdateDocumentByFileAsync(string datasetId, string documentId,
        UpdateByFile.RequestBody                                                      body)
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
        int                                                                        limit = 20)
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
        string                                                                       segmentId)
    {
        return await Api.DeleteSegments(datasetId, documentId, segmentId);
    }

    public async Task<HttpResponseMessage> PostUpdateSegment(string datasetId, string documentId, string segmentId,
        object                                                      body)
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

    /// <summary>
    /// Create a instance of Dify <see cref="CompletionClient"/> with the given api key and base url.
    /// </summary>
    /// <param name="apiKey">api key for dify completion application</param>
    /// <param name="baseUrl">base url of a dify server, default value: https://api.dify.ai/v1</param>
    public CompletionClient(string apiKey, string baseUrl = "https://api.dify.ai/v1") : base(
        new DifyClient<ICompletionApi>(new DifyApiSecret(apiKey), baseUrl))
    {
    }


    #region Api Calling

    /// <inheritdoc />
    public async Task<Completion.Application.Basic.ResponseBody> GetInfo()
    {
        return await Api.GetInfo();
    }

    /// <inheritdoc />
    public async Task<Parameters.ResponseBody> GetParameters()
    {
        return await Api.GetParameters();
    }

    /// <inheritdoc />
    public async Task<HttpResponseMessage> PostCompletionMessages(CompletionMessages.RequestBody requestBody)
    {
        return await Api.PostCompletionMessages(requestBody);
    }

    /// <inheritdoc />
    public async Task<Completion.CompletionMessages.Stop.ResponseBody> PostCompletionsMessagesStop(string taskId)
    {
        return await Api.PostCompletionsMessagesStop(taskId);
    }

    /// <inheritdoc />
    public async Task<PostFeedback.ResponseBody> PostMessagesFeedbacks(string messageId,
        PostFeedback.RequestBody                                              requestBody)
    {
        return await Api.PostMessagesFeedbacks(messageId, requestBody);
    }

    /// <inheritdoc />
    public async Task<HttpResponseMessage> PostFilesUpload(string user, FileInfo file)
    {
        return await Api.PostFilesUpload(user, file);
    }

    /// <inheritdoc />
    public async Task<HttpResponseMessage> PostTextToAudio(object requestBody)
    {
        return await Api.PostTextToAudio(requestBody);
    }

    #endregion
}

public class ChatClient : DifyClientProxy<IChatApi>, IChatApi
{
    internal ChatClient(DifyClient<IChatApi> client) : base(client)
    {
    }

    /// <summary>
    /// Create a instance of Dify <see cref="ChatClient"/> with the given api key and base url.
    /// </summary>
    /// <param name="apiKey">api key for dify chat application</param>
    /// <param name="baseUrl">base url of a dify server, default value: https://api.dify.ai/v1</param>
    public ChatClient(string apiKey, string baseUrl = "https://api.dify.ai/v1") : base(
        new DifyClient<IChatApi>(new DifyApiSecret(apiKey), baseUrl))
    {
    }

    #region ApiCalling

    /// <inheritdoc />
    public async Task<Chat.Application.Basic.ResponseBody> GetInfo()
    {
        return await Api.GetInfo();
    }

    /// <inheritdoc />
    public async Task<Chat.Application.Parameters.ResponseBody> GetParameters()
    {
        return await Api.GetParameters();
    }

    /// <inheritdoc />
    public async Task<Meta.ResponseBody> GetMeta()
    {
        return await Api.GetMeta();
    }

    /// <inheritdoc />
    public async Task<Chat.Conversations.Get.ResponseBody> GetConversations(
        string  user,
        string? last_id = null,
        int?    limit   = null,
        string? sort_by = null)
    {
        return await Api.GetConversations(user, last_id, limit, sort_by);
    }

    /// <inheritdoc />
    public async Task<Chat.Conversations.Delete.RequestBody> DeleteConversations(string conversationId,
        Chat.Conversations.Delete.RequestBody                                           requestBody)
    {
        return await Api.DeleteConversations(conversationId, requestBody);
    }

    /// <inheritdoc />
    public async Task<Conversation> PostRenameConversation(string conversationId, Rename.RequestBody requestBody)
    {
        return await Api.PostRenameConversation(conversationId, requestBody);
    }

    /// <inheritdoc />
    public async Task<HttpResponseMessage> PostChatMessages(ChatMessage.RequestBody requestBody)
    {
        return await Api.PostChatMessages(requestBody);
    }

    /// <inheritdoc />
    public async Task<Stop.ResponseBody> PostChatMessagesStop(string taskId, Stop.RequestBody requestBody)
    {
        return await Api.PostChatMessagesStop(taskId, requestBody);
    }

    /// <inheritdoc />
    public async Task<Chat.Messages.PostFeedback.ResponseBody> PostMessagesFeedbacks(string messageId,
        Chat.Messages.PostFeedback.RequestBody                                              requestBody)
    {
        return await Api.PostMessagesFeedbacks(messageId, requestBody);
    }

    /// <inheritdoc />
    public async Task<GetSuggested.ResponseBody> GetMessagesSuggested(string messageId, string user)
    {
        return await Api.GetMessagesSuggested(messageId, user);
    }

    /// <inheritdoc />
    public async Task<Chat.Messages.Get.ResponseBody> GetMessages(string conversation_id, string user, string first_id,
        int                                                              limit)
    {
        return await Api.GetMessages(conversation_id, user, first_id, limit);
    }

    /// <inheritdoc />
    public async Task<HttpResponseMessage> PostFilesUpload(string user, FileInfo file)
    {
        return await Api.PostFilesUpload(user, file);
    }

    /// <inheritdoc />
    public async Task<HttpResponseMessage> PostAudioToText(FileInfo file, string user)
    {
        return await Api.PostAudioToText(file, user);
    }

    /// <inheritdoc />
    public async Task<HttpResponseMessage> PostTextToAudio(object requestBody)
    {
        return await Api.PostTextToAudio(requestBody);
    }

    #endregion
}

public class WorkflowClient : DifyClientProxy<IWorkflowApi>, IWorkflowApi
{
    internal WorkflowClient(DifyClient<IWorkflowApi> client) : base(client)
    {
    }

    /// <summary>
    /// Create a instance of Dify <see cref="WorkflowClient"/> with the given api key and base url.
    /// </summary>
    /// <param name="apiKey">api key for dify chat application</param>
    /// <param name="baseUrl">base url of a dify server, default value: https://api.dify.ai/v1</param>
    public WorkflowClient(string apiKey, string baseUrl = "https://api.dify.ai/v1") : base(
        new DifyClient<IWorkflowApi>(new DifyApiSecret(apiKey), baseUrl))
    {
    }

    public async Task<Basic.ResponseBody> GetInfo()
    {
        return await Api.GetInfo();
    }

    #region Api Calling

    /// <inheritdoc />
    public async Task<Workflow.Application.Parameters.ResponseBody> GetParameters() =>
        await Api.GetParameters();

    /// <inheritdoc />
    public async Task<HttpResponseMessage> PostWorkflowsRun(Run.RequestBody requestBody) =>
        await Api.PostWorkflowsRun(requestBody);

    /// <inheritdoc />
    public async Task<Run.Data> GetWorkflowsRun(string workflowId) => await Api.GetWorkflowsRun(workflowId);

    /// <inheritdoc />
    public async Task<HttpResponseMessage> PostWorkflowsTasksStop(string taskId) =>
        await Api.PostWorkflowsTasksStop(taskId);

    /// <inheritdoc />
    public async Task<HttpResponseMessage> GetWorkflowsLogs(string keyword, string status, int page, int limit) =>
        await Api.GetWorkflowsLogs(keyword, status, page, limit);

    /// <inheritdoc />
    public async Task<HttpResponseMessage> PostFilesUpload(string user, FileInfo file) =>
        await Api.PostFilesUpload(user, file);

    #endregion
}