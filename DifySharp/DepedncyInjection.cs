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
    public static IServiceCollection AddDifySharp(this IServiceCollection services,
        Action<DifyClientOptions>                                         configOptions)
    {
        var options = new DifyClientOptions();
        configOptions(options);

        services.AddOptions<DifyClientOptions>()
            .Configure(configOptions).Services
            .ConfigureDifyApi<IKnowledgeBaseApi>()
            .ConfigureDifyApi<IChatApi>()
            .ConfigureDifyApi<ICompletionApi>()
            .ConfigureDifyApi<IWorkflowApi>()
            ;

        services.AddScoped<IApiKeyProvider, ApiKeyProvider>();

        foreach (var apiKey in options.Secrets)
        {
            var key = apiKey.Name;

            switch (apiKey.ApiType)
            {
                case DifyApiType.KNOWLEDGE_BASE:
                    // services.AddKeyedScoped<DifyClient<IKnowledgeBaseApi>>(key);
                    services
                        .AddKeyedScoped(key, DifyClientFactory.DifyClientGenericFactory<IKnowledgeBaseApi>(apiKey))
                        .AddKeyedScoped(key, DifyClientFactory.MakeKnowledgeBaseClientFactory())
                        ;

                    break;

                case DifyApiType.COMPLETION:
                    services
                        .AddKeyedScoped(key, DifyClientFactory.DifyClientGenericFactory<ICompletionApi>(apiKey))
                        .AddKeyedScoped(key, DifyClientFactory.MakeCompletionClientFactory())
                        ;

                    break;

                case DifyApiType.CHAT:
                    services
                        .AddKeyedScoped(key, DifyClientFactory.DifyClientGenericFactory<IChatApi>(apiKey))
                        .AddKeyedScoped(key, DifyClientFactory.MakeChatClientFactory())
                        ;

                    break;

                case DifyApiType.WORKFLOW:
                    services
                        .AddKeyedScoped(key, DifyClientFactory.DifyClientGenericFactory<IWorkflowApi>(apiKey))
                        .AddKeyedScoped(key, DifyClientFactory.MakeWorkflowClientFactory())
                        ;

                    break;
            }
        }

        return services;
    }
    

    private static IServiceCollection ConfigureDifyApi<TApi>(this IServiceCollection services) where TApi : class
    {
        return services
                .AddHttpApi<TApi>((apiOptions, sp) =>
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
                }).Services
            ;
    }
    
    
    // private static IServiceCollection ConfigureDifyKnowledgeApi(this IServiceCollection services)
    // {
    //     services
    //         .AddHttpApi<IKnowledgeBaseApi>((apiOptions, sp) =>
    //         {
    //             var difyOptions = sp.GetRequiredService<IOptions<DifyClientOptions>>().Value;
    //             apiOptions.HttpHost = new Uri(difyOptions.BaseUrl);
    //
    //             // 序列化配置
    //             apiOptions.JsonDeserializeOptions.PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower;
    //             apiOptions.JsonDeserializeOptions.Converters.Add(
    //                 new JsonStringEnumConverter(JsonNamingPolicy.SnakeCaseLower));
    //
    //             apiOptions.JsonSerializeOptions.PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower;
    //             apiOptions.JsonSerializeOptions.Converters.Add(
    //                 new JsonStringEnumConverter(JsonNamingPolicy.SnakeCaseLower));
    //
    //             apiOptions.KeyValueSerializeOptions.IgnoreNullValues = true;
    //             apiOptions.UseLogging                                = true;
    //         });
    //
    //     return services;
    // }

    // public static IServiceCollection ConfigureDifyApplicationApi(this IServiceCollection services)
    // {
    //     // services.AddHttpApi<IApplicationApi>()
    //     //     ;
    //
    //     // services.AddTokenProvider<IApplicationApi, MyTokenProvider>("something thing");
    //     // services.AddTokenProvider<IApplicationApi, MyTokenProvider>("something thing2");
    //
    //     // services.AddKeyedScoped<ITokenProvider, MyTokenProvider>(
    //     // 	"some key", (sp, key) => { return new MyTokenProvider(); });
    //
    //     // services.add
    //
    //     return services;
    // }
}