using System.CommandLine;
using DifySharp.Cli.Commands;

namespace DifySharp.Cli;

class Program
{
    static async Task<int> Main(string[] args)
    {
        var apiKeyOption = new Option<string>(
            "--api-key",
            () => Environment.GetEnvironmentVariable("DIFY_API_KEY") ?? "",
            "API key (or set DIFY_API_KEY env var)");
        apiKeyOption.AddAlias("-k");

        var baseUrlOption = new Option<string>(
            "--base-url",
            () => "https://api.dify.ai/v1",
            "Base URL of Dify API");
        baseUrlOption.AddAlias("-b");

        var rootCommand = new RootCommand("DifySharp CLI - Command line tool for Dify API")
        {
            ChatCommand.Create(apiKeyOption, baseUrlOption),
            CompletionCommand.Create(apiKeyOption, baseUrlOption),
            WorkflowCommand.Create(apiKeyOption, baseUrlOption),
            KnowledgeBaseCommand.Create(apiKeyOption, baseUrlOption),
        };

        rootCommand.AddGlobalOption(apiKeyOption);
        rootCommand.AddGlobalOption(baseUrlOption);

        var cts = new CancellationTokenSource();
        Console.CancelKeyPress += (_, e) =>
        {
            e.Cancel = true;
            cts.Cancel();
        };

        return await rootCommand.InvokeAsync(args);
    }
}
