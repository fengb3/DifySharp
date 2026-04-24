using System.Text.Json;

namespace DifySharp.Cli.Helpers;

public static class StreamingHelper
{
    public static async Task<int> WriteStreamToConsole(
        IAsyncEnumerable<ChunkCompletionResponseBody> stream,
        CancellationToken ct = default)
    {
        await foreach (var chunk in stream.WithCancellation(ct))
        {
            switch (chunk)
            {
                case MessageEvent msg:
                    Console.Write(msg.Answer);
                    break;
                case AgentMessageEvent agentMsg:
                    Console.Write(agentMsg.Answer);
                    break;
                case AgentThoughtEvent thought:
                    Console.WriteLine($"\n[Agent Thought] {thought.Thought}");
                    break;
                case MessageEndEvent:
                    Console.WriteLine();
                    break;
                case ErrorEvent err:
                    Console.Error.WriteLine($"\nError: {err.Message} (code: {err.Code})");
                    return 1;
                case PingEvent:
                    break;
                case WorkflowStartedEvent ws:
                    Console.WriteLine($"[Workflow Started] run_id={ws.WorkflowRunId}");
                    break;
                case NodeStartedEvent ns:
                    if (ns.Data != null)
                        Console.WriteLine($"  [Node] {ns.Data.Title} ({ns.Data.NodeType})...");
                    break;
                case NodeFinishedEvent nf:
                    if (nf.Data != null)
                        Console.WriteLine($"  [Node] {nf.Data.NodeId} - {nf.Data.Status}");
                    break;
                case WorkflowFinishedEvent wf:
                    if (wf.Data != null)
                        Console.WriteLine($"[Workflow Finished] status={wf.Data.Status}");
                    break;
                default:
                    Console.WriteLine(JsonSerializer.Serialize(chunk, DifyApiClient.WriteIndentedOptions));
                    break;
            }
        }

        return 0;
    }
}
