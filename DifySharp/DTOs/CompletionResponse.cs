using System.Text.Json;
using System.Text.Json.Serialization;

namespace DifySharp;

#region Response - Streaming Mode

/// <summary>
/// 返回 App 输出的流式块，
/// Content-Type 为 text/event-stream。
/// 每个流式块均为 data: 开头，块之间以 \n\n 即两个换行符分隔，如下所示：
/// <code>
///	data: {
///		"event": "message",
///		"task_id": "900bbd43-dc0b-4383-a372-aa6e6c414227",
///		"id": "663c5084-a254-4040-8ad3-51f2a3c1a77c", "answer": "Hi",
///		"created_at": 1705398420
/// }\n\n
/// </code>
/// 除了event字段，其他字段由event字段内容决定。
/// 见每一个继承的类。
/// </summary>
[JsonPolymorphic(TypeDiscriminatorPropertyName = "event")]
[JsonDerivedType(typeof(MessageEvent), typeDiscriminator: "message")]
[JsonDerivedType(typeof(AgentMessageEvent), typeDiscriminator: "agent_message")]
[JsonDerivedType(typeof(AgentThoughtEvent), typeDiscriminator: "agent_thought")]
[JsonDerivedType(typeof(MessageFileEvent), typeDiscriminator: "message_file")]
[JsonDerivedType(typeof(MessageEndEvent), typeDiscriminator: "message_end")]
[JsonDerivedType(typeof(MessageReplaceEvent), typeDiscriminator: "message_replace")]
[JsonDerivedType(typeof(WorkflowStartedEvent), typeDiscriminator: "workflow_started")]
[JsonDerivedType(typeof(NodeStartedEvent), typeDiscriminator: "node_started")]
[JsonDerivedType(typeof(NodeFinishedEvent), typeDiscriminator: "node_finished")]
[JsonDerivedType(typeof(WorkflowFinishedEvent), typeDiscriminator: "workflow_finished")]
[JsonDerivedType(typeof(ErrorEvent), typeDiscriminator: "error")]
[JsonDerivedType(typeof(PingEvent), typeDiscriminator: "ping")]
[Serializable]
public record ChunkCompletionResponseBody
{
    public const string CHUNK_PREFIX = "data: ";
    public const string CHUNK_SUFFIX = "\n\n";

    /// <summary>
    /// 事件类型 <br/>
    /// possible values:
    /// <list type="bullet">
    /// <item><description>message</description></item>
    /// <item><description>agent_message</description></item>
    /// <item><description>agent_thought</description></item>
    /// <item><description>message_file</description></item>
    /// <item><description>message_end</description></item>
    /// <item><description>message_replace</description></item>
    /// <item><description>workflow_started</description></item>
    /// <item><description>node_started</description></item>
    /// <item><description>node_finished</description></item>
    /// <item><description>workflow_finished</description></item>
    /// <item><description>error</description></item>
    /// <item><description>ping</description></item>
    /// </list>
    /// </summary>
    public string? Event { get; set; }
}

/*
    event: message LLM 返回文本块事件，即：完整的文本以分块的方式输出。
    task_id (string) 任务 ID，用于请求跟踪和下方的停止响应接口
    message_id (string) 消息唯一 ID
    conversation_id (string) 会话 ID
    answer (string) LLM 返回文本块内容
    created_at (int) 创建时间戳，如：1705395332
 */
/// <summary>
/// LLM 返回文本块事件，即：完整的文本以分块的方式输出。when Event is message
/// </summary>
[Serializable]
public record MessageEvent : ChunkCompletionResponseBody
{
    /// <summary>
    /// 任务 ID，用于请求跟踪和下方的停止响应接口
    /// </summary>
    public required string TaskId { get; init; }

    /// <summary>
    /// 消息唯一 ID
    /// </summary>
    public required string MessageId { get; init; }

    /// <summary>
    /// 会话 ID
    /// </summary>
    public required string ConversationId { get; init; }

    /// <summary>
    /// LLM 返回文本块内容
    /// </summary>
    public required string Answer { get; init; }

    /// <summary>
    /// 创建时间戳
    /// </summary>
    public int CreateAt { get; init; }
}

/// <summary>
/// Agent模式下返回文本块事件，即：在Agent模式下，文章的文本以分块的方式输出（仅Agent模式下使用）
/// </summary>
[Serializable]
public record AgentMessageEvent : ChunkCompletionResponseBody
{
    /// <summary>
    /// 任务 ID，用于请求跟踪和下方的停止响应接口
    /// </summary>
    public required string TaskId { get; init; }

    /// <summary>
    /// 消息唯一 ID
    /// </summary>
    public required string MessageId { get; init; }

