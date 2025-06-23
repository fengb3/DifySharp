using System.Text.Json.Serialization;

namespace DifySharp.KnowledgeBase;

/// <summary>
/// index mode
/// </summary>
public enum IndexingTechnique
{
    /// <summary>
    /// High quality
    /// </summary>
    [JsonPropertyName("high_quality")]
    HighQuality = 0,

    /// <summary>
    /// Economy
    /// </summary>
    [JsonPropertyName("economy")]
    Economy = 1
}