namespace DifySharp.KnowledgeBase.Dataset;

public class Get
{
	public record ResponseBody(
		ICollection<Dataset> Data,
		bool                 HasMore,
		int                  Limit,
		int                  Total,
		int                  Page
	);
}