    /// <summary>
    /// 会话 ID
    /// </summary>
    public required string ConversationId { get; init; }

    /// <summary>
    /// LLM 返回文本块内容
    /// </summary>
    public required string Answer { get; init; }

    /// <summary>
    /// 创建时间戳
    /// </summary>
    public int CreateAt { get; init; }
}

/*
    event: agent_thought Agent模式下有关Agent思考步骤的相关内容，涉及到工具调用（仅Agent模式下使用）
    id (string) agent_thought ID，每一轮Agent迭代都会有一个唯一的id
    task_id (string) 任务ID，用于请求跟踪下方的停止响应接口
    message_id (string) 消息唯一ID
    position (int) agent_thought在消息中的位置，如第一轮迭代position为1
    thought (string) agent的思考内容
    observation (string) 工具调用的返回结果
    tool (string) 使用的工具列表，以 ; 分割多个工具
    tool_input (string) 工具的输入，JSON格式的字符串(object)。如：{"dalle3": {"prompt": "a cute cat"}}
    created_at (int) 创建时间戳，如：1705395332
    message_files (array[string]) 当前 agent_thought 关联的文件ID
    file_id (string) 文件ID
    conversation_id (string) 会话ID
 */

/// <summary>
/// Agent模式下有关Agent思考步骤的相关内容，涉及到工具调用（仅Agent模式下使用）
/// </summary>
public record AgentThoughtEvent : ChunkCompletionResponseBody
{
    /// <summary>
    /// agent_thought ID，每一轮Agent迭代都会有一个唯一的id
    /// </summary>
    public required string Id { get; init; }

    /// <summary>
    /// 任务ID，用于请求跟踪下方的停止响应接口
    /// </summary>
    public required string TaskId { get; init; }

    /// <summary>
    /// 消息唯一ID
    /// </summary>
    public required string MessageId { get; init; }

    /// <summary>
    /// agent_thought在消息中的位置，如第一轮迭代position为1
    /// </summary>
    public int Position { get; init; }

    /// <summary>
    /// agent的思考内容
    /// </summary>
    public required string Thought { get; init; }

    /// <summary>
    /// 工具调用的返回结果
    /// </summary>
    public required string Observation { get; init; }

    /// <summary>
    /// 使用的工具列表，以 ; 分割多个工具
    /// </summary>
    public required string Tool { get; init; }

    /// <summary>
    /// 工具的输入，JSON格式的字符串(object)。如：
    /// <code>{"dalle3": {"prompt": "a cute cat"}}</code>
    /// </summary>
    public required string ToolInput { get; init; }

    /// <summary>
    /// 创建时间戳
    /// </summary>
    public int CreatedAt { get; init; }

    /// <summary>
    /// 当前 agent_thought 关联的文件ID
    /// </summary>
    public required string[] MessageFiles { get; init; }

    /// <summary>
    /// 文件ID
    /// </summary>
    public required string FileId { get; init; }

    /// <summary>
    /// 会话ID
    /// </summary>
    public required string ConversationId { get; init; }
}

/*
    event: message_file 文件事件，表示有新文件需要展示
    id (string) 文件唯一ID
    type (string) 文件类型，目前仅为image
    belongs_to (string) 文件归属，user或assistant，该接口返回仅为 assistant
    url (string) 文件访问地址
    conversation_id (string) 会话ID
 */
/// <summary>
/// 文件事件，表示有新文件需要展示
/// </summary>
[Serializable]
public record MessageFileEvent : ChunkCompletionResponseBody
{
    /// <summary>
    /// 文件唯一ID
    /// </summary>
    public required string Id { get; init; }

    /// <summary>
    /// 文件类型，目前仅为image
    /// </summary>
    public required string Type { get; init; }

    /// <summary>
    /// 文件归属，user或assistant，该接口返回仅为 assistant
    /// </summary>
    public required string BelongsTo { get; init; } // user or assistant

    /// <summary>
    /// 文件访问地址
    /// </summary>
    public required string Url { get; init; }

    /// <summary>
    /// 会话ID
    /// </summary>
    public required string ConversationId { get; init; }
}

/*
    event: message_end 消息结束事件，收到此事件则代表流式返回结束。
    task_id (string) 任务 ID，用于请求跟踪和下方的停止响应接口
    message_id (string) 消息唯一 ID
    conversation_id (string) 会话 ID
    metadata (object) 元数据
        usage (Usage) 模型用量信息
        retriever_resources (array[RetrieverResource]) 引用和归属分段列表
 */
/// <summary>
/// 消息结束事件，收到此事件则代表流式返回结束。
/// </summary>
[Serializable]
public record MessageEndEvent : ChunkCompletionResponseBody
{
    /// <summary>
    /// 任务 ID，用于请求跟踪和下方的停止响应接口
    /// </summary>
    public required string TaskId { get; init; }

