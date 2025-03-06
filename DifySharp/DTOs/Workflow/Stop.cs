namespace DifySharp.Workflow.Run;

public class Stop
{
    /// <param name="Result">固定返回 "success"</param>
    public record ResponseBody(string Result);
}