namespace DifySharp.KnowledgeBase;

public record Retrieve
{
    #region Request

    public record RequestBody(
        string         Query,
        RetrievalModel RetrievalModel
    );

    public record RetrievalModel(
        SearchMethod  SearchMethod,
        bool          RerankingEnable,
        RerankingMode RerankingMode,
        float         Weights,
        int           TopL,
        bool          ScoreThresholdEnabled,
        float         ScoreThreshold
    );

    public enum SearchMethod
    {
        KeywordSearch,
        SemanticSearch,
        FullTextSearch,
        HybridSearch
    }

    public record RerankingMode(
        string RerankingProviderName,
        string RerankingModelName
    );

    #endregion

    #region Response

    public record ResponseBody(
        QueryContent Query,
        List<Record> Records
    );

    public record QueryContent(string Content);

    public record Document(
        string  Id,
        string  DataSourceType,
        string  Name,
        string? DocType
    );

    public record Segment(
        string       Id,
        int          Position,
        string       DocumentId,
        string       Content,
        string?      Answer,
        int          WordCount,
        int          Tokens,
        List<string> Keywords,
        string       IndexNodeId,
        string       IndexNodeHash,
        int          HitCount,
        bool         Enabled,
        DateTime?    DisabledAt,
        string?      DisabledBy,
        string       Status,
        string       CreatedBy,
        DateTime     CreatedAt,
        DateTime     IndexingAt,
        DateTime     CompletedAt,
        string?      Error,
        DateTime?    StoppedAt,
        Document     Document
    );

    public record Record(
        Segment Segment,
        double  Score,
        object? TsnePosition
    );

    #endregion
}