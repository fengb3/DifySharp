using DifySharp;
using JetBrains.Annotations;

namespace DifySharp.Test;

[TestSubject(typeof(DifyClient))]
public class DifyClientTest
{
	[Fact]
	public async Task TestDifyClient()
	{
		// var client = new KnowledgeBaseClient(new ApiKeyOptions("apikey", "test-name", DifyApiType.KNOWLEDGE_BASE));
		//
		// await client.Test();
	}
}