using System.Text.Json;
using System.Text.Json.Serialization;
using DifySharp;
using Microsoft.Extensions.Options;
using WebApiClientCore.Extensions.OAuths;
// ReSharper disable once CheckNamespace
using DifySharp.Apis;

namespace Microsoft.Extensions.DependencyInjection;

public static class DependencyInjection
{
    public static IServiceCollection AddDifySdk(this IServiceCollection services)
    {
        return services;
    }

    public static IServiceCollection ConfigDifyKnowledgeApi(this IServiceCollection services)
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
            })
            ;

        services.AddTokenProvider<IKnowledgeBaseApi>(sp =>
        {
            var options = sp.GetRequiredService<IOptions<DifyClientOptions>>().Value;

            return Task.FromResult(new TokenResult
            {
                Access_token = options.KnowledgeBaseApiKey,
                Expires_in   = 60 * 60 * 24, // one
            })!;
        });

        return services;
    }
}