namespace DifySharp.KnowledgeBase.Document;

public record Get
{
    public record ResponseBody(
        ICollection<Document> Data,
        bool                  HasMore,
        int                   Limit,
        int                   Total,
        int                   Page
    );
}