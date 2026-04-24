using System.Text.Json;
using System.Text.Json.Serialization;

namespace DifySharp.Cli;

[JsonSourceGenerationOptions(
    WriteIndented = false,
    DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
    PropertyNamingPolicy = JsonKnownNamingPolicy.SnakeCaseLower,
    GenerationMode = JsonSourceGenerationMode.Default)]
[JsonSerializable(typeof(ApplicationResponseMode))]
[JsonSerializable(typeof(ChunkCompletionResponseBody))]
[JsonSerializable(typeof(MessageEvent))]
[JsonSerializable(typeof(AgentMessageEvent))]
[JsonSerializable(typeof(AgentThoughtEvent))]
[JsonSerializable(typeof(MessageFileEvent))]
[JsonSerializable(typeof(MessageEndEvent))]
[JsonSerializable(typeof(MessageReplaceEvent))]
[JsonSerializable(typeof(WorkflowStartedEvent))]
[JsonSerializable(typeof(NodeStartedEvent))]
[JsonSerializable(typeof(NodeFinishedEvent))]
[JsonSerializable(typeof(WorkflowFinishedEvent))]
[JsonSerializable(typeof(ErrorEvent))]
[JsonSerializable(typeof(PingEvent))]
// Chat
[JsonSerializable(typeof(Chat.ChatMessages.ChatMessage.RequestBody))]
[JsonSerializable(typeof(Chat.ChatMessages.ChatMessage.ResponseBody))]
[JsonSerializable(typeof(Chat.ChatMessages.ChatMessage.File))]
[JsonSerializable(typeof(Chat.Conversations.Get.ResponseBody))]
[JsonSerializable(typeof(Chat.Conversations.Conversation))]
[JsonSerializable(typeof(Chat.Application.Basic.ResponseBody))]
[JsonSerializable(typeof(Chat.Application.Parameters.ResponseBody))]
[JsonSerializable(typeof(Chat.Application.Meta.ResponseBody))]
// Completion
[JsonSerializable(typeof(Completion.CompletionMessages.CompletionMessages.RequestBody))]
[JsonSerializable(typeof(Completion.CompletionMessages.CompletionMessages.ResponseBody))]
[JsonSerializable(typeof(Completion.Application.Basic.ResponseBody))]
[JsonSerializable(typeof(Completion.Application.Parameters.ResponseBody))]
// Workflow
[JsonSerializable(typeof(Workflow.Run.RequestBody))]
[JsonSerializable(typeof(Workflow.Run.ResponseBody))]
[JsonSerializable(typeof(Workflow.Run.Data))]
[JsonSerializable(typeof(Workflow.Run.File))]
// Knowledge Base - Dataset
[JsonSerializable(typeof(KnowledgeBase.Dataset.Create.RequestBody))]
[JsonSerializable(typeof(KnowledgeBase.Dataset.Get.ResponseBody))]
[JsonSerializable(typeof(KnowledgeBase.Dataset.Dataset))]
[JsonSerializable(typeof(KnowledgeBase.Dataset.Create.Permission))]
[JsonSerializable(typeof(KnowledgeBase.Dataset.Create.Provider))]
// Knowledge Base - Document
[JsonSerializable(typeof(KnowledgeBase.Document.CreateByText.RequestBody))]
[JsonSerializable(typeof(KnowledgeBase.Document.CreateByText.ResponseBody))]
[JsonSerializable(typeof(KnowledgeBase.Document.CreateByText.SearchMethod))]
[JsonSerializable(typeof(KnowledgeBase.Document.Get.ResponseBody))]
[JsonSerializable(typeof(KnowledgeBase.Document.Document))]
[JsonSerializable(typeof(KnowledgeBase.Document.DataSourceInfo))]
// Knowledge Base - Chunk
[JsonSerializable(typeof(KnowledgeBase.Chunk.Create.RequestBody))]
[JsonSerializable(typeof(KnowledgeBase.Chunk.Create.Segment))]
[JsonSerializable(typeof(KnowledgeBase.Chunk.Create.ResponseBody))]
[JsonSerializable(typeof(KnowledgeBase.Chunk.Get.ResponseBody))]
[JsonSerializable(typeof(KnowledgeBase.Chunk.Chunk))]
// Knowledge Base - shared
[JsonSerializable(typeof(KnowledgeBase.IndexingTechnique))]
[JsonSerializable(typeof(KnowledgeBase.DocForm))]
[JsonSerializable(typeof(KnowledgeBase.ProcessRule))]
[JsonSerializable(typeof(KnowledgeBase.Rules))]
[JsonSerializable(typeof(KnowledgeBase.PreProcessingRule))]
[JsonSerializable(typeof(KnowledgeBase.Segmentation))]
[JsonSerializable(typeof(KnowledgeBase.SubchunkSegmentation))]
// Misc
[JsonSerializable(typeof(object))]
public partial class CliJsonContext : JsonSerializerContext;
