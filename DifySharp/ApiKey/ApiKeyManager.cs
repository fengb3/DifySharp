using Microsoft.Extensions.Options;

namespace DifySharp.Token;

public class ApiKeyManager
{
	public const string DEFAULT_API_NAME = "Default";

	// private Dictionary<string, string> _apiKeys = new();

	private Dictionary<string, string> _knowledgeBaseKeys;

	private Dictionary<string, string> _applicationKeys;

	public ApiKeyManager(IOptionsMonitor<DifyClientOptions> optionsMonitor)
	{
		var currentOptions = optionsMonitor.CurrentValue;

		// _knowledgeBaseKeys = currentOptions
		//                     .ApiKeys
		//                     .Where(o => o.ApiType == difya.KNOWLEDGE_BASE)
		//                     .ToDictionary(o => o.ApiName, o => o.ApiKey);
		//
		// _applicationKeys = currentOptions
		//                   .ApiKeys
		//                   .Where(key => key.ApiType == ApiKeyOptions.APPLICATION)
		//                   .ToDictionary(k => k.ApiName, k => k.ApiKey);
	}
}