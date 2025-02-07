namespace DifySharp.ApiKey;

public interface IApiKeyProvider
{
    public string? ApiKey { get; set; }
}

public class ApiKeyProvider : IApiKeyProvider
{
    public string? ApiKey { get; set; }
}