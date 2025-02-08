using System.Text.Encodings.Web;
using System.Text.Json;
using DifySharp;
using DifySharp.Apis;
using DifySharp.Chat.ChatMessages;
using DifySharp.Extensions;
using DifySharp.KnowledgeBase;
using DifySharp.KnowledgeBase.Document;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDifySharp(o =>
{
    o.Secrets =
    [
        new DifyApiSecret("<your-knowledge-base-secret>", "knowledge", DifyApiType.KNOWLEDGE_BASE),
        new DifyApiSecret("<your-chat-application-secret>", "chat", DifyApiType.CHAT)
    ];
}); // add you api key here

var app = builder.Build();

app.MapGet("/", async (IServiceProvider sp) =>
{
    var api = sp.GetRequiredKeyedService<KnowledgeBaseClient>("knowledge"); // get client instance by name in configuration

    var uuid = Guid.NewGuid().ToString("N")[..6];

    var response = await api.PostCreateDocumentByTextAsync("", // add a dataset id here
        new CreateByText.RequestBody(
            $"Test Document {uuid}",
            "Test Content",
            IndexingTechnique.Economy,
            DocForm.TextModel,
            "",
            new ProcessRule(
                "automatic",
                new Rules(
                    [
                        new PreProcessingRule(
                            "remove_extra_spaces",
                            true
                        ),
                        new PreProcessingRule(
                            "remove_urls_emails",
                            true
                        )
                    ],
                    new Segmentation(
                        "\n\n",
                        1000
                    ),
                    "paragraph",
                    new SubChunkSegmentation(
                        "\n\n",
                        1000,
                        200
                    )
                )
            ),
            new CreateByText.RetrievalModel(
                CreateByText.SearchMethod.HybridSearch,
                false,
                new CreateByText.RerankingModel(
                    "",
                    ""
                ),
                4,
                false,
                0.9f
            ),
            "",
            ""
        ));

    var document = response.Document;

    return new
    {
        document
    };
});

app.MapGet("/ChatApiDemo/ChatMessagesBlocking", async (IServiceProvider sp) =>
{
    // get chat client instance by name in configuration
    var client = sp.GetRequiredKeyedService<ChatClient>("chat"); 
    
    // send chat message in blocking mode
    var response = await client.PostChatMessageBlocking(new ChatMessage.RequestBody
    {
        Query        = "ping",
        ResponseMode = ChatMessage.ResponseMode.Blocking,
        User         = "test-user"
    });

    return new
    {
        response.Answer,
    };
});

app.Run();