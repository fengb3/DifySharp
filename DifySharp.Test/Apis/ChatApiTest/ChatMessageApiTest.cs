using DifySharp.Apis;
using DifySharp.Chat.ChatMessages;
using DifySharp.Extensions;
using JetBrains.Annotations;
using Microsoft.Extensions.DependencyInjection;

namespace DifySharp.Test.Apis.ChatApiTest;

public class ChatMessageApiTestFixture : ApiTestFixture
{
    public ChatClient Client { get; }

    public ChatMessageApiTestFixture()
    {
        Client = ServiceProvider.GetRequiredKeyedService<ChatClient>("chat");
    }
}

[TestSubject(typeof(IChatApi))]
public class ChatMessageApiTest(ChatMessageApiTestFixture fixture) : IClassFixture<ChatMessageApiTestFixture>
{
    private ChatClient Client => fixture.Client;

    [Fact]
    public async Task TestChatMessage_shouldHaveMessage()
    {
        var responseBody = await Client.PostChatMessageBlocking(new ChatMessage.RequestBody
        {
            Query = "Why the sky is blue",
            User  = Guid.NewGuid().ToString("N") + "-- test case"
        });
        
        Assert.NotNull(responseBody);
        Assert.NotNull(responseBody.Answer);
    }
    
}