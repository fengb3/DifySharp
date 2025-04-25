namespace DifySharp.Workflow;

public class Stop
{
    /// <param name="result">固定返回 "success"</param>
    public class ResponseBody(string result)
    {
        /// <summary>固定返回 "success"</summary>
        public string Result { get; init; } = result;

        public void Deconstruct(out string result)
        {
            result = this.Result;
        }
    }
}