namespace DifySharp.Chat.Conversations;

public record Get
{
    public record ResponseBody(
        Conversation[] Data,
        bool           HasMore,
        int            Limit
    );
}