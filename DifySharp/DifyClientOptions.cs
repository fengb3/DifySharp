namespace DifySharp;

public class DifyClientOptions
{
    /// <summary>
    /// base url of a dify api server, defaults to https://api.dify.ai/v1
    /// </summary>
    public string BaseUrl { get; set; } = "https://api.dify.ai/v1";
    
    /// <summary>
    /// a collection of <see cref="DifyApiSecret"/>, for multiple dify applications and knowledge bases management.
    /// </summary>
    public ICollection<DifyApiSecret> Secrets { get; set; } = new List<DifyApiSecret>();
}

/// <summary>
/// represents a dify api secret
/// </summary>
public class DifyApiSecret(string? secretKey = "", string? name = "", string? apiType = "")
{
    /// <summary>
    /// Represents the API key associated with a specific API. The key is used for authentication and authorization purposes
    /// when making requests to a dify api server.
    /// </summary>
    public string? SecretKey { get; } = secretKey;

    /// <summary>
    /// Specifies the name of the API associated with a particular API key. This property is used to differentiate
    /// between multiple APIs keys, typically when managing multiple dify applications and knowledge bases api keys.
    /// </summary>
    public string? Name { get; } = name;

    /// <summary>
    /// Specifies the type of the API associated with the key.
    /// Can be:
    /// <list type="bullet">
    /// <item>
    /// <term><see cref="DifyApiType.KNOWLEDGE_BASE"/></term>
    /// </item>
    /// <item>
    /// <term><see cref="DifyApiType.COMPLETION"/></term>
    /// </item>
    /// <item>
    /// <term><see cref="DifyApiType.CHAT"/></term>
    /// </item>
    /// <item>
    /// <term><see cref="DifyApiType.WORKFLOW"/></term>
    /// </item>
    /// </list>
    /// </summary>
    public string? ApiType { get; } = apiType;
}

public static class DifyApiType
{
    /// <summary>
    /// knowledge base api type
    /// </summary>
    public const string KNOWLEDGE_BASE = "KNOWLEDGE_BASE";
    
    /// <summary>
    /// completion api type
    /// </summary>
    public const string COMPLETION     = "COMPLETION";
    
    /// <summary>
    /// chat api type
    /// </summary>
    public const string CHAT           = "CHAT";
    
    /// <summary>
    /// workflow api type
    /// </summary>
    public const string WORKFLOW       = "WORKFLOW";
}