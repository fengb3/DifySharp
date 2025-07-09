using System.Text.Json;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Xunit.DependencyInjection.Logging;

// using Xunit.DependencyInjection.Logging;

namespace DifySharp.Test;

public class Startup
{
    public static IServiceProvider ServiceProvider { get; set; }

    public void ConfigureServices(IServiceCollection services)
    {
        ReadEnvironmentVariables();

        var knowledgeApiKey    = Environment.GetEnvironmentVariable("KNOWLEDGE_BASE_API_KEY");
        var workflowApiTestKey = Environment.GetEnvironmentVariable("WORKFLOW_API_KEY");
        var chatApiKey         = Environment.GetEnvironmentVariable("CHAT_API_KEY");
        services.AddDifySharp(option => {
            option.Secrets =
            [
                new DifyApiSecret(knowledgeApiKey!, "knowledge", DifyApiType.KNOWLEDGE_BASE),
                new DifyApiSecret(workflowApiTestKey!, "workflow", DifyApiType.WORKFLOW),
                new DifyApiSecret(chatApiKey, "chat", DifyApiType.CHAT)
            ];

            option.EnableLogging = true;
        });
        services.AddLogging(lb => lb.AddXunitOutput());

        ServiceProvider = services.BuildServiceProvider();
    }

    private static void ReadEnvironmentVariables()
    {
        var settingFilePath = Path.Join(Directory.GetCurrentDirectory(), "TestEnvironmentVariables.json");

        if (!File.Exists(settingFilePath)) return;

        using var file = File.Open(settingFilePath, FileMode.Open);
        var document = JsonDocument.Parse(file, new JsonDocumentOptions
        {
            CommentHandling = JsonCommentHandling.Skip
        });

        var variables = document.RootElement.EnumerateObject();

        foreach (var variable in variables)
        {
            var value = variable.Value.ToString();

            if (string.IsNullOrEmpty(value))
                continue;

            if (Environment.GetEnvironmentVariable(variable.Name) is null)
                Environment.SetEnvironmentVariable(variable.Name, variable.Value.ToString());
        }
    }
}