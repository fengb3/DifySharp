using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.Extensions.Options;
using WebApiClientCore.Extensions.OAuths;
using DifySharp;
using DifySharp.Apis;
using Microsoft.Extensions.Configuration;

namespace Microsoft.Extensions.DependencyInjection;

public static class DependencyInjection
{
    public static IServiceCollection AddDifySdk(this IServiceCollection services,
        Action<DifyClientOptions>                                       configOptions)
    {
        services.AddOptions<DifyClientOptions>()
            .Configure(configOptions).Services
            .ConfigureDifyKnowledgeApi()
            ;

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

                apiOptions.UseLogging = true;
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
                    Expires_in   = 60 * 60 * 24, // one
                })!;
            });

        return services;
    }
}