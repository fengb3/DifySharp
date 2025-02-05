// See https://aka.ms/new-console-template for more information

using DifySharp;
using DifySharp.KnowledgeBase.Dataset;

Console.WriteLine("Hello, World!");


// knowledge base api

var knowledgeBaseClient = new KnowledgeBaseClient("your-api-key", "https://api.dify.ai/v1");

var response1 = await knowledgeBaseClient.PostCreateDatasetAsync(
	new Create.RequestBody(
		"Dataset Name",
		"this is a test dataset"
	)
);

// completion api

var complietionClient = new CompletionClient("your-api-key");

var response2 = await complietionClient.PostCompletionMessages(new());