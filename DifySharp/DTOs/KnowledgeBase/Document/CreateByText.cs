using System.Text.Json.Serialization;

namespace DifySharp.KnowledgeBase.Document;

public record CreateByText
{
    /// <summary>
    /// Creates a new document through text based on this knowledge base.
    /// </summary>
    /// <param name="Name">Document name</param>
    /// <param name="Text">Document content</param>
    /// <param name="DocType">Document type (optional)</param>
    /// <param name="DocMetadata">Document metadata (required if doc_type is provided)</param>
    /// <param name="IndexingTechnique">Indexing technique</param>
    /// <param name="DocForm">Format of indexed content</param>
    /// <param name="DocLanguage">Document language for Q&A mode</param>
    /// <param name="ProcessRule">Processing rules</param>
    /// <param name="RetrievalModel">Retrieval model (required when knowledge base has no parameters set for first upload)</param>
    /// <param name="EmbeddingModel">Embedding model name</param>
    /// <param name="EmbeddingModelProvider">Embedding model provider</param>
    public record RequestBody(
        string Name,
        string Text,
        DocType? DocType = null,
        object? DocMetadata = null,
        IndexingTechnique? IndexingTechnique = null,
        DocForm? DocForm = null,
        string? DocLanguage = null,
        ProcessRule? ProcessRule = null,
        RetrievalModel? RetrievalModel = null,
        string? EmbeddingModel = null,
        string? EmbeddingModelProvider = null
    );

    /// <summary>
    /// Document types supported by Dify
    /// </summary>
    public enum DocType
    {
        /// <summary>
        /// Book
        /// </summary>
        [JsonPropertyName("book")]
        Book,

        /// <summary>
        /// Web page
        /// </summary>
        [JsonPropertyName("web_page")]
        WebPage,

        /// <summary>
        /// Academic paper/article
        /// </summary>
        [JsonPropertyName("paper")]
        Paper,

        /// <summary>
        /// Social media post
        /// </summary>
        [JsonPropertyName("social_media_post")]
        SocialMediaPost,

        /// <summary>
        /// Wikipedia entry
        /// </summary>
        [JsonPropertyName("wikipedia_entry")]
        WikipediaEntry,

        /// <summary>
        /// Personal document
        /// </summary>
        [JsonPropertyName("personal_document")]
        PersonalDocument,

        /// <summary>
        /// Business document
        /// </summary>
        [JsonPropertyName("business_document")]
        BusinessDocument,

        /// <summary>
        /// Chat log
        /// </summary>
        [JsonPropertyName("im_chat_log")]
        ImChatLog,

        /// <summary>
        /// Notion synced document
        /// </summary>
        [JsonPropertyName("synced_from_notion")]
        SyncedFromNotion,

        /// <summary>
        /// GitHub synced document
        /// </summary>
        [JsonPropertyName("synced_from_github")]
        SyncedFromGitHub,

        /// <summary>
        /// Other document types
        /// </summary>
        [JsonPropertyName("others")]
        Others
    }

    /// <summary>
    /// Retrieval model configuration
    /// </summary>
    /// <param name="SearchMethod">Search method</param>
    /// <param name="RerankingEnable">Whether to enable reranking</param>
    /// <param name="RerankingModel">Rerank model configuration</param>
    /// <param name="TopK">Number of results to return</param>
    /// <param name="ScoreThresholdEnabled">Whether to enable score threshold</param>
    /// <param name="ScoreThreshold">Score threshold</param>
    public record RetrievalModel(
        SearchMethod SearchMethod,
        bool RerankingEnable,
        RerankingModel? RerankingModel = null,
        int TopK = 10,
        bool ScoreThresholdEnabled = false,
        float ScoreThreshold = 0.5f
    );

    public enum SearchMethod
    {
        /// <summary>
        /// Semantic search
        /// </summary>
        [JsonPropertyName("semantic_search")]
        SemanticSearch,

        /// <summary>
        /// Full-text search
        /// </summary>
        [JsonPropertyName("full_text_search")]
        FullTextSearch,

        /// <summary>
        /// Hybrid search
        /// </summary>
        [JsonPropertyName("hybrid_search")]
        HybridSearch
    }

    /// <summary>
    /// Rerank model configuration
    /// </summary>
    /// <param name="RerankingProviderName">Rerank model provider</param>
    /// <param name="RerankingModelName">Rerank model name</param>
    public record RerankingModel(
        string RerankingProviderName,
        string RerankingModelName
    );

    /// <summary>
    /// Response body for document creation
    /// </summary>
    /// <param name="Document">Created document details</param>
    /// <param name="Batch">Batch identifier</param>
    public record ResponseBody(
        Document Document,
        string Batch
    );
}

/// <summary>
/// Metadata for book documents
/// </summary>
/// <param name="Title">Book title</param>
/// <param name="Language">Book language</param>
/// <param name="Author">Book author</param>
/// <param name="Publisher">Publisher name</param>
/// <param name="PublicationDate">Publication date</param>
/// <param name="Isbn">ISBN number</param>
/// <param name="Category">Book category</param>
public record BookMetadata(
    string? Title = null,
    string? Language = null,
    string? Author = null,
    string? Publisher = null,
    string? PublicationDate = null,
    string? Isbn = null,
    string? Category = null
);

/// <summary>
/// Metadata for web page documents
/// </summary>
/// <param name="Title">Page title</param>
/// <param name="Url">Page URL</param>
/// <param name="Language">Page language</param>
/// <param name="PublishDate">Publish date</param>
/// <param name="AuthorPublisher">Author or publisher</param>
/// <param name="TopicKeywords">Topic or keywords</param>
/// <param name="Description">Page description</param>
public record WebPageMetadata(
    string? Title = null,
    string? Url = null,
    string? Language = null,
    string? PublishDate = null,
    string? AuthorPublisher = null,
    string? TopicKeywords = null,
    string? Description = null
);