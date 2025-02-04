using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.Extensions.Options;
using WebApiClientCore.Extensions.OAuths;
using DifySharp;
using DifySharp.ApiKey;
using DifySharp.Apis;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using WebApiClientCore.Extensions.OAuths.TokenProviders;

namespace Microsoft.Extensions.DependencyInjection;

public static class DependencyInjection
{
	public static IServiceCollection AddDifySharp(this IServiceCollection   services,
	                                              Action<DifyClientOptions> configOptions)
	{
		var options = new DifyClientOptions();
		configOptions(options);

		services.AddOptions<DifyClientOptions>()
		        .Configure(configOptions).Services
		        .ConfigureDifyKnowledgeApi()
		        .ConfigureDifyApplicationApi()
			;

		// services.AddScoped<ApiKeyOptions>();

		services.AddScoped<IApiKeyProvider, ApiKeyProvider>();

		foreach (var optionsApiKey in options.ApiKeys)
		{
			// services.AddKeyedScoped<ApiKeyOptions>(optionsApiKey.ApiName, (sp, key) =>
			// {
			// 	return optionsApiKey;
			// });

			switch (optionsApiKey.ApiType)
			{
				case DifyApiType.KNOWLEDGE_BASE:
					services.AddKeyedScoped<KnowledgeBaseClient>(
						optionsApiKey.ApiName,
						(sp, key) => { return new KnowledgeBaseClient(optionsApiKey, sp); });

					break;

				case DifyApiType.COMPLETION:
					services.AddKeyedScoped<CompletionClient>(
						optionsApiKey.ApiName,
						(sp, key) => { return new CompletionClient(optionsApiKey, sp); });

					break;

				case DifyApiType.CHAT:
					services.AddKeyedScoped<ChatClient>(
						optionsApiKey.ApiName,
						(sp, key) => { return new ChatClient(optionsApiKey, sp); });

					break;

				case DifyApiType.WORKFLOW:
					services.AddKeyedScoped<WorkflowClient>(
						optionsApiKey.ApiName,
						(sp, key) => { return new WorkflowClient(optionsApiKey, sp); });

					break;
			}
		}

		return services;
	}

	private static IServiceCollection ConfigureDifyKnowledgeApi(this IServiceCollection services)
	{
		services
		   .AddHttpApi<IKnowledgeBaseApi>((apiOptions, sp) =>
			{
				var difyOptions = sp.GetRequiredService<IOptions<DifyClientOptions>>().Value;
				apiOptions.HttpHost = new Uri(difyOptions.BaseUrl);

				// 序列化配置
				apiOptions.JsonDeserializeOptions.PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower;
				apiOptions.JsonDeserializeOptions.Converters.Add(
					new JsonStringEnumConverter(JsonNamingPolicy.SnakeCaseLower));

				apiOptions.JsonSerializeOptions.PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower;
				apiOptions.JsonSerializeOptions.Converters.Add(
					new JsonStringEnumConverter(JsonNamingPolicy.SnakeCaseLower));

				apiOptions.KeyValueSerializeOptions.IgnoreNullValues = true;
				apiOptions.UseLogging                                = true;
			})
			// .ConfigureHttpClient((sp, client) =>
			// {
			//     var options = sp.GetRequiredService<IOptions<DifyClientOptions>>().Value;
			//     client.DefaultRequestHeaders.Add("Authorization", $"Bearer {options.KnowledgeBaseApiKey}");
			// })
			;

		services
		   .AddTokenProvider<IKnowledgeBaseApi>(async sp =>
			{
				var options = sp.GetRequiredService<IOptions<DifyClientOptions>>().Value;

				return await Task.FromResult(new TokenResult
				{
					Access_token = options.KnowledgeBaseApiKey,
					Expires_in   = 60 * 60 * 24, // one day
				})!;
			});

		return services;
	}

	public static IServiceCollection ConfigureDifyApplicationApi(this IServiceCollection services)
	{
		services.AddHttpApi<IApplicationApi>()
			;

		// services.AddTokenProvider<IApplicationApi, MyTokenProvider>("something thing");
		// services.AddTokenProvider<IApplicationApi, MyTokenProvider>("something thing2");

		services.AddKeyedScoped<ITokenProvider, MyTokenProvider>(
			"some key", (sp, key) => { return new MyTokenProvider(); });

		// services.add

		return services;
	}

	public class MyTokenProvider : ITokenProvider
	{
		private ILogger<MyTokenProvider> _logger;

		public MyTokenProvider()
		{
		}

		public MyTokenProvider(ILogger<MyTokenProvider> logger)
		{
		}

		public void ClearToken()
		{
			_logger.LogInformation("clear token");
		}

		public Task<TokenResult> GetTokenAsync()
		{
			_logger.LogInformation("get token");
			return Task.FromResult(new TokenResult());
		}

		private string? _name;

		public string Name
		{
			set
			{
				_logger.LogInformation("set name: {name}", value);
				_name = value;
			}
		}
	}
}