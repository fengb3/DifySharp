namespace DifySharp.Chat.Messages;

public record PostFeedback
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="Rating">Upvote as like, downvote as dislike, revoke upvote as null</param>
    /// <param name="User">User identifier, defined by the developer's rules, must be unique within the application.</param>
    /// <param name="Content">The specific content of message feedback.</param>
    public record RequestBody(
        string? Rating,
        string  User,
        string  Content
    );

    /// <summary>
    /// 
    /// </summary>
    /// <param name="Result">Always returns "success"</param>
    public record ResponseBody(
        string Result
    );
}