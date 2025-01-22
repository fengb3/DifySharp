namespace DifySharp.KnowledgeBase.Document;

public class GetEmbeddingStatus
{
    public record ResponseBody(
        ICollection<Data> Data
    );

    public record Data(
        string    Id,
        string    IndexStatus,
        DateTime? ProcessingStartedAt,
        DateTime? ParsingCompletedAt,
        DateTime? CleaningCompletedAt,
        DateTime? SplittingCompletedAt,
        DateTime? CompletedAt,
        DateTime? PausedAt,
        object?   Error,
        DateTime? StoppedAt,
        int       CompletedSegments,
        int       TotalSegments
    );
}