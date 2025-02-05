using System.Net.Http.Headers;
using DifySharp.ApiKey;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using WebApiClientCore;
using WebApiClientCore.Attributes;

namespace DifySharp.Attributes;

public class DifyAuthAttribute : ApiFilterAttribute
{
	public override Task OnRequestAsync(ApiRequestContext context)
	{
		var x         = context.HttpContext.ServiceProvider.GetRequiredService<IApiKeyProvider>();
		var tokenType = "Bearer";
		var token     = x.ApiKey;
		context.HttpContext.RequestMessage.Headers.Authorization = new AuthenticationHeaderValue(tokenType, token);
		return Task.CompletedTask;
	}

	public override async Task OnResponseAsync(ApiResponseContext context)
	{
		// var logger       = context.HttpContext.ServiceProvider.GetRequiredService<ILogger<DifyAuthAttribute>>();
		// var responseBody = await context.HttpContext.ResponseMessage.Content?.ReadAsStringAsync();
		// // context.HttpContext.ResponseMessage.Content.
		
		// logger.LogInformation(responseBody);
	}
}