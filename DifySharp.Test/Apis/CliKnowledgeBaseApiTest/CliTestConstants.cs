using DifySharp.Cli;

namespace DifySharp.Test.Apis.CliKnowledgeBaseApiTest;

public static class CliTestConstants
{
    public static string ApiKey =>
        Environment.GetEnvironmentVariable("CLI_KB_API_KEY")
        ?? throw new InvalidOperationException("CLI_KB_API_KEY environment variable not set");

    // Base URL WITHOUT /v1 suffix -- the CLI's URL paths include /v1 already
    public static string BaseUrl =>
        Environment.GetEnvironmentVariable("CLI_KB_BASE_URL")
        ?? "http://11.53.116.97:11450";

    public static DifyApiClient CreateClient() => new(ApiKey, BaseUrl);
}
