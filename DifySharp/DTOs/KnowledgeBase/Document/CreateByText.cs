namespace DifySharp.KnowledgeBase.Document;

public record CreateByText
{
    /// <summary>
    ///  creates a new document through text based on this knowledge.
    /// </summary>
    /// <param name="Name">Document name</param>
    /// <param name="Content">Document content</param>
    /// <param name="IndexTechnique">Index mode</param>
    /// <param name="DocForm">Format of indexed content</param>
    /// <param name="DocLang">In Q and A mode, specify the language of the document, for example: English, Chinese</param>
    /// <param name="ProcessRule"></param>
    /// <param name="RetrievalModel">Retrieval model</param>
    /// <param name="EmbeddingMode">Embedding model name</param>
    /// <param name="EmbeddingModelProvider">Embedding model provider</param>
    public record RequestBody(
        string            Name,
        string?           Text,
        IndexingTechnique IndexingTechnique,
        DocForm           DocForm,
        string            DocLanguage,
        ProcessRule       ProcessRule,
        RetrievalModel    RetrievalModel,
        string            EmbeddingMode,
        string            EmbeddingModelProvider
    );


    /// <summary>
    /// Retrieval model
    /// </summary>
    /// <param name="SearchMethod"></param>
    /// <param name="RerankingEnable">(bool) Whether to enable reranking</param>
    /// <param name="RerankingModel">(object) Rerank model configuration</param>
    /// <param name="TopK">(int) Number of results to return</param>
    /// <param name="ScoreThresholdEnabled">(bool) Whether to enable score threshold</param>
    /// <param name="ScoreThreshold">(float) Score threshold</param>
    public record RetrievalModel(
        SearchMethod   SearchMethod,
        bool           RerankingEnable,
        RerankingModel RerankingModel,
        int            TopK,
        bool           ScoreThresholdEnabled,
        float          ScoreThreshold
    );

    public enum SearchMethod
    {
        /// <summary>
        /// Semantic search
        /// </summary>
        SemanticSearch,

        /// <summary>
        /// Full-text search
        /// </summary>
        FullTextSearch,

        /// <summary>
        /// Hybrid search
        /// </summary>
        HybridSearch
    }

    /// <summary>
    /// Rerank model configuration
    /// </summary>
    /// <param name="RerankingProviderName">(string) Rerank model provider</param>
    /// <param name="RerankingModeName">(string) Rerank model name</param>
    public record RerankingModel(
        string RerankingProviderName,
        string RerankingModeName
    );

    /// <summary>
    /// response body
    /// </summary>
    /// <param name="Document"></param>
    /// <param name="Batch"></param>
    public record ResponseBody(
        Document Document,
        string   Batch
    );
}