    /// <summary>
    /// 消息唯一 ID
    /// </summary>
    public required string MessageId { get; init; }

    /// <summary>
    ///  会话 ID
    /// </summary>
    public required string ConversationId { get; init; }

    /// <summary>
    /// 
    /// </summary>
    public MetadataImpl? Metadata { get; init; }


    [Serializable]
    public record MetadataImpl
    {
        /// <summary>
        /// 模型用量信息
        /// </summary>
        public JsonElement Usage { get; init; }

        /// <summary>
        /// 引用和归属分段列表
        /// </summary>
        /// retriever_resources 
        public JsonElement RetrieverResources { get; init; }
    }
}

/// <summary>
/// 消息内容替换事件。 开启内容审查和审查输出内容时，若命中了审查条件，则会通过此事件替换消息内容为预设回复。
/// </summary>
[Serializable]
public record MessageReplaceEvent : ChunkCompletionResponseBody
{
    /// <summary>
    /// 任务 ID，用于请求跟踪和下方的停止响应接口
    /// </summary>
    public required string TaskId { get; init; }

    /// <summary>
    /// 消息唯一 ID
    /// </summary>
    public required string MessageId { get; init; }

    /// <summary>
    /// 会话 ID
    /// </summary>
    public required string ConversationId { get; init; }

    /// <summary>
    /// 替换内容（直接替换 LLM 所有回复文本）
    /// </summary>
    public required string Answer { get; init; }

    /// <summary>
    /// 创建时间戳
    /// </summary>
    public int CreateAt { get; init; }
}

/// <summary>
/// workflow 开始执行
/// </summary>
[Serializable]
public record WorkflowStartedEvent : ChunkCompletionResponseBody
{
    /// <summary>
    /// 任务 ID，用于请求跟踪和下方的停止响应接口
    /// </summary>
    public required string TaskId { get; init; }

    /// <summary>
    /// workflow 执行 ID
    /// </summary>
    public required string WorkflowRunId { get; init; }

    /// <summary>
    /// 详细内容
    /// </summary>
    public DataImpl? Data { get; init; }

    /// <summary>
    /// 
    /// </summary>
    [Serializable]
    public record DataImpl
    {
        /// <summary>
        /// workflow 执行 ID
        /// </summary>
        public required string Id { get; init; }

        /// <summary>
        /// 关联 Workflow ID
        /// </summary>
        public required string WorkflowId { get; init; }

        /// <summary>
        /// 自增序号，App 内自增，从 1 开始
        /// </summary>
        public int SequenceNumber { get; init; }

        /// <summary>
        /// 开始时间
        /// </summary>
        public long CreatedAt { get; init; }
    }
}

/// <summary>
/// node_started node 开始执行
/// </summary>
[Serializable]
public record NodeStartedEvent : ChunkCompletionResponseBody
{
    /// <summary>
    /// 任务 ID，用于请求跟踪和下方的停止响应接口
    /// </summary>
    public required string TaskId { get; init; }

    /// <summary>
    /// workflow 执行 ID
    /// </summary>
    public required string WorkflowRunId { get; init; }

    /// <summary>
    /// 详细内容
    /// </summary>
    public DataImpl? Data { get; init; }

    [Serializable]
    public record DataImpl
    {
        /// <summary>
        /// workflow 执行 ID
        /// </summary>
        public required string Id { get; init; }

        /// <summary>
        /// 节点 ID
        /// </summary>
        public required string NodeId { get; init; }

        /// <summary>
        /// 节点类型
        /// </summary>
        public required string NodeType { get; init; }

        /// <summary>
        /// 节点名称
        /// </summary>
        public required string Title { get; init; }

        /// <summary>
        /// 执行序号，用于展示 Tracing Node 顺序
        /// </summary>
        public int Index { get; init; }

        /// <summary>
        /// 前置节点 ID，用于画布展示执行路径
        /// </summary>
        public required string PredecessorNodeId { get; init; }

        /// <summary>
        /// 节点中所有使用到的前置节点变量内容
        /// </summary>
        public JsonElement Inputs { get; init; }

        /// <summary>
        /// 开始时间
        /// </summary>
        public long CreatedAt { get; init; }
    }
}

/// <summary>
/// node_finished node 执行结束，成功失败同一事件中不同状态
/// </summary>
[Serializable]
public record NodeFinishedEvent : ChunkCompletionResponseBody
{
    /// <summary>
    /// 任务 ID，用于请求跟踪和下方的停止响应接口
    /// </summary>
    public required string TaskId { get; init; }

