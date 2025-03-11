using System.Text.Json;
using System.Text.Json.Serialization;
using DifySharp;
using DifySharp.Chat.ChatMessages;
using DifySharp.Extensions;
using DifySharp.KnowledgeBase;
using DifySharp.KnowledgeBase.Document;
using WebApiClientCore.Parameters;

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

var group          = app.MapGroup("example");
var knowledgeGroup = group.MapGroup("knowledge");

knowledgeGroup.MapGet("create_file_by_text", async (IServiceProvider sp) =>
{
    var api = sp
        .GetRequiredKeyedService<KnowledgeBaseClient>("knowledge"); // get client instance by name in configuration

    var uuid = Guid.NewGuid().ToString("N")[..6];

    var response = await api.PostCreateDocumentByTextAsync(
        "<your-dataset-id>", // add a dataset id here
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

knowledgeGroup.MapGet("create_file_by_file", async (IServiceProvider sp) =>
{
    var api = sp
        .GetRequiredKeyedService<KnowledgeBaseClient>("knowledge"); // get client instance by name in configuration

    var uuid = Guid.NewGuid().ToString("N")[..6];

    var tmpFile = Path.GetTempFileName();
    await File.WriteAllTextAsync(tmpFile, "Test Content");

    var _defaultProcessRule = new ProcessRule(
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
    );

    // var dataJsonStr =
    //     "{\"indexing_technique\":\"high_quality\",\"process_rule\":{\"rules\":{\"pre_processing_rules\":[{\"id\":\"remove_extra_spaces\",\"enabled\":true},{\"id\":\"remove_urls_emails\",\"enabled\":true}],\"segmentation\":{\"separator\":\"###\",\"max_tokens\":500}},\"mode\":\"custom\"}}";

    var data = new CreateByFile.Data(
        null,
        IndexingTechnique.Economy,
        DocForm.TextModel,
        "",
        _defaultProcessRule
    );
    
    var response = await api.PostCreateDocumentByFileAsync( // add a dataset id here
        "<your-dataset-id>", data, new FormDataFile("<your-file-path>"));

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
        Query = "ping",
        User  = "test-user"
    });

    return new
    {
        response.Answer,
    };
});

app.Run();