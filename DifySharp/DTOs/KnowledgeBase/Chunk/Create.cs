namespace DifySharp.KnowledgeBase.Chunk;

/// <summary>
/// Add chunks to a document
/// </summary>
public record Create
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="Segments">segments</param>
    public record RequestBody(
        ICollection<Segment> Segments
    );

    /// <summary>
    /// 
    /// </summary>
    /// <param name="Content">(text) Text content / question content, required</param>
    /// <param name="Answer">(text) Answer content, if the mode of the knowledge is Q and A mode, pass the value (optional)</param>
    /// <param name="Keywords">(list) Keywords (optional)</param>
    public record Segment(
        string               Content,
        string?              Answer,
        ICollection<string>? Keywords
    );

    public record ResponseBody(
        ICollection<Chunk> Data,
        string             DocForm
    );
}