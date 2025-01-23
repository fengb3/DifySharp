using System.Text.Json;
using Microsoft.Extensions.DependencyInjection;
using Xunit.DependencyInjection.Logging;

namespace DifySharp.Test;

public class Startup
{
    public void ConfigureServices(IServiceCollection services)
    {
        // Directory.GetCurrentDirectory() + "/TestSettings.Local.json";
        ReadEnviromnetVariables();

        var knowledgeApiKey = Environment.GetEnvironmentVariable("KNOWLEDGE_BASE_API_KEY");
        services.AddDifySdk(option => { option.KnowledgeBaseApiKey = knowledgeApiKey!; });
        services.AddLogging(lb => lb.AddXunitOutput());
    }

    private void ReadEnviromnetVariables()
    {
        var settingFilePath = Path.Join(Directory.GetCurrentDirectory(), "TestEnvironmentVariables.json");

        if (!File.Exists(settingFilePath)) return;

        using var file = File.Open(settingFilePath, FileMode.Open);
        var document = JsonDocument.Parse(file, new JsonDocumentOptions()
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