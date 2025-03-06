namespace DifySharp.KnowledgeBase.Chunk;

/// <summary>
/// Get chunks form a document 
/// </summary>
public record Get
{
    public record RequestBody;

    public record ResponseBody(
        ICollection<Chunk> Data,
        string             DocForm
    );
}