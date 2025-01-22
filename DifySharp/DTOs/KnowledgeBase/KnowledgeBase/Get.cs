namespace DifySharp.KnowledgeBase.KnowledgeBase;

public class Get
{
    public record ResponseBody(
        ICollection<KnowledgeBase> Data,
        bool                       HasMore,
        int                        Limit,
        int                        Total,
        int                        Page
    );
}