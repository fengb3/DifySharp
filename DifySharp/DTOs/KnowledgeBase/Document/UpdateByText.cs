namespace DifySharp.KnowledgeBase.Document;

public record UpdateByText
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="Name">Document name (optional)</param>
    /// <param name="Text">Document content (optional)</param>
    /// <param name="ProcessRule">Processing rules</param>
    public record RequestBody(
        string?      Name,
        string?      Text,
        ProcessRule? ProcessRule
    );

    /// <summary>
    /// response body
    /// </summary>
    /// <param name="Document"></param>
    /// <param name="Batch"></param>
    public record ResponseBody(
        Document Document,
        string   Batch
    );
}