namespace DifySharp.Chat.Messages;

public record GetSuggested
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="Result"></param>
    /// <param name="Data"></param>
    public record ResponseBody(string Result, ICollection<string> Data);
}