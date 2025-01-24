
namespace DifySharp.KnowledgeBase.Document;

public record CreateByFile
{
    public record RequestBody(
        Data Data
    );

    /// <summary>
    /// 
    /// </summary>
    /// <param name="OriginalDocumentId"></param>
    /// <param name="IndexTechnique"></param>
    /// <param name="DocForm"></param>
    /// <param name="DocLanguage"></param>
    /// <param name="ProcessRule"></param>
    public record Data(
        string?           OriginalDocumentId,
        IndexingTechnique IndexTechnique,
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
        string                                     Batch
    );
}