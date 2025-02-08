namespace DifySharp.Chat.Conversations;

public record Rename
{
    public record RequestBody(string Name, bool AutoGenerate, string User);

    // public record ResponseBody() : Conversation();
}