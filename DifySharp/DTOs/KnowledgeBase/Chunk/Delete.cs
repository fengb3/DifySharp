using System.Text.Json.Serialization;

namespace DifySharp.KnowledgeBase.Chunk;

/// <summary>
/// Delete a chunk in document 
/// </summary>
public record Delete
{
    public class ResponseBody
    {
        [JsonPropertyName("result")]
        public string? Result { get; set; } = "success";
    }
}