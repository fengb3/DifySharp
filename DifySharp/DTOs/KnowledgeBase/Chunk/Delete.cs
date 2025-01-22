namespace DifySharp.KnowledgeBase.Chunk;

/// <summary>
/// Delete a chunk in document 
/// </summary>
public record Delete
{
    public record ResponseBody(string Result);
}