using DifySharp.Apis;
using DifySharp.KnowledgeBase;
using DifySharp.KnowledgeBase.Chunk;
using DifySharp.KnowledgeBase.Dataset;
using DifySharp.KnowledgeBase.Document;
using JetBrains.Annotations;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using DatasetCreate = DifySharp.KnowledgeBase.Dataset.Create;

// using Create = DifySharp.KnowledgeBase.Chunk.Create;

namespace DifySharp.Test.Apis.KnowledgeBaseApiTest;

public class ChunkApiTestFixture : ApiTestFixture
{
	public Dataset  Dataset  { get; private set; }
	public Document Document { get; set; }

	public KnowledgeBaseClient Client { get; set; }

	public ChunkApiTestFixture()
	{
		Client = ServiceProvider.GetRequiredKeyedService<KnowledgeBaseClient>("knowledge");

		// create a dataset
		var uuid = Guid.NewGuid().ToString("N")[..6];
		Dataset =
			Client.PostCreateDatasetAsync(new DatasetCreate.RequestBody(Name: $"Chunk Api Test Dataset {uuid}"))
			      .GetAwaiter()
			      .GetResult();

		Assert.NotNull(Dataset);

		// crate a document
		var createDocRequestBody = new CreateByText.RequestBody(
			Name: $"Chunk Api Test Document {uuid}",
			Text: "Test Content \n\n Test Content \n\n Test Content",
			DocType: null,
			DocMetadata: null,
			IndexingTechnique: IndexingTechnique.Economy,
			DocForm: DocForm.TextModel,
			DocLanguage: "",
			ProcessRule: new ProcessRule(
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
					new SubchunkSegmentation(
						"\n\n",
						1000,
						200
					)
				)
			),
			RetrievalModel: new CreateByText.RetrievalModel(
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
			EmbeddingModel: "",
			EmbeddingModelProvider: ""
		);

		Document = Client.PostCreateDocumentByTextAsync(
			                  Dataset.Id,
			                  createDocRequestBody
		                  )
		                 .GetAwaiter()
		                 .GetResult().Document;

		Assert.NotNull(Document);
	}

	public override void Dispose()
	{
		Client.DeleteDataset(Dataset.Id).GetAwaiter().GetResult();
		Client.Dispose();
		base.Dispose();
	}
}

[TestSubject(typeof(IChunkApi))]
public class ChunkApiTest(
	ChunkApiTestFixture      fixture,
	ILogger<DocumentApiTest> logger
) : IClassFixture<ChunkApiTestFixture>
{
	public Dataset  Dataset  => fixture.Dataset;
	public Document Document => fixture.Document;

	public IKnowledgeBaseApi Client => fixture.Client;


	[Fact]
	public async Task TestGetChunk()
	{
		Assert.NotNull(Dataset);
		Assert.NotNull(Document);
		Assert.NotNull(Client);

		// var response = await Client.GetSegments(Dataset.Id, Document.Id);
		//
		// Assert.NotNull(response);

	}
}