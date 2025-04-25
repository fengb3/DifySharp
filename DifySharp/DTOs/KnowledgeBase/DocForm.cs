namespace DifySharp.KnowledgeBase;

/// <summary>
/// Format of indexed content
/// </summary>
public enum DocForm
{
    /// <summary>
    /// Text documents are directly embedded; <see cref="IndexingTechnique.Economy"/> mode defaults to using this form
    /// </summary>
    TextModel,

    /// <summary>
    /// Parent-child mode
    /// </summary>
    HierarchicalModel,

    /// <summary>
    /// Q and A Mode: Generates Q and A pairs for segmented documents and then embeds the questions
    /// </summary>
    QaModel,
}