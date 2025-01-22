namespace DifySharp.KnowledgeBase.Document;

public record Document(
    string          Id,
    int?            Position,
    string?         DataSourceType,
    DataSourceInfo? DataSourceInfo,
    string          DatasetProcessRuleId,
    string?         Name,
    string?         CreatedFrom,
    string?         CreatedBy,
    long?           CreatedAt,
    int?            Tokens,
    string?         IndexingStatus,
    string?         Error,
    bool?           Enabled,
    DateTime?       DisabledAt,
    string?         DisabledBy,
    bool?           Archived,
    string?         DisplayStatus,
    int?            WordCount,
    int?            HitCount,
    string?         DocForm
);

public record DataSourceInfo(
    string UploadFileId
);