using System.Text.Json.Serialization;

namespace DifySharp;

/// <summary>
/// represents the mode of chat api response
/// </summary>
[JsonConverter(typeof(JsonStringEnumConverter))]
public enum ApplicationResponseMode
{
    /// <summary>
    /// Streaming mode (recommended), implements a typewriter-like output through SSE (Server-Sent Events).
    /// </summary>
    Streaming,

    /// <summary>
    /// Blocking mode, returns result after execution is complete.
    /// (Requests may be interrupted if the process is long)
    /// Due to Cloudflare restrictions, the request will be interrupted without a return after 100 seconds.
    /// Note: blocking mode is not supported in Agent Assistant mode
    /// </summary>
    Blocking
}
