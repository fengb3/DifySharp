using System.Diagnostics;
using System.Net.Http.Json;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Serialization;
using DifySharp.Apis;
using DifySharp.Chat.ChatMessages;
using DifySharp.Workflow;

namespace DifySharp.Extensions;

public static class ApiExtensions
{
    /// <summary>
    /// the JsonSerializerOptions instance for JSON serialization and deserialization.
    /// </summary>
    private static readonly JsonSerializerOptions? JsonOptions = new()
    {
        // WriteIndented          = true,
        Encoder                = JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
        PropertyNamingPolicy   = JsonNamingPolicy.SnakeCaseLower,
        Converters             = { new JsonStringEnumConverter(JsonNamingPolicy.SnakeCaseLower) }
    };

    /// <summary>
    /// Use the blocking response pattern to send a chat message asynchronously and return the chat message to complete the response body.
    /// </summary>
    /// <param name="chatApi"></param>
    /// <param name="requestBody"> the request body of the chat message. </param>
    /// <returns>The chat message completes the  response body. </returns>
    /// <exception cref="ArgumentException"> throws an exception when ResponseMode is not Blocking. </exception>
    public static async Task<ChatMessage.ResponseBody> PostChatMessageBlocking(
        this IChatApi           chatApi,
        ChatMessage.RequestBody requestBody)
    {
        // if (requestBody.ResponseMode != ChatMessage.ResponseMode.Blocking)
        //     throw new ArgumentException(
        //         $"ResponseMode must be Blocking when calling {nameof(PostChatMessageBlocking)}");

        requestBody.ApplicationResponseMode = ApplicationResponseMode.Blocking;

        var httpResponseMessage = await chatApi.PostChatMessages(requestBody);

        if (!httpResponseMessage.IsSuccessStatusCode)
        {
            var msg = await httpResponseMessage.Content.ReadAsStringAsync();

            throw new HttpRequestException(msg);
        }

        var result =
            await httpResponseMessage.Content.ReadFromJsonAsync<ChatMessage.ResponseBody>(JsonOptions);

        Debug.Assert(result != null, nameof(result) + " != null");

        return result;
    }

    /// <summary>
    /// Use the streaming response pattern to send a chat message asynchronously and return the chat message chunked to complete an asynchronous iterative collection of response bodies.
    /// </summary>
    /// <param name="api">IDifyWorkflowApi接口实例</param>
    /// <param name="requestBody">ChatMessageRequestBody</param>
    /// <returns>An asynchronous enumerable that contains a chunked response body for chat messages.</returns>
    /// <exception cref="ArgumentException">Throws an exception when ResponseMode is not Streaming.</exception>
    /// <exception cref="HttpRequestException">Throws an exception when the HTTP response is not successful.</exception>
    public static async IAsyncEnumerable<ChunkCompletionResponseBody> PostChatMessageStreaming(
        this IChatApi           api,
        ChatMessage.RequestBody requestBody
    )
    {
        requestBody.ApplicationResponseMode = ApplicationResponseMode.Streaming;

        var httpResponseMessage = await api.PostChatMessages(requestBody);

        if (!httpResponseMessage.IsSuccessStatusCode)
        {
            var msg = await httpResponseMessage.Content.ReadAsStringAsync();

            throw new HttpRequestException(msg);
        }

        var stream = await httpResponseMessage.Content.ReadAsStreamAsync();

        var reader = new StreamReader(stream);

        const string prefix = ChunkCompletionResponseBody.CHUNK_PREFIX;

        while (await reader.ReadLineAsync() is { } line)
        {
            if (!line.StartsWith(prefix)) continue;

            var json = line[prefix.Length..];

            var chunk = JsonSerializer.Deserialize<ChunkCompletionResponseBody>(json, JsonOptions);

            Debug.Assert(chunk != null, nameof(chunk) + " != null");

            // chunk.Event = chunk.GetType().Name;

            yield return chunk;
        }
    }


    public static async Task<Run.ResponseBody> PostWorkflowsRunBlocking(this IWorkflowApi api,
        Run.RequestBody                                                                  requestBody)
    {
        requestBody.ResponseMode = ApplicationResponseMode.Blocking;

        var httpResponseMessage = await api.PostWorkflowsRun(requestBody);
        
        if (!httpResponseMessage.IsSuccessStatusCode)
        {
            var msg = await httpResponseMessage.Content.ReadAsStringAsync();

            throw new HttpRequestException(msg);
        }
        
        var result =
            await httpResponseMessage.Content.ReadFromJsonAsync<Run.ResponseBody>(JsonOptions);

        Debug.Assert(result != null, nameof(result) + " != null");

        return result;
    }
    
    public static async IAsyncEnumerable<ChunkCompletionResponseBody> PostWorkflowsRunStreaming(this IWorkflowApi api,
        Run.RequestBody                                                                  requestBody)
    {
        requestBody.ResponseMode = ApplicationResponseMode.Streaming;

        var httpResponseMessage = await api.PostWorkflowsRun(requestBody);

        if (!httpResponseMessage.IsSuccessStatusCode)
        {
            var msg = await httpResponseMessage.Content.ReadAsStringAsync();

            throw new HttpRequestException(msg);
        }

        var stream = await httpResponseMessage.Content.ReadAsStreamAsync();

        var reader = new StreamReader(stream);

        const string prefix = ChunkCompletionResponseBody.CHUNK_PREFIX;

        while (await reader.ReadLineAsync() is { } line)
        {
            if (!line.StartsWith(prefix)) continue;

            var json = line[prefix.Length..];

            var chunk = JsonSerializer.Deserialize<ChunkCompletionResponseBody>(json, JsonOptions);

            Debug.Assert(chunk != null, nameof(chunk) + " != null");

            // chunk.Event = chunk.GetType().Name;

            yield return chunk;
        }
    }
}