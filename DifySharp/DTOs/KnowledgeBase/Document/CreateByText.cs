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
    /// <param name="DocLang">In Q and mode, specify the language of the document, for example: English, Chinese</param>
    /// <param name="ProcessRule"></param>
    /// <param name="RetrievalModel">Retrieval model</param>
    /// <param name="EmbeddingMode">Embedding model name</param>
    /// <param name="EmbeddingModelProvider">Embedding model provider</param>
    public record RequestBody(
        string         Name,
        string         Content,
        IndexTechnique IndexTechnique,
        DocForm        DocForm,
        string         DocLanguage,
        ProcessRule    ProcessRule,
        RetrievalModel RetrievalModel,
        string         EmbeddingMode,
        string         EmbeddingModelProvider
    );

    // /// <summary>
    // /// Format of indexed content
    // /// </summary>
    // public enum DocForm
    // {
    //     /// <summary>
    //     /// Text documents are directly embedded; <see cref="IndexTechnique.Economy"/> mode defaults to using this form
    //     /// </summary>
    //     TextModel,
    //
    //     /// <summary>
    //     /// Parent-child mode
    //     /// </summary>
    //     HierarchicalMode,
    //
    //     /// <summary>
    //     /// Q and A Mode: Generates Q and A pairs for segmented documents and then embeds the questions
    //     /// </summary>
    //     QaModel,
    // }

    // /// <summary>
    // /// 
    // /// </summary>
    // /// <param name="Mode">(string) Cleaning, segmentation mode, automatic / custom</param>
    // /// <param name="Rules">(object) Custom rules (in automatic mode, this field is empty)</param>
    // public record ProcessRule(
    //     string Mode,
    //     Rules  Rules
    // );
    //
    // /// <summary>
    // /// 
    // /// </summary>
    // /// <param name="PreProcessingRules">(array[object]) Preprocessing rules</param>
    // /// <param name="Segmentation">Segmentation rules</param>
    // /// <param name="ParentMode">Retrieval mode of parent chunks:
    // /// <list type="bullet">
    // /// <item>full-doc : full text retrieval</item>
    // /// <item>paragraph : paragraph retrieval</item>
    // /// </list>
    // /// </param>
    // /// <param name="SubChunkSegmentation">(object) Child chunk rules</param>
    // public record Rules(
    //     ICollection<PreProcessingRule> PreProcessingRules,
    //     Segmentation                   Segmentation,
    //     string                         ParentMode,
    //     SubChunkSegmentation           SubChunkSegmentation
    // );
    //
    // /// <summary>
    // /// Preprocessing rule
    // /// </summary>
    // /// <param name="Id">(string) Unique identifier for the preprocessing rule
    // /// <list type="bullet">
    // /// <item>remove_extra_spaces : Replace consecutive spaces, newlines, tabs</item>
    // /// <item>remove_urls_emails : Delete URL, email address</item>
    // /// </list>
    // /// </param>
    // /// <param name="Enabled">(bool) Whether to select this rule or not. If no document ID is passed in, it represents the default value.</param>
    // public record PreProcessingRule(
    //     string Id,
    //     bool   Enabled
    // );
    //
    // /// <summary>
    // /// Segmentation rule
    // /// </summary>
    // /// <param name="Separator">Custom segment identifier, currently only allows one delimiter to be set. Default is \n</param>
    // /// <param name="MaxTokens">Maximum length (token) defaults to 1000</param>
    // public record Segmentation(
    //     string Separator,
    //     int    MaxTokens
    // );
    //
    // /// <summary>
    // /// (object) Child chunk rules
    // /// </summary>
    // /// <param name="Separator">Segmentation identifier. Currently, only one delimiter is allowed. The default is ***</param>
    // /// <param name="MaxTokens">The maximum length (tokens) must be validated to be shorter than the length of the parent chunk</param>
    // /// <param name="ChunkOverlap">Define the overlap between adjacent chunks (optional)</param>
    // public record SubChunkSegmentation(
    //     string Separator,
    //     int    MaxTokens,
    //     int?   ChunkOverlap
    // );

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
    public record RerankingMode(
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
        string                                     Batch
    );
}