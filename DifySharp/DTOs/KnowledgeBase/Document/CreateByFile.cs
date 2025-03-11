namespace DifySharp.KnowledgeBase.Document;

public record CreateByFile
{
    public record RequestBody(
        Data Data,
        string File
    );

    /// <summary>
    /// 
    /// </summary>
    /// <param name="OriginalDocumentId"></param>
    /// <param name="IndexingTechnique"></param>
    /// <param name="DocForm"></param>
    /// <param name="DocLanguage"></param>
    /// <param name="ProcessRule"></param>
    public record Data(
        string?           OriginalDocumentId,
        IndexingTechnique IndexingTechnique,
        DocForm           DocForm,
        string            DocLanguage,
        ProcessRule       ProcessRule
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