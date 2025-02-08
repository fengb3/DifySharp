// See https://aka.ms/new-console-template for more information

using System.Text.Encodings.Web;
using System.Text.Json;
using DifySharp;
using DifySharp.Chat.ChatMessages;
using DifySharp.Extensions;
using DifySharp.KnowledgeBase.Dataset;
using Microsoft.Extensions.Logging;


// knowledge base api

// var knowledgeBaseClient = new KnowledgeBaseClient("your-api-key", "https://api.dify.ai/v1");
//
// var response1 = await knowledgeBaseClient.PostCreateDatasetAsync(
// 	new Create.RequestBody(
// 		"Dataset Name",
// 		"this is a test dataset"
// 	)
// );

// chat api

var chatClient = new ChatClient("app-YmdV5dKG5KQMZOfT44AATGCR");

// for blocking mode
var response = await chatClient.PostChatMessageBlocking(new ChatMessage.RequestBody
{
    Query        = "ping",
    ResponseMode = ChatMessage.ResponseMode.Blocking,
    User         = "test-user"
});

Console.WriteLine(response.Answer);

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

// var response2 = await chatClient.