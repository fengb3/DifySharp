using DifySharp.KnowledgeBase;
using DifySharp.KnowledgeBase.Dataset;
using DifySharp.KnowledgeBase.Document;
using JetBrains.Annotations;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace DifySharp.Test.Apis.KnowledgeBaseApiTest;

public class DocumentApiTestFixture : KnowledgeBaseApiTestFixture
{
	public Dataset             Dataset { get; private set; }
	public KnowledgeBaseClient Client  { get; }

	public DocumentApiTestFixture()
	{
		Client = ServiceProvider.GetRequiredKeyedService<KnowledgeBaseClient>("knowledge");

		// create a dataset
		var uuid = Guid.NewGuid().ToString("N")[..6];
		Dataset =
			Client.PostCreateDatasetAsync(new Create.RequestBody(Name: $"Test Dataset {uuid}"))
			      .GetAwaiter()
			      .GetResult();
	}

	public override void Dispose()
	{
		Client.DeleteDataset(Dataset.Id).GetAwaiter().GetResult();
		Client.Dispose();
		base.Dispose();
	}
}

[TestSubject(typeof(IDocumentApi))]
public class DocumentApiTest(
	DocumentApiTestFixture   fixture,
	ILogger<DocumentApiTest> logger
) : IClassFixture<DocumentApiTestFixture>
{
	private static Document? Document { get; set; }
	private        Dataset   Dataset  => fixture.Dataset;

	private KnowledgeBaseClient Client => fixture.Client;

	[Fact, TestPriority(1)]
	public async Task TestCreateDocumentByText_ShouldHaveDocumentInfoInResponse()
	{
		Assert.Null(Document);

		var uuid = Guid.NewGuid().ToString("N")[..6];

		var response = await Client.PostCreateDocumentByTextAsync(
			Dataset.Id,
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


		Assert.NotNull(response.Document);
		Assert.NotEmpty(response.Document.Id);
		Document = response.Document;
	}

	[Fact, TestPriority(2)]
	public async Task TestListDocument_ShouldContainsDocumentInDataset()
	{
		Assert.NotNull(Document);

		var response = await Client.GetDocuments(Dataset.Id);

		var documents = response.Data;

		Assert.Contains(documents, doc => doc.Id == Document.Id);
	}

	[Fact, TestPriority(3)]
	public async Task TestDeleteDocument_ShouldNotContainsDocumentInDataset()
	{
		Assert.NotNull(Document);

		var response = await Client.DeleteDocument(Dataset.Id, Document.Id);
		Assert.Equal("success", response.Result);

		var documents = await Client.GetDocuments(Dataset.Id);
		Assert.DoesNotContain(documents.Data, doc => doc.Id == Document.Id);
	}
}