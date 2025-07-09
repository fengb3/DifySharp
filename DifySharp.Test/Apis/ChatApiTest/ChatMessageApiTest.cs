using DifySharp.Apis;
using DifySharp.Chat.ChatMessages;
using DifySharp.Extensions;
using JetBrains.Annotations;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Text;
using System.Text.Json;

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
public class ChatMessageApiTest(ChatMessageApiTestFixture fixture, ILogger<ChatMessageApiTest> logger) : IClassFixture<ChatMessageApiTestFixture>
{
    private ChatClient Client => fixture.Client;

    private static JsonSerializerOptions JsonSerializerOptions = new()
    {
        WriteIndented = true
    };

    [Fact]
    public async Task TestChatMessage_ShouldHaveMessage()
    {
        var responseBody = await Client.PostChatMessageBlocking(new ChatMessage.RequestBody
        {
            Query = "Why the sky is blue",
            User  = Guid.NewGuid().ToString("N") + "-- test case"
        });
        
        Assert.NotNull(responseBody);
        Assert.NotNull(responseBody.Answer);
        
        logger.LogInformation("TestChatMessage_shouldHaveMessage: {info}", 
            System.Text.Json.JsonSerializer.Serialize(responseBody, JsonSerializerOptions));
    }
    
    [Fact]
    public async Task TestGetInfo_ShouldReturnApplicationInfo()
    {
        var result = await Client.GetInfo();
        
        Assert.NotNull(result);
        logger.LogInformation("GetInfo result: {info}", 
            JsonSerializer.Serialize(result, JsonSerializerOptions));
    }
    
    [Fact]
    public async Task TestGetParameters_ShouldReturnParameters()
    {
        var result = await Client.GetParameters();
        
        Assert.NotNull(result);
        logger.LogInformation("GetParameters result: {info}", 
            JsonSerializer.Serialize(result, JsonSerializerOptions));
    }
    
    [Fact]
    public async Task TestGetMeta_ShouldReturnMeta()
    {
        var result = await Client.GetMeta();
        
        Assert.NotNull(result);
        logger.LogInformation("GetMeta result: {info}", 
            JsonSerializer.Serialize(result, JsonSerializerOptions));
    }
    
    [Fact]
    public async Task TestChatMessageStreaming_ShouldReceiveStreamingResponse()
    {
        var requestBody = new ChatMessage.RequestBody
        {
            Query = "请简短介绍一下人工智能",
            User = Guid.NewGuid().ToString("N") + "-- streaming test"
        };

        var chunks = new List<string>();
        
        await foreach (var chunk in Client.PostChatMessageStreaming(requestBody))
        {
            Assert.NotNull(chunk);
            
            // 根据不同的事件类型处理Answer属性
            switch (chunk)
            {
                case MessageEvent messageEvent:
                    if (!string.IsNullOrEmpty(messageEvent.Answer))
                    {
                        chunks.Add(messageEvent.Answer);
                    }
                    break;
                case AgentMessageEvent agentMessageEvent:
                    if (!string.IsNullOrEmpty(agentMessageEvent.Answer))
                    {
                        chunks.Add(agentMessageEvent.Answer);
                    }
                    break;
                case MessageReplaceEvent messageReplaceEvent:
                    if (!string.IsNullOrEmpty(messageReplaceEvent.Answer))
                    {
                        chunks.Add(messageReplaceEvent.Answer);
                    }
                    break;
            }
            
            logger.LogInformation("Received chunk ({eventType}): {chunk}", 
                chunk.Event,
                JsonSerializer.Serialize(chunk, JsonSerializerOptions));
        }
        
        Assert.NotEmpty(chunks);
        logger.LogInformation("Total chunks received: {count}", chunks.Count);
    }
    
    [Fact]
    public async Task TestGetConversations_ShouldReturnConversationsList()
    {
        var user = Guid.NewGuid().ToString("N") + "-- conversations test";
        
        // 先发送一条消息创建对话
        await Client.PostChatMessageBlocking(new ChatMessage.RequestBody
        {
            Query = "Hello, this is a test conversation",
            User = user
        });
        
        // 获取对话列表
        var result = await Client.GetConversations(user, limit: 10);
        
        Assert.NotNull(result);
        logger.LogInformation("GetConversations result: {info}", 
            JsonSerializer.Serialize(result, JsonSerializerOptions));
    }
    
    [Fact]
    public async Task TestChatMessageWithConversationId_ShouldContinueConversation()
    {
        var user = Guid.NewGuid().ToString("N") + "-- conversation test";
        
        // 第一条消息
        var firstResponse = await Client.PostChatMessageBlocking(new ChatMessage.RequestBody
        {
            Query = "我的名字叫小明",
            User = user
        });
        
        Assert.NotNull(firstResponse);
        Assert.NotNull(firstResponse.ConversationId);
        
        // 第二条消息，使用相同的对话ID
        var secondResponse = await Client.PostChatMessageBlocking(new ChatMessage.RequestBody
        {
            Query = "我的名字是什么？",
            User = user,
            ConversationId = firstResponse.ConversationId
        });
        
        Assert.NotNull(secondResponse);
        Assert.Equal(firstResponse.ConversationId, secondResponse.ConversationId);
        
        logger.LogInformation("First response: {first}", 
            JsonSerializer.Serialize(firstResponse, JsonSerializerOptions));
        logger.LogInformation("Second response: {second}", 
            JsonSerializer.Serialize(secondResponse, JsonSerializerOptions));
    }
    
    [Fact]
    public async Task TestChatMessageWithEmptyQuery_ShouldThrowException()
    {
        await Assert.ThrowsAsync<HttpRequestException>(async () =>
        {
            await Client.PostChatMessageBlocking(new ChatMessage.RequestBody
            {
                Query = "",
                User = Guid.NewGuid().ToString("N") + "-- empty query test"
            });
        });
    }
    
    [Fact]
    public async Task TestChatMessageWithInvalidUser_ShouldHandleGracefully()
    {
        // 测试用空字符串作为用户
        var exception = await Assert.ThrowsAsync<HttpRequestException>(async () =>
        {
            await Client.PostChatMessageBlocking(new ChatMessage.RequestBody
            {
                Query = "Test query",
                User = ""
            });
        });
        
        logger.LogInformation("Expected exception for invalid user: {exception}", exception.Message);
    }
}