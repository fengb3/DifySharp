namespace DifySharp.KnowledgeBase.Chunk;

public abstract record Chunk(
    string       Id,
    int          Position,
    string       DocumentId,
    string       Content,
    string       Answer,
    int          WordCount,
    int          Tokens,
    List<string> Keywords,
    string       IndexNodeId,
    string       IndexNodeHash,
    int          HitCount,
    bool         Enabled,
    long?        DisabledAt,
    string?      DisabledBy,
    string       Status,
    string       CreatedBy,
    long         CreatedAt,
    long         IndexingAt,
    long         CompletedAt,
    string?      Error,
    long?        StoppedAt
);