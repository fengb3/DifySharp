namespace DifySharp.Chat.Messages;

public class Get
{
    /// <summary>
    /// response body
    /// </summary>
    /// <param name="Messages"></param>
    /// <param name="HasMore"></param>
    /// <param name="Limit"></param>
    public record ResponseBody(
        ICollection<Message> Messages,
        bool                 HasMore,
        int                  Limit
    );
}

/// <summary>
/// 
/// </summary>
/// <param name="Id">Message ID</param>
/// <param name="ConversationId">Conversation ID</param>
/// <param name="Inputs">User input parameters</param>
/// <param name="Query">User input / question content</param>
/// <param name="MessageFiles">Message files</param>
/// <param name="Answer">Response message content</param>
/// <param name="CreatedAt">Creation timestamp, e.g., 1705395332</param>
/// <param name="Feedback">Feedback information</param>
public record Message(
    string            Id,
    string            ConversationId,
    object            Inputs,
    string            Query,
    List<MessageFile> MessageFiles,
    string            Answer,
    long              CreatedAt,
    Feedback          Feedback
);

/// <summary>
/// Message files
/// </summary>
/// <param name="Id">ID</param>
/// <param name="Type">File Type</param>
/// <param name="Url">Preview image URL</param>
/// <param name="BelongsTo">belongs to，user orassistant</param>
public record MessageFile(
    string Id,
    string Type,
    string Url,
    string BelongsTo
);

/// <summary>
/// Feedback information
/// </summary>
/// <param name="Rating">Upvote as like / Downvote as dislike</param>
public record Feedback(
    string Rating
);

public record RetrieverResource(
    int    Position,
    string DatasetId,
    string DatasetName,
    string DocumentId,
    string DocumentName,
    string SegmentId,
    double Score,
    string Content
);