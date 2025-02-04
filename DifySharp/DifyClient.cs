using DifySharp.ApiKey;
using DifySharp.Apis;
using DifySharp.KnowledgeBase.Dataset;
using DifySharp.KnowledgeBase.Document;
using Microsoft.Extensions.DependencyInjection;
using Delete = DifySharp.KnowledgeBase.Document.Delete;
using Get = DifySharp.KnowledgeBase.Dataset.Get;

namespace DifySharp;

// public static class DifyClientFactory<T> where T : DifyClient, new()
// {
// 	private static T? Client { get; set; }
//
// 	public static T Create(string name, string key, string type)
// 	{
// 		Client = new T();
//
// 		return Client;
// 	}
// }

public abstract class DifyClient : IDisposable
{
	public IServiceScope ServiceScope { get; set; }

	protected IServiceProvider ServiceProvider => ServiceScope.ServiceProvider;

	public ApiKeyOptions ApiKeyOptions { get; set; }

	public DifyClient(ApiKeyOptions apiKeyOptions, IServiceProvider? sp = null)
	{
		ApiKeyOptions = apiKeyOptions;

		ConfigureApiServices(apiKeyOptions.ApiKey, apiKeyOptions.ApiType);

		if (sp == null)
		{
			var services = new ServiceCollection();
			ConfigureServices?.Invoke(services);
			sp = services.BuildServiceProvider();
		}

		ServiceScope = sp.CreateScope();

		ServiceProvider.GetRequiredService<IApiKeyProvider>().ApiKey = apiKeyOptions.ApiKey;
	}

	public Action<IServiceCollection>? ConfigureServices { get; protected set; }

	public void ConfigureApiServices(string apiKey, string apiType)
	{
		ConfigureServices += services =>
		{
			services.AddDifySharp(options =>
			{
				options.ApiKeys =
				[
					new ApiKeyOptions(apiKey, "default", apiType)
				];
			});
		};
	}

	public void Dispose()
	{
		ServiceScope.Dispose();
	}
}

public class DifyClient<T> : DifyClient
{
	public T Api { get; set; }

	public DifyClient(ApiKeyOptions apiKeyOptions, IServiceProvider? sp = null) : base(apiKeyOptions, sp)
	{
		Api = ServiceProvider.GetRequiredService<T>();
	}
}

public class KnowledgeBaseClient : DifyClient<IKnowledgeBaseApi>
{
	public KnowledgeBaseClient(ApiKeyOptions apiKeyOptions, IServiceProvider? sp = null) : base(apiKeyOptions, sp)
	{
	}
}

public class CompletionClient : DifyClient<ICompletionApi>
{
	public CompletionClient(ApiKeyOptions apiKeyOptions, IServiceProvider? sp = null) : base(apiKeyOptions, sp)
	{
	}
}

public class ChatClient : DifyClient<IChatApi>
{
	public ChatClient(ApiKeyOptions apiKeyOptions, IServiceProvider? sp = null) : base(apiKeyOptions, sp)
	{
	}
}

public class WorkflowClient : DifyClient<IWorkflowApi>
{
	public WorkflowClient(ApiKeyOptions apiKeyOptions, IServiceProvider? sp = null) : base(apiKeyOptions, sp)
	{
	}
}

// public class DifyClientFactory(IServiceProvider sp)
// {
// 	
// 	
// }

//
// public abstract class DifyClient : IDisposable
// {
// 	public IServiceScope ServiceScope { get; set; }
//
// 	protected IServiceProvider ServiceProvider => ServiceScope.ServiceProvider;
//
// 	public DifyClient(IServiceProvider sp)
// 	{
// 		ServiceScope = sp.CreateScope();
// 	}
//
// 	public Action<IServiceCollection>? ConfigureServices { get; protected set; }
//
// 	public DifyClient()
// 	{
// 		var services = new ServiceCollection();
// 		ConfigureServices?.Invoke(services);
// 		ServiceScope = services.BuildServiceProvider().CreateScope();
// 	}
//
// 	public void Dispose() => ServiceScope.Dispose();
//
// 	public void ConfigureApiServices(string apiKey, string apiType)
// 	{
// 		ConfigureServices += services =>
// 		{
// 			services.AddDifySharp(options =>
// 			{
// 				options.ApiKeys =
// 				[
// 					new ApiKeyOptions(apiKey, "default", apiType)
// 				];
// 			});
// 		};
// 	}
// }
//
// public class KnowledgeBaseClient : DifyClient
// {
// 	public KnowledgeBaseClient(IServiceProvider sp) : base(sp)
// 	{
// 	}
//
// 	public KnowledgeBaseClient(string apiKey) : base()
// 	{
// 		ConfigureApiServices(apiKey, DifyApiType.KNOWLEDGE_BASE);
// 	}
// }
//
// public class CompletionClient : DifyClient
// {
// 	public CompletionClient(IServiceProvider sp) : base(sp)
// 	{
// 	}
//
// 	public CompletionClient(string apiKey) : base()
// 	{
// 		ConfigureApiServices(apiKey, DifyApiType.COMPLETION);
// 	}
// }
//
// public class ChatClient : DifyClient
// {
// 	public ChatClient(IServiceProvider sp) : base(sp)
// 	{
// 	}
//
// 	public ChatClient(string apiKey) : base()
// 	{
// 		ConfigureApiServices(apiKey, DifyApiType.CHAT);
// 	}
// }
//
// public class WorkflowClient : DifyClient
// {
// 	public WorkflowClient(IServiceProvider sp) : base(sp)
// 	{
// 	}
//
// 	public WorkflowClient(string apiKey) : base()
// 	{
// 		ConfigureApiServices(apiKey, DifyApiType.WORKFLOW);
// 	}
// }