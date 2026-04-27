using System.CommandLine;
using System.CommandLine.Invocation;
using System.Text;
using System.Text.Json;
using DifySharp.KnowledgeBase;
using DifySharp.KnowledgeBase.Chunk;
using DifySharp.KnowledgeBase.Dataset;
using DifySharp.KnowledgeBase.Document;

namespace DifySharp.Cli.Commands;

public static class KnowledgeBaseCommand
{
    public static Command Create(Option<string> apiKeyOption, Option<string> baseUrlOption)
    {
        // --- datasets sub-commands ---
        var datasetsCommand = new Command("datasets", "Dataset operations");

        // datasets list
        var pageOption = new Option<int>("--page", () => 1, "Page number");
        var limitOption = new Option<int>("--limit", () => 20, "Items per page");

        var datasetsListCommand = new Command("list", "List datasets")
        {
            pageOption, limitOption
        };

        datasetsListCommand.SetHandler((Func<InvocationContext, Task>)(async context =>
        {
            var page = context.ParseResult.GetValueForOption(pageOption);
            var limit = context.ParseResult.GetValueForOption(limitOption);
            var apiKey = context.ParseResult.GetValueForOption(apiKeyOption)!;
            var baseUrl = context.ParseResult.GetValueForOption(baseUrlOption)!;

            using var client = new DifyApiClient(apiKey, baseUrl);
            var result = await client.GetAsync<DifySharp.KnowledgeBase.Dataset.Get.ResponseBody>(
                $"/v1/datasets?page={page}&limit={limit}", context.GetCancellationToken());
            Console.WriteLine(JsonSerializer.Serialize(result, DifyApiClient.WriteIndentedOptions));
        }));

        // datasets create
        var nameOption = new Option<string>("--name", "Dataset name") { IsRequired = true };
        var descriptionOption = new Option<string?>("--description", () => null, "Dataset description");

        var datasetsCreateCommand = new Command("create", "Create a dataset")
        {
            nameOption, descriptionOption
        };

        datasetsCreateCommand.SetHandler((Func<InvocationContext, Task>)(async context =>
        {
            var name = context.ParseResult.GetValueForOption(nameOption)!;
            var description = context.ParseResult.GetValueForOption(descriptionOption);
            var apiKey = context.ParseResult.GetValueForOption(apiKeyOption)!;
            var baseUrl = context.ParseResult.GetValueForOption(baseUrlOption)!;

            using var client = new DifyApiClient(apiKey, baseUrl);
            var body = new DifySharp.KnowledgeBase.Dataset.Create.RequestBody(name, description);
            var result = await client.PostAsync<DifySharp.KnowledgeBase.Dataset.Create.RequestBody, Dataset>(
                "/v1/datasets", body, context.GetCancellationToken());
            Console.WriteLine(JsonSerializer.Serialize(result, DifyApiClient.WriteIndentedOptions));
        }));

        // datasets delete
        var datasetIdArgument = new Argument<string>("dataset-id", "Dataset ID");

        var datasetsDeleteCommand = new Command("delete", "Delete a dataset")
        {
            datasetIdArgument
        };

        datasetsDeleteCommand.SetHandler((Func<InvocationContext, Task>)(async context =>
        {
            var datasetId = context.ParseResult.GetValueForArgument(datasetIdArgument);
            var apiKey = context.ParseResult.GetValueForOption(apiKeyOption)!;
            var baseUrl = context.ParseResult.GetValueForOption(baseUrlOption)!;

            using var client = new DifyApiClient(apiKey, baseUrl);
            await client.DeleteAsync($"/v1/datasets/{datasetId}", context.GetCancellationToken());
            Console.WriteLine($"Dataset {datasetId} deleted.");
        }));

        datasetsCommand.Add(datasetsListCommand);
        datasetsCommand.Add(datasetsCreateCommand);
        datasetsCommand.Add(datasetsDeleteCommand);

        // --- documents sub-commands ---
        var documentsCommand = new Command("documents", "Document operations");

        var datasetIdOption = new Option<string>("--dataset-id", "Dataset ID") { IsRequired = true };
        var docPageOption = new Option<int>("--page", () => 1, "Page number");
        var docLimitOption = new Option<int>("--limit", () => 20, "Items per page");

        // documents list
        var docsListCommand = new Command("list", "List documents in a dataset")
        {
            datasetIdOption, docPageOption, docLimitOption
        };

        docsListCommand.SetHandler((Func<InvocationContext, Task>)(async context =>
        {
            var datasetId = context.ParseResult.GetValueForOption(datasetIdOption)!;
            var page = context.ParseResult.GetValueForOption(docPageOption);
            var limit = context.ParseResult.GetValueForOption(docLimitOption);
            var apiKey = context.ParseResult.GetValueForOption(apiKeyOption)!;
            var baseUrl = context.ParseResult.GetValueForOption(baseUrlOption)!;

            using var client = new DifyApiClient(apiKey, baseUrl);
            var result = await client.GetAsync<DifySharp.KnowledgeBase.Document.Get.ResponseBody>(
                $"/v1/datasets/{datasetId}/documents?page={page}&limit={limit}", context.GetCancellationToken());
            Console.WriteLine(JsonSerializer.Serialize(result, DifyApiClient.WriteIndentedOptions));
        }));

        // documents create-text
        var docNameOption = new Option<string>("--name", "Document name") { IsRequired = true };
        var docTextOption = new Option<string>("--text", "Document text content") { IsRequired = true };
        var indexingTechniqueOption = new Option<string?>("--indexing-technique", () => null,
            "Indexing technique: high_quality or economy");
        var docFormOption = new Option<string?>("--doc-form", () => null,
            "Doc form: text_model, hierarchical_model, qa_model");

        var docsCreateTextCommand = new Command("create-text", "Create a document from text")
        {
            datasetIdOption, docNameOption, docTextOption, indexingTechniqueOption, docFormOption
        };

        docsCreateTextCommand.SetHandler((Func<InvocationContext, Task>)(async context =>
        {
            var datasetId = context.ParseResult.GetValueForOption(datasetIdOption)!;
            var name = context.ParseResult.GetValueForOption(docNameOption)!;
            var text = context.ParseResult.GetValueForOption(docTextOption)!;
            var indexingTechniqueStr = context.ParseResult.GetValueForOption(indexingTechniqueOption);
            var docFormStr = context.ParseResult.GetValueForOption(docFormOption);
            var apiKey = context.ParseResult.GetValueForOption(apiKeyOption)!;
            var baseUrl = context.ParseResult.GetValueForOption(baseUrlOption)!;

            IndexingTechnique? indexingTechnique = indexingTechniqueStr switch
            {
                "high_quality" => IndexingTechnique.HighQuality,
                "economy" => IndexingTechnique.Economy,
                null => null,
                _ => throw new ArgumentException($"Invalid indexing technique: {indexingTechniqueStr}")
            };

            DocForm? docForm = docFormStr switch
            {
                "text_model" => DocForm.TextModel,
                "hierarchical_model" => DocForm.HierarchicalModel,
                "qa_model" => DocForm.QaModel,
                null => null,
                _ => throw new ArgumentException($"Invalid doc form: {docFormStr}")
            };

            using var client = new DifyApiClient(apiKey, baseUrl);
            var body = new CreateByText.RequestBody(name, text,
                IndexingTechnique: indexingTechnique, DocForm: docForm);
            var result = await client.PostAsync<CreateByText.RequestBody, CreateByText.ResponseBody>(
                $"/v1/datasets/{datasetId}/document/create_by_text", body, context.GetCancellationToken());
            Console.WriteLine(JsonSerializer.Serialize(result, DifyApiClient.WriteIndentedOptions));
        }));

        // documents delete
        var documentIdOption = new Option<string>("--document-id", "Document ID") { IsRequired = true };

        var docsDeleteCommand = new Command("delete", "Delete a document")
        {
            datasetIdOption, documentIdOption
        };

        docsDeleteCommand.SetHandler((Func<InvocationContext, Task>)(async context =>
        {
            var datasetId = context.ParseResult.GetValueForOption(datasetIdOption)!;
            var documentId = context.ParseResult.GetValueForOption(documentIdOption)!;
            var apiKey = context.ParseResult.GetValueForOption(apiKeyOption)!;
            var baseUrl = context.ParseResult.GetValueForOption(baseUrlOption)!;

            using var client = new DifyApiClient(apiKey, baseUrl);
            await client.DeleteAsync($"/v1/datasets/{datasetId}/documents/{documentId}", context.GetCancellationToken());
            Console.WriteLine($"Document {documentId} deleted.");
        }));

        documentsCommand.Add(docsListCommand);
        documentsCommand.Add(docsCreateTextCommand);
        documentsCommand.Add(docsDeleteCommand);

        // --- chunks sub-commands ---
        var chunksCommand = new Command("chunks", "Chunk operations");

        var chunkDatasetIdOption = new Option<string>("--dataset-id", "Dataset ID") { IsRequired = true };
        var chunkDocumentIdOption = new Option<string>("--document-id", "Document ID") { IsRequired = true };

        // chunks list
        var chunksListCommand = new Command("list", "List chunks in a document")
        {
            chunkDatasetIdOption, chunkDocumentIdOption
        };

        chunksListCommand.SetHandler((Func<InvocationContext, Task>)(async context =>
        {
            var datasetId = context.ParseResult.GetValueForOption(chunkDatasetIdOption)!;
            var documentId = context.ParseResult.GetValueForOption(chunkDocumentIdOption)!;
            var apiKey = context.ParseResult.GetValueForOption(apiKeyOption)!;
            var baseUrl = context.ParseResult.GetValueForOption(baseUrlOption)!;

            using var client = new DifyApiClient(apiKey, baseUrl);
            var result = await client.GetAsync<DifySharp.KnowledgeBase.Chunk.Get.ResponseBody>(
                $"/v1/datasets/{datasetId}/documents/{documentId}/segments", context.GetCancellationToken());
            Console.WriteLine(JsonSerializer.Serialize(result, DifyApiClient.WriteIndentedOptions));
        }));

        // chunks create
        var chunkContentOption = new Option<string>("--content", "Chunk content") { IsRequired = true };

        var chunksCreateCommand = new Command("create", "Create a chunk")
        {
            chunkDatasetIdOption, chunkDocumentIdOption, chunkContentOption
        };

        chunksCreateCommand.SetHandler((Func<InvocationContext, Task>)(async context =>
        {
            var datasetId = context.ParseResult.GetValueForOption(chunkDatasetIdOption)!;
            var documentId = context.ParseResult.GetValueForOption(chunkDocumentIdOption)!;
            var content = context.ParseResult.GetValueForOption(chunkContentOption)!;
            var apiKey = context.ParseResult.GetValueForOption(apiKeyOption)!;
            var baseUrl = context.ParseResult.GetValueForOption(baseUrlOption)!;

            using var client = new DifyApiClient(apiKey, baseUrl);
            var body = new DifySharp.KnowledgeBase.Chunk.Create.RequestBody(
                new DifySharp.KnowledgeBase.Chunk.Create.Segment(content, null)
            );
            var result = await client.PostAsync<DifySharp.KnowledgeBase.Chunk.Create.RequestBody, DifySharp.KnowledgeBase.Chunk.Create.ResponseBody>(
                $"/v1/datasets/{datasetId}/documents/{documentId}/segments", body, context.GetCancellationToken());
            Console.WriteLine(JsonSerializer.Serialize(result, DifyApiClient.WriteIndentedOptions));
        }));

        chunksCommand.Add(chunksListCommand);
        chunksCommand.Add(chunksCreateCommand);

        // --- retrieve sub-command ---
        var retrieveDatasetIdOption = new Option<string>("--dataset-id", "Dataset ID") { IsRequired = true };
        var queryOption = new Option<string>("--query", "Search query") { IsRequired = true };
        var topKOption = new Option<int>("--top-k", () => 3, "Number of results to return");
        var searchMethodOption = new Option<string>("--search-method", () => "semantic_search",
            "Search method: semantic_search, full_text_search, hybrid_search");

        var retrieveCommand = new Command("retrieve", "Search/retrieve from a dataset")
        {
            retrieveDatasetIdOption, queryOption, topKOption, searchMethodOption
        };

        retrieveCommand.SetHandler((Func<InvocationContext, Task>)(async context =>
        {
            var datasetId = context.ParseResult.GetValueForOption(retrieveDatasetIdOption)!;
            var query = context.ParseResult.GetValueForOption(queryOption)!;
            var topK = context.ParseResult.GetValueForOption(topKOption);
            var searchMethod = context.ParseResult.GetValueForOption(searchMethodOption)!;
            var apiKey = context.ParseResult.GetValueForOption(apiKeyOption)!;
            var baseUrl = context.ParseResult.GetValueForOption(baseUrlOption)!;

            using var client = new DifyApiClient(apiKey, baseUrl);
            var body = new Dictionary<string, object>
            {
                ["query"] = query,
                ["retrieval_model"] = new Dictionary<string, object>
                {
                    ["search_method"] = searchMethod,
                    ["reranking_enable"] = false,
                    ["top_k"] = topK,
                    ["score_threshold_enabled"] = false
                }
            };
            var response = await client.PostRawAsync(
                $"/v1/datasets/{datasetId}/retrieve", body, context.GetCancellationToken());
            var content = await response.Content.ReadAsStringAsync(context.GetCancellationToken());
            using var doc = JsonDocument.Parse(content);
            using var stream = new MemoryStream();
            using (var writer = new Utf8JsonWriter(stream, new JsonWriterOptions { Indented = true }))
            {
                doc.WriteTo(writer);
            }
            Console.WriteLine(Encoding.UTF8.GetString(stream.ToArray()));
        }));

        var command = new Command("kb", "Knowledge base operations")
        {
            datasetsCommand, documentsCommand, chunksCommand, retrieveCommand
        };

        return command;
    }
}
