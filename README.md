# DifySharp

![GitHub Actions Workflow Status](https://img.shields.io/github/actions/workflow/status/fengb3/DifySharp/unit-test.yml?style=flat&logo=dotnet)
![NuGet Version](https://img.shields.io/nuget/v/DifySharp?style=flat&logo=nuget)

Dify SDK for csharp


## Install

```bash
dotnet add package DifySharp
```

## Usage

### Without Dependency Injection


```csharp

var chatClient = new ChatClient("<your-api-key>");

// for blocking mode
var response = await chatClient.PostChatMessageBlocking(new ChatMessage.RequestBody
{
    Query        = "ping!",
    ResponseMode = ChatMessage.ResponseMode.Blocking,
    User         = "test-user"
});

Console.WriteLine(response.Answer); // pong！

// for streaming mode
var chunks = chatClient.PostChatMessageStreaming(new ChatMessage.RequestBody
{
    Query        = "What dose the fox say?",
    ResponseMode = ChatMessage.ResponseMode.Streaming,
    User         = "test-user"
});

await foreach (var chunk in chunks)
{
    if (chunk is ChatMessage.MessageEvent messageEvent)
        Console.Write(messageEvent.Answer);
}
// Ring-ding-ding-ding-dingeringeding! Gering-ding-ding-ding-dingeringeding! Wa-pa-pa-pa-pa-pa-pow! Jacha-chacha-chacha-chow! Fraka-kaka-kaka-kaka-kow! A-hee-ahee ha-hee! A-oo-oo-oo-ooo!
```
