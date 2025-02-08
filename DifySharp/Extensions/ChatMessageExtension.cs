using System.Diagnostics;
using System.Net.Http.Json;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Serialization;
using DifySharp.Apis;
using DifySharp.Chat.ChatMessages;
using Microsoft.Extensions.Logging;

namespace DifySharp.Extensions;

public static class ChatMessageExtension
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
    public static async Task<ChatMessage.ChatCompletionResponseBody> PostChatMessageBlocking(
        this IChatApi           chatApi,
        ChatMessage.RequestBody requestBody)
    {
        if (requestBody.ResponseMode != ChatMessage.ResponseMode.Blocking)
            throw new ArgumentException(
                $"ResponseMode must be Blocking when calling {nameof(PostChatMessageBlocking)}");

        var httpResponseMessage = await chatApi.PostChatMessages(requestBody);

        if (!httpResponseMessage.IsSuccessStatusCode)
        {
            var msg = await httpResponseMessage.Content.ReadAsStringAsync();

            throw new HttpRequestException(msg);
        }

        var result =
            await httpResponseMessage.Content.ReadFromJsonAsync<ChatMessage.ChatCompletionResponseBody>(JsonOptions);

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
    public static async IAsyncEnumerable<ChatMessage.ChunkChatCompletionResponseBody> PostChatMessageStreaming(
        this IChatApi           api,
        ChatMessage.RequestBody requestBody
    )
    {
        if (requestBody.ResponseMode != ChatMessage.ResponseMode.Streaming)
            throw new ArgumentException(
                $"ResponseMode must be Streaming when calling {nameof(PostChatMessageStreaming)}");

        var httpResponseMessage = await api.PostChatMessages(requestBody);

        if (!httpResponseMessage.IsSuccessStatusCode)
        {
            var msg = await httpResponseMessage.Content.ReadAsStringAsync();

            throw new HttpRequestException(msg);

            yield break;
        }

        var stream = await httpResponseMessage.Content.ReadAsStreamAsync();

        var reader = new StreamReader(stream);

        const string prefix = ChatMessage.ChunkChatCompletionResponseBody.CHUNK_PREFIX;

        while (await reader.ReadLineAsync() is { } line)
        {
            if (!line.StartsWith(prefix)) continue;

            var json = line[prefix.Length..];

            var chunk = JsonSerializer.Deserialize<ChatMessage.ChunkChatCompletionResponseBody>(json, JsonOptions);

            Debug.Assert(chunk != null, nameof(chunk) + " != null");

            chunk.Event = chunk.GetType().Name;

            yield return chunk;
        }
    }
}