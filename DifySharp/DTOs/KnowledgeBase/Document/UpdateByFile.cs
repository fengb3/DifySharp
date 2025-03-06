namespace DifySharp.KnowledgeBase.Document;

public class UpdateByFile
{
    public record RequestBody(
        Data Data
    );

    /// <summary>
    /// 
    /// </summary>
    /// <param name="Name">Document name (optional)</param>
    /// <param name="Text">Document content (optional)</param>
    /// <param name="ProcessRule">Processing rules</param>
    public record Data(
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