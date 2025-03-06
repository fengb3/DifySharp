namespace DifySharp.Completion.Application;

public class Basic
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="Name">application name</param>
    /// <param name="Description">description</param>
    /// <param name="Tags">tags</param>
    public record ResponseBody(string Name, String Description, ICollection<string> Tags);
}