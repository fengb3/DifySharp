using System.CommandLine;
using System.CommandLine.Invocation;
using System.Text.Json;
using DifySharp.Chat.ChatMessages;
using DifySharp.Cli.Helpers;
using ChatApplication = DifySharp.Chat.Application;
using ChatConversations = DifySharp.Chat.Conversations;

namespace DifySharp.Cli.Commands;

public static class ChatCommand
{
    public static Command Create(Option<string> apiKeyOption, Option<string> baseUrlOption)
    {
        var userOption = new Option<string>("--user", () => "cli-user", "User identifier");
        userOption.AddAlias("-u");

        var streamingOption = new Option<bool>("--streaming", "Use streaming mode");
        streamingOption.AddAlias("-s");

        var conversationIdOption = new Option<string?>("--conversation-id", () => null, "Conversation ID for continuing a chat");

        // chat send
        var queryArgument = new Argument<string>("query", "The message to send");

        var sendCommand = new Command("send", "Send a chat message")
        {
            queryArgument, userOption, streamingOption, conversationIdOption
        };

        sendCommand.SetHandler((Func<InvocationContext, Task>)(async context =>
        {
            var query = context.ParseResult.GetValueForArgument(queryArgument);
            var user = context.ParseResult.GetValueForOption(userOption)!;
            var streaming = context.ParseResult.GetValueForOption(streamingOption);
            var convId = context.ParseResult.GetValueForOption(conversationIdOption);
            var apiKey = context.ParseResult.GetValueForOption(apiKeyOption)!;
            var baseUrl = context.ParseResult.GetValueForOption(baseUrlOption)!;

            using var client = new DifyApiClient(apiKey, baseUrl);
            var body = new ChatMessage.RequestBody
            {
                Query = query,
                User = user,
                ConversationId = convId ?? ""
            };

            if (streaming)
            {
                body.ResponseMode = ApplicationResponseMode.Streaming;
                var stream = client.PostStreamAsync("/v1/chat-messages", body, context.GetCancellationToken());
                context.ExitCode = await StreamingHelper.WriteStreamToConsole(stream, context.GetCancellationToken());
            }
            else
            {
                body.ResponseMode = ApplicationResponseMode.Blocking;
                var result = await client.PostAsync<ChatMessage.RequestBody, ChatMessage.ResponseBody>(
                    "/v1/chat-messages", body, context.GetCancellationToken());
                Console.WriteLine(result.Answer);
            }
        }));

        // chat conversations
        var conversationsCommand = new Command("conversations", "List conversations")
        {
            userOption
        };

        var lastIdOption = new Option<string?>("--last-id", () => null, "Last ID for pagination");
        var limitOption = new Option<int?>("--limit", () => null, "Number of records");
        conversationsCommand.AddOption(lastIdOption);
        conversationsCommand.AddOption(limitOption);

        conversationsCommand.SetHandler((Func<InvocationContext, Task>)(async context =>
        {
            var user = context.ParseResult.GetValueForOption(userOption)!;
            var lastId = context.ParseResult.GetValueForOption(lastIdOption);
            var limit = context.ParseResult.GetValueForOption(limitOption);
            var apiKey = context.ParseResult.GetValueForOption(apiKeyOption)!;
            var baseUrl = context.ParseResult.GetValueForOption(baseUrlOption)!;

            using var client = new DifyApiClient(apiKey, baseUrl);
            var queryParams = new List<string> { $"user={Uri.EscapeDataString(user)}" };
            if (lastId != null) queryParams.Add($"last_id={Uri.EscapeDataString(lastId)}");
            if (limit != null) queryParams.Add($"limit={limit}");

            var url = $"/v1/conversations?{string.Join("&", queryParams)}";
            var result = await client.GetAsync<ChatConversations.Get.ResponseBody>(url, context.GetCancellationToken());
            Console.WriteLine(JsonSerializer.Serialize(result, DifyApiClient.WriteIndentedOptions));
        }));

        // chat info
        var infoCommand = new Command("info", "Get application info");
        infoCommand.SetHandler((Func<InvocationContext, Task>)(async context =>
        {
            var apiKey = context.ParseResult.GetValueForOption(apiKeyOption)!;
            var baseUrl = context.ParseResult.GetValueForOption(baseUrlOption)!;

            using var client = new DifyApiClient(apiKey, baseUrl);
            var result = await client.GetAsync<ChatApplication.Basic.ResponseBody>("/v1/info", context.GetCancellationToken());
            Console.WriteLine(JsonSerializer.Serialize(result, DifyApiClient.WriteIndentedOptions));
        }));

        // chat parameters
        var parametersCommand = new Command("parameters", "Get application parameters");
        parametersCommand.SetHandler((Func<InvocationContext, Task>)(async context =>
        {
            var apiKey = context.ParseResult.GetValueForOption(apiKeyOption)!;
            var baseUrl = context.ParseResult.GetValueForOption(baseUrlOption)!;

            using var client = new DifyApiClient(apiKey, baseUrl);
            var result = await client.GetAsync<ChatApplication.Parameters.ResponseBody>("/v1/parameters", context.GetCancellationToken());
            Console.WriteLine(JsonSerializer.Serialize(result, DifyApiClient.WriteIndentedOptions));
        }));

        var command = new Command("chat", "Chat app operations")
        {
            sendCommand, conversationsCommand, infoCommand, parametersCommand
        };

        return command;
    }
}
