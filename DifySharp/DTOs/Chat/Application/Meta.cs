using System.Text.Json;

namespace DifySharp.Chat.Application;

public class Meta
{
    public record ResponseBody(
        JsonElement ToolIcons
    );

    // public record ToolIcon();
}