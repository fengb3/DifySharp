using System.Text.Json.Serialization;

namespace DifySharp.KnowledgeBase;

/// <summary>
/// Retrieval model
/// </summary>
/// <param name="SearchMethod"></param>
/// <param name="RerankingEnabled">(bool) Whether to enable reranking</param>
/// <param name="RerankingMode">(object) Rerank model configuration</param>
/// <param name="TopK">(int) Number of results to return</param>
/// <param name="ScoreThresholdEnable">(bool) Whether to enable score threshold</param>
/// <param name="ScoreThreshold">(float) Score threshold</param>
public record RetrievalModel(
    SearchMethod  SearchMethod,
    bool          RerankingEnabled,
    RerankingMode RerankingMode,
    int           TopK,
    bool          ScoreThresholdEnable,
    float         ScoreThreshold
);

[JsonConverter(typeof(JsonStringEnumConverter<SearchMethod>))]
public enum SearchMethod
{
    [JsonPropertyName("semantic_search")] SemanticSearch,
    [JsonPropertyName("full_text_search")] FullTextSearch,
    [JsonPropertyName("hybrid_search")] HybridSearch
}

/// <summary>
/// Rerank model configuration
/// </summary>
/// <param name="RerankingProviderName">(string) Rerank model provider</param>
/// <param name="RerankingModeName">(string) Rerank model name</param>
public record RerankingMode(
    string RerankingProviderName,
    string RerankingModeName
);