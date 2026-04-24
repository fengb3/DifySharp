using System.CommandLine;
using System.CommandLine.Invocation;
using System.Text.Json;
using DifySharp.Cli.Helpers;
using DifySharp.Completion.CompletionMessages;
using CompletionApplication = DifySharp.Completion.Application;

namespace DifySharp.Cli.Commands;

public static class CompletionCommand
{
    public static Command Create(Option<string> apiKeyOption, Option<string> baseUrlOption)
    {
        var userOption = new Option<string>("--user", () => "cli-user", "User identifier");
        userOption.AddAlias("-u");

        var streamingOption = new Option<bool>("--streaming", "Use streaming mode");
        streamingOption.AddAlias("-s");

        var queryArgument = new Argument<string>("query", "The prompt to send");

        var sendCommand = new Command("send", "Send a completion message")
        {
            queryArgument, userOption, streamingOption
        };

        sendCommand.SetHandler((Func<InvocationContext, Task>)(async context =>
        {
            var query = context.ParseResult.GetValueForArgument(queryArgument);
            var user = context.ParseResult.GetValueForOption(userOption)!;
            var streaming = context.ParseResult.GetValueForOption(streamingOption);
            var apiKey = context.ParseResult.GetValueForOption(apiKeyOption)!;
            var baseUrl = context.ParseResult.GetValueForOption(baseUrlOption)!;

            using var client = new DifyApiClient(apiKey, baseUrl);
            var body = new CompletionMessages.RequestBody
            {
                Query = query,
                User = user
            };

            if (streaming)
            {
                body.ResponseMode = ApplicationResponseMode.Streaming;
                var stream = client.PostStreamAsync("/v1/completion-messages", body, context.GetCancellationToken());
                context.ExitCode = await StreamingHelper.WriteStreamToConsole(stream, context.GetCancellationToken());
            }
            else
            {
                body.ResponseMode = ApplicationResponseMode.Blocking;
                var result = await client.PostAsync<CompletionMessages.RequestBody, CompletionMessages.ResponseBody>(
                    "/v1/completion-messages", body, context.GetCancellationToken());
                Console.WriteLine(result.Answer);
            }
        }));

        var infoCommand = new Command("info", "Get application info");
        infoCommand.SetHandler((Func<InvocationContext, Task>)(async context =>
        {
            var apiKey = context.ParseResult.GetValueForOption(apiKeyOption)!;
            var baseUrl = context.ParseResult.GetValueForOption(baseUrlOption)!;

            using var client = new DifyApiClient(apiKey, baseUrl);
            var result = await client.GetAsync<CompletionApplication.Basic.ResponseBody>("/v1/info", context.GetCancellationToken());
            Console.WriteLine(JsonSerializer.Serialize(result, DifyApiClient.WriteIndentedOptions));
        }));

        var parametersCommand = new Command("parameters", "Get application parameters");
        parametersCommand.SetHandler((Func<InvocationContext, Task>)(async context =>
        {
            var apiKey = context.ParseResult.GetValueForOption(apiKeyOption)!;
            var baseUrl = context.ParseResult.GetValueForOption(baseUrlOption)!;

            using var client = new DifyApiClient(apiKey, baseUrl);
            var result = await client.GetAsync<CompletionApplication.Parameters.ResponseBody>("/v1/parameters", context.GetCancellationToken());
            Console.WriteLine(JsonSerializer.Serialize(result, DifyApiClient.WriteIndentedOptions));
        }));

        var command = new Command("completion", "Completion app operations")
        {
            sendCommand, infoCommand, parametersCommand
        };

        return command;
    }
}
