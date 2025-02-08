namespace DifySharp.Chat.Conversations;

public class Delete
{
    public record RequestBody(string User);

    public record ResponseBody(string Result);
}