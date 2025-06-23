using System.Text.Json.Serialization;

namespace DifySharp.KnowledgeBase.Document;

public record Delete
{
    public class ResponseBody
    {
        [JsonPropertyName("result")]
        public string? Result { get; set; } = "success";
    }
}