namespace AlertSuggestion.ChatHub.DifyClient.DTOs;

public record BaseResponse;

public record ErrorResponse(string Code, string Message, string Status) : BaseResponse;

// public class KnowledgeBase
// {
// }