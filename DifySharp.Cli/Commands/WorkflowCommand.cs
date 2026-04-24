using System.CommandLine;
using System.CommandLine.Invocation;
using System.Text.Json;
using DifySharp.Cli.Helpers;
using DifySharp.Workflow;

namespace DifySharp.Cli.Commands;

public static class WorkflowCommand
{
    public static Command Create(Option<string> apiKeyOption, Option<string> baseUrlOption)
    {
        var userOption = new Option<string>("--user", () => "cli-user", "User identifier");
        userOption.AddAlias("-u");

        var inputsOption = new Option<string?>("--inputs", () => null,
            "Workflow inputs as JSON, e.g. '{\"key\":\"value\"}'");

        var streamingOption = new Option<bool>("--streaming", "Use streaming mode");
        streamingOption.AddAlias("-s");

        // workflow run
        var runCommand = new Command("run", "Execute a workflow")
        {
            userOption, inputsOption, streamingOption
        };

        runCommand.SetHandler((Func<InvocationContext, Task>)(async context =>
        {
            var user = context.ParseResult.GetValueForOption(userOption)!;
            var inputs = context.ParseResult.GetValueForOption(inputsOption);
            var streaming = context.ParseResult.GetValueForOption(streamingOption);
            var apiKey = context.ParseResult.GetValueForOption(apiKeyOption)!;
            var baseUrl = context.ParseResult.GetValueForOption(baseUrlOption)!;

            using var client = new DifyApiClient(apiKey, baseUrl);
            var parsedInputs = inputs != null
                ? JsonSerializer.Deserialize<object>(inputs) ?? new {}
                : new {};

            var body = new Run.RequestBody
            {
                User = user,
                Inputs = parsedInputs
            };

            if (streaming)
            {
                body.ResponseMode = ApplicationResponseMode.Streaming;
                var stream = client.PostStreamAsync("/v1/workflows/run", body, context.GetCancellationToken());
                context.ExitCode = await StreamingHelper.WriteStreamToConsole(stream, context.GetCancellationToken());
            }
            else
            {
                body.ResponseMode = ApplicationResponseMode.Blocking;
                var result = await client.PostAsync<Run.RequestBody, Run.ResponseBody>(
                    "/v1/workflows/run", body, context.GetCancellationToken());
                Console.WriteLine(JsonSerializer.Serialize(result, DifyApiClient.WriteIndentedOptions));
            }
        }));

        // workflow status
        var workflowIdArgument = new Argument<string>("workflow-id", "Workflow execution ID");
        var statusCommand = new Command("status", "Get workflow execution status")
        {
            workflowIdArgument
        };

        statusCommand.SetHandler((Func<InvocationContext, Task>)(async context =>
        {
            var workflowId = context.ParseResult.GetValueForArgument(workflowIdArgument);
            var apiKey = context.ParseResult.GetValueForOption(apiKeyOption)!;
            var baseUrl = context.ParseResult.GetValueForOption(baseUrlOption)!;

            using var client = new DifyApiClient(apiKey, baseUrl);
            var result = await client.GetAsync<Run.Data>($"/v1/workflows/run/{workflowId}", context.GetCancellationToken());
            Console.WriteLine(JsonSerializer.Serialize(result, DifyApiClient.WriteIndentedOptions));
        }));

        // workflow logs
        var keywordOption = new Option<string?>("--keyword", () => null, "Search keyword");
        var statusFilterOption = new Option<string?>("--status", () => null, "Filter by status");
        var pageOption = new Option<int>("--page", () => 1, "Page number");
        var limitOption = new Option<int>("--limit", () => 20, "Items per page");

        var logsCommand = new Command("logs", "Get workflow execution logs")
        {
            keywordOption, statusFilterOption, pageOption, limitOption
        };

        logsCommand.SetHandler((Func<InvocationContext, Task>)(async context =>
        {
            var keyword = context.ParseResult.GetValueForOption(keywordOption);
            var status = context.ParseResult.GetValueForOption(statusFilterOption);
            var page = context.ParseResult.GetValueForOption(pageOption);
            var limit = context.ParseResult.GetValueForOption(limitOption);
            var apiKey = context.ParseResult.GetValueForOption(apiKeyOption)!;
            var baseUrl = context.ParseResult.GetValueForOption(baseUrlOption)!;

            using var client = new DifyApiClient(apiKey, baseUrl);
            var queryParams = new List<string> { $"page={page}", $"limit={limit}" };
            if (keyword != null) queryParams.Add($"keyword={Uri.EscapeDataString(keyword)}");
            if (status != null) queryParams.Add($"status={Uri.EscapeDataString(status)}");

            var url = $"/v1/workflows/logs?{string.Join("&", queryParams)}";
            using var response = await client.PostRawAsync<object>(url, new {});
            var content = await response.Content.ReadAsStringAsync();
            Console.WriteLine(content);
        }));

        var command = new Command("workflow", "Workflow app operations")
        {
            runCommand, statusCommand, logsCommand
        };

        return command;
    }
}
