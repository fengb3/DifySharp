using DifySharp.Apis;
using DifySharp.KnowledgeBase;
using DifySharp.KnowledgeBase.Dataset;
using DifySharp.KnowledgeBase.Document;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDifySdk(o => { o.KnowledgeBaseApiKey = ""; }); // add you api key here

var app = builder.Build();

app.MapGet("/", async (IKnowledgeBaseApi api) =>
{
    var uuid = Guid.NewGuid().ToString("N")[..6];
    // var dataset = await api.PostCreateDatasetAsync(new Create.RequestBody(Name: $"test-dataset-{uuid}"));

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

app.Run();