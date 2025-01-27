using System.Diagnostics;
using DifySharp.Apis;
using DifySharp.KnowledgeBase;
using DifySharp.KnowledgeBase.Chunk;
using DifySharp.KnowledgeBase.Dataset;
using DifySharp.KnowledgeBase.Document;
using DifySharp.Test.Apis.KnowledgeBaseApiTest;
using JetBrains.Annotations;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using ChunkCreate = DifySharp.KnowledgeBase.Chunk.Create;
using DatasetCreate = DifySharp.KnowledgeBase.Dataset.Create;

// using Create = DifySharp.KnowledgeBase.Chunk.Create;

namespace DifySharp.Test.Apis;

public class ChunkApiTestFixture : KnowledgeBaseApiTestFixture
{
	public Dataset  Dataset  { get; private set; }
	public Document Document { get; set; }

	public ChunkApiTestFixture() : base()
	{
		var api = Services.GetRequiredService<IKnowledgeBaseApi>();

		// create a dataset
		var uuid = Guid.NewGuid().ToString("N")[..6];
		Dataset =
			api.PostCreateDatasetAsync(new DatasetCreate.RequestBody(Name: $"Test Dataset {uuid}"))
			   .GetAwaiter()
			   .GetResult();

		Assert.NotNull(Dataset);

		// crate a document
		var createDocRequestBody = new CreateByText.RequestBody(
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
		);

		Document = api.PostCreateDocumentByTextAsync(
			               Dataset.Id,
			               createDocRequestBody
		               )
		              .GetAwaiter()
		              .GetResult().Document;

		Assert.NotNull(Document);
	}

	public override void Dispose()
	{
		// var api = Services.GetRequiredService<IKnowledgeBaseApi>();
		// api.DeleteDataset(Dataset.Id).GetAwaiter().GetResult();
		base.Dispose();
	}
}

[TestSubject(typeof(IChunkApi))]
public class ChunkApiTest(
	ChunkApiTestFixture      fixture,
	ILogger<DocumentApiTest> logger,
	IKnowledgeBaseApi        api
) : IClassFixture<ChunkApiTestFixture>
{
	public Dataset  Dataset  => fixture.Dataset;
	public Document Document => fixture.Document;


	[Fact]
	public async Task TestCreateChunk()
	{
		Assert.NotNull(Dataset);
		Assert.NotNull(Document);

		// var response = await api.PostCreateSegmentAsync(
		// 	Dataset.Id,
		// 	Document.Id,
		// 	new ChunkCreate.RequestBody(
		// 		new ChunkCreate.Segment("chunk1", null, "keyword1", "keyword2"),
		// 		new ChunkCreate.Segment("chunk3", null, "keyword2", "keyword8"),
		// 		new ChunkCreate.Segment("chunk5", null, "keyword4", "keyword5"),
		// 		new ChunkCreate.Segment("chunk7", null, "keyword5", "keyword7")
		// 	));
		//
		// Assert.NotNull(response);
		// Assert.NotEmpty(response.Data);
	}
}