    /// <summary>
    /// workflow 执行 ID
    /// </summary>
    public required string WorkflowRunId { get; init; }

    /// <summary>
    /// 详细内容
    /// </summary>
    public DataImpl? Data { get; init; }

    [Serializable]
    public record DataImpl
    {
        /// <summary>
        ///  node 执行 ID
        /// </summary>
        public required string Id { get; init; }

        /// <summary>
        /// 节点 ID
        /// </summary>
        public required string NodeId { get; init; }

        /// <summary>
        /// 执行序号，用于展示 Tracing Node 顺序
        /// </summary>
        public int Index { get; init; }

        /// <summary>
        /// optional 前置节点 ID，用于画布展示执行路径
        /// </summary>
        public required string PredecessorNodeId { get; init; }

        /// <summary>
        /// 节点中所有使用到的前置节点变量内容
        /// </summary>
        public JsonElement Inputs { get; init; }

        /// <summary>
        /// Optional 节点过程数据
        /// </summary>
        public JsonElement ProcessData { get; init; }

        /// <summary>
        /// Optional 输出内容
        /// </summary>
        public JsonElement Outputs { get; init; }

        /// <summary>
        /// 执行状态 running / succeeded / failed / stopped
        /// </summary>
        public required string Status { get; init; }

        /// <summary>
        /// Optional 错误原因
        /// </summary>
        public required string Error { get; init; }

        /// <summary>
        /// Optional 耗时(s)
        /// </summary>
        public float ElapsedTime { get; init; }

        /// <summary>
        /// 元数据
        /// </summary>
        public JsonElement ExecutionMetadata { get; init; }

        /// <summary>
        /// 开始时间
        /// </summary>
        public long CreatedAt { get; init; }
    }
}

/// <summary>
/// workflow_finished workflow 执行结束，成功失败同一事件中不同状态
/// </summary>
[Serializable]
public record WorkflowFinishedEvent : ChunkCompletionResponseBody
{
    /// <summary>
    /// 任务 ID，用于请求跟踪和下方的停止响应接口
    /// </summary>
    public required string TaskId { get; init; }

    /// <summary>
    /// workflow 执行 ID
    /// </summary>
    public required string WorkflowRunId { get; init; }

    public DataImpl? Data { get; init; }

    [Serializable]
    public record DataImpl
    {
        /// <summary>
        /// workflow 执行 ID
        /// </summary>
        public required string Id { get; init; }

        /// <summary>
        /// 关联 Workflow ID
        /// </summary>
        public required string WorkflowId { get; init; }

        /// <summary>
        /// 执行状态 running / succeeded / failed / stopped
        /// </summary>
        public required string Status { get; init; }

        /// <summary>
        /// Optional 输出内容
        /// </summary>
        public JsonElement? Outputs { get; init; }

        /// <summary>
        /// Optional 错误原因
        /// </summary>
        public required string Error { get; init; }

        /// <summary>
        /// Optional 耗时(s)
        /// </summary>
        public float ElapsedTime { get; init; }

        /// <summary>
        /// Optional 总使用 tokens
        /// </summary>
        public int TotalTokens { get; init; }

        /// <summary>
        /// 总步数（冗余），默认 0
        /// </summary>
        public int TotalSteps { get; init; }

        /// <summary>
        /// 开始时间
        /// </summary>
        public long CreatedAt { get; init; }

        /// <summary>
        /// 结束时间
        /// </summary>
        public long FinishedAt { get; init; }
    }
}

/*
    event: error 流式输出过程中出现的异常会以 stream event 形式输出，收到异常事件后即结束。
    task_id (string) 任务 ID，用于请求跟踪和下方的停止响应接口
    message_id (string) 消息唯一 ID
    status (int) HTTP 状态码
    code (string) 错误码
    message (string) 错误消息
 */
/// <summary>
/// error 流式输出过程中出现的异常会以 stream event 形式输出，收到异常事件后即结束。
/// </summary>
[Serializable]
public record ErrorEvent : ChunkCompletionResponseBody
{
    /// <summary>
    /// 任务 ID，用于请求跟踪和下方的停止响应接口
    /// </summary>
    public string TaskId { get; init; } = "";

    /// <summary>
    /// 消息唯一 ID
    /// </summary>
    public required string MessageId { get; init; }

    /// <summary>
    /// HTTP 状态码
    /// </summary>
    public int Status { get; init; }

    /// <summary>
    /// 错误码
    /// </summary>
    public required string Code { get; init; }

    /// <summary>
    /// 错误消息
    /// </summary>
    public required string Message { get; init; }
}

/// <summary>
/// 每 10s 一次的 ping 事件，保持连接存活。
/// </summary>
[Serializable]
public record PingEvent : ChunkCompletionResponseBody;

#endregion