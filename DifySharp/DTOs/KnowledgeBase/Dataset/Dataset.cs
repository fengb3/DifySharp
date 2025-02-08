namespace DifySharp.KnowledgeBase.Dataset;

public record Dataset(
    string  Id,
    string  Name,
    string? Description,
    string  Provider,
    string  Permission,
    string? DataSourceType,
    string? IndexingTechnique,
    int     AppCount,
    int     DocumentCount,
    int     WordCount,
    string  CreatedBy,
    long    CreatedAt,
    string  UpdatedBy,
    long    UpdatedAt,
    string? EmbeddingModel,
    string? EmbeddingModelProvider,
    bool?   EmbeddingAvailable
);