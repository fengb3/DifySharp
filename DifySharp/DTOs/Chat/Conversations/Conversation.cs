namespace DifySharp.Chat.Conversations;

/// <summary>
/// conversation info
/// </summary>
/// <param name="Id">Conversation Id</param>
/// <param name="Name">Conversation Name</param>
/// <param name="Inputs">User input parameters.</param>
/// <param name="Status">Conversation status</param>
/// <param name="Introduction">Introduction</param>
/// <param name="CreatedAt">Creation timestamp, e.g., 1705395332</param>
/// <param name="UpdatedAt">Update timestamp, e.g., 1705395332</param>
public record Conversation(
    string Id,
    string Name,
    object Inputs,
    string Status,
    string Introduction,
    long   CreatedAt,
    long   UpdatedAt
);