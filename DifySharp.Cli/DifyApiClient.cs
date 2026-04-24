using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Runtime.CompilerServices;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.Json.Serialization.Metadata;

namespace DifySharp.Cli;

public class DifyApiClient : IDisposable
{
    private readonly HttpClient _httpClient;
    private bool _disposed;

    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
        PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower,
        Converters = { new JsonStringEnumConverter(JsonNamingPolicy.SnakeCaseLower) },
        TypeInfoResolver = new DefaultJsonTypeInfoResolver()
    };

    internal static readonly JsonSerializerOptions WriteIndentedOptions = new(JsonOptions)
    {
        WriteIndented = true
    };

    public DifyApiClient(string apiKey, string baseUrl = "https://api.dify.ai/v1")
    {
        _httpClient = new HttpClient
        {
            BaseAddress = new Uri(baseUrl)
        };
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", apiKey);
    }

    public async Task<T> GetAsync<T>(string url, CancellationToken ct = default)
    {
        var response = await _httpClient.GetAsync(url, ct);
        response.EnsureSuccessStatusCode();
        return (await response.Content.ReadFromJsonAsync<T>(JsonOptions, ct))!;
    }

    public async Task<TResponse> PostAsync<TRequest, TResponse>(string url, TRequest body,
        CancellationToken ct = default)
    {
        var response = await _httpClient.PostAsJsonAsync(url, body, JsonOptions, ct);
        response.EnsureSuccessStatusCode();
        return (await response.Content.ReadFromJsonAsync<TResponse>(JsonOptions, ct))!;
    }

    public async Task PostAsync<TRequest>(string url, TRequest body, CancellationToken ct = default)
    {
        var response = await _httpClient.PostAsJsonAsync(url, body, JsonOptions, ct);
        response.EnsureSuccessStatusCode();
    }

    public async Task<HttpResponseMessage> PostRawAsync<TRequest>(string url, TRequest body,
        CancellationToken ct = default)
    {
        var response = await _httpClient.PostAsJsonAsync(url, body, JsonOptions, ct);
        response.EnsureSuccessStatusCode();
        return response;
    }

    public async Task DeleteAsync(string url, CancellationToken ct = default)
    {
        var response = await _httpClient.DeleteAsync(url, ct);
        response.EnsureSuccessStatusCode();
    }

    public async IAsyncEnumerable<ChunkCompletionResponseBody> PostStreamAsync<TRequest>(
        string url, TRequest body, [EnumeratorCancellation] CancellationToken ct = default)
    {
        using var request = new HttpRequestMessage(HttpMethod.Post, url)
        {
            Content = JsonContent.Create(body, null, JsonOptions)
        };
        using var response = await _httpClient.SendAsync(request,
            HttpCompletionOption.ResponseHeadersRead, ct);
        response.EnsureSuccessStatusCode();

        await using var stream = await response.Content.ReadAsStreamAsync(ct);
        using var reader = new StreamReader(stream);

        const string prefix = "data: ";

        while (await reader.ReadLineAsync() is { } line && !ct.IsCancellationRequested)
        {
            if (string.IsNullOrEmpty(line) || !line.StartsWith(prefix)) continue;

            var json = line[prefix.Length..];
            if (json == "[DONE]") break;

            var chunk = JsonSerializer.Deserialize(json, CliJsonContext.Default.ChunkCompletionResponseBody);
            if (chunk != null) yield return chunk;
        }
    }

    public void Dispose()
    {
        if (_disposed) return;
        _disposed = true;
        _httpClient.Dispose();
    }
}
