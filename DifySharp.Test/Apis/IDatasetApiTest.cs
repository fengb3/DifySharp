using DifySharp.Apis;
using DifySharp.KnowledgeBase.Dataset;
using JetBrains.Annotations;

namespace DifySharp.Test.Apis;

[TestSubject(typeof(IDatasetApi))]
public class DatasetApiTest(IKnowledgeBaseApi api)
{

	[Fact]
	public void TestCreateDataset()
	{
		
	}
}