namespace DifySharp.KnowledgeBase;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="Mode">(string) Cleaning, segmentation mode, automatic / custom</param>
    /// <param name="Rules">(object) Custom rules (in automatic mode, this field is empty)</param>
    public record ProcessRule(
        string Mode,
        Rules  Rules
    );

    /// <summary>
    /// 
    /// </summary>
    /// <param name="PreProcessingRules">(array[object]) Preprocessing rules</param>
    /// <param name="Segmentation">Segmentation rules</param>
    /// <param name="ParentMode">Retrieval mode of parent chunks:
    /// <list type="bullet">
    /// <item>full-doc : full text retrieval</item>
    /// <item>paragraph : paragraph retrieval</item>
    /// </list>
    /// </param>
    /// <param name="SubChunkSegmentation">(object) Child chunk rules</param>
    public record Rules(
        ICollection<PreProcessingRule> PreProcessingRules,
        Segmentation                   Segmentation,
        string                         ParentMode,
        SubChunkSegmentation           SubChunkSegmentation
    );

    /// <summary>
    /// Preprocessing rule
    /// </summary>
    /// <param name="Id">(string) Unique identifier for the preprocessing rule
    /// <list type="bullet">
    /// <item>remove_extra_spaces : Replace consecutive spaces, newlines, tabs</item>
    /// <item>remove_urls_emails : Delete URL, email address</item>
    /// </list>
    /// </param>
    /// <param name="Enabled">(bool) Whether to select this rule or not. If no document ID is passed in, it represents the default value.</param>
    public record PreProcessingRule(
        string Id,
        bool   Enabled
    );

    /// <summary>
    /// Segmentation rule
    /// </summary>
    /// <param name="Separator">Custom segment identifier, currently only allows one delimiter to be set. Default is \n</param>
    /// <param name="MaxTokens">Maximum length (token) defaults to 1000</param>
    public record Segmentation(
        string Separator,
        int    MaxTokens
    );

    /// <summary>
    /// (object) Child chunk rules
    /// </summary>
    /// <param name="Separator">Segmentation identifier. Currently, only one delimiter is allowed. The default is ***</param>
    /// <param name="MaxTokens">The maximum length (tokens) must be validated to be shorter than the length of the parent chunk</param>
    /// <param name="ChunkOverlap">Define the overlap between adjacent chunks (optional)</param>
    public record SubChunkSegmentation(
        string Separator,
        int    MaxTokens,
        int?   ChunkOverlap
    );
