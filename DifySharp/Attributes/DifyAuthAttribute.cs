using System.Net.Http.Headers;
using DifySharp.ApiKey;
using Microsoft.Extensions.DependencyInjection;
using WebApiClientCore;
using WebApiClientCore.Attributes;

namespace DifySharp.Attributes;

public class DifyAuthAttribute : ApiFilterAttribute
{
    public override Task OnRequestAsync(ApiRequestContext context)
    {
        var apiKeyProvider = context.HttpContext.ServiceProvider.GetRequiredService<IApiKeyProvider>();
        var tokenType      = "Bearer";
        var token          = apiKeyProvider.ApiKey;
        context.HttpContext.RequestMessage.Headers.Authorization = new AuthenticationHeaderValue(tokenType, token);
        return Task.CompletedTask;
    }

    public override Task OnResponseAsync(ApiResponseContext context)
    {
        return Task.CompletedTask;
        // var logger       = context.HttpContext.ServiceProvider.GetRequiredService<ILogger<DifyAuthAttribute>>();
        // var responseBody = await context.HttpContext.ResponseMessage.Content?.ReadAsStringAsync();
        // // context.HttpContext.ResponseMessage.Content.

        // logger.LogInformation(responseBody);
    }
}