namespace DifySharp.KnowledgeBase.Chunk;

/// <summary>
/// Update a chunk from a document
/// </summary>
public record Update
{
    public record RequestBody(
        Segment Segment
    );

    /// <summary>
    /// 
    /// </summary>
    /// <param name="Content">(text) content / question content, required</param>
    /// <param name="Answer">(text) Answer content, passed if the knowledge is in Q and A mode (optional)</param>
    /// <param name="Keywords">(list) Keyword (optional)</param>
    /// <param name="Enabled">(bool) False / true (optional)</param>
    /// <param name="RegenerateChildChunks">(bool) Whether to regenerate child chunks (optional)</param>
    public record Segment(
        string    Content,
        string?   Answer,
        string[]? Keywords,
        bool?     Enabled,
        bool?     RegenerateChildChunks
    );

    public record ResponseBody(
        List<Chunk> Data,
        string      DocForm
    );
}