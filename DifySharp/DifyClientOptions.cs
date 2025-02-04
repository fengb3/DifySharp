namespace DifySharp;

public class DifyClientOptions
{
	public string BaseUrl { get; set; } = "https://api.dify.ai/v1";

	public string KnowledgeBaseApiKey { get; set; } = "";

	public ICollection<ApiKeyOptions> ApiKeys { get; set; } = new List<ApiKeyOptions>();
}

public class ApiKeyOptions(string apiKey = "", string apiName = "", string apiType = "")
{
	/// <summary>
	/// Represents the API key associated with a specific API. The key is used for authentication and authorization purposes
	/// when making requests to secure endpoints.
	/// </summary>
	public string ApiKey { get; set; } = apiKey;

	/// <summary>
	/// Specifies the name of the API associated with a particular API key. This property is used to differentiate
	/// between multiple APIs, typically when managing multiple API keys within a system.
	/// </summary>
	public string ApiName { get; set; } = apiName;

	/// <summary>
	/// Specifies the type of the API associated with the key.
	/// Can be `knowledge_base` and `application`
	/// </summary>
	public string ApiType { get; set; } = apiType;
}

public static class DifyApiType
{
	public const string KNOWLEDGE_BASE = "knowledge_base";
	public const string COMPLETION     = "completion";
	public const string CHAT           = "chat";
	public const string WORKFLOW       = "workflow";
}