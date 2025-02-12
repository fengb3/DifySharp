using System.Text.Json;

namespace DifySharp.Completion.CompletionMessages;

public class CompletionMessages
{
    public record RequestBody
    {
        /// <summary>
        /// 用户输入/提问内容。
        /// </summary>
        public required string Query { get; init; }

        /// <summary>
        /// 允许传入 App 定义的各变量值。默认值为 {}。
        /// inputs 参数包含了多组键值对（Key/Value pairs），
        /// 每组的键对应一个特定变量，每组的值则是该变量的具体值。
        /// </summary>
        public object Inputs { get; init; } = new();

        /// <summary>
        /// <para>
        /// `streaming` 流式模式（推荐）。基于 SSE（Server-Sent Events）实现类似打字机输出方式的流式返回。
        /// </para>
        /// <para>
        /// `blocking` 阻塞模式，等待执行完毕后返回结果。（请求若流程较长可能会被中断）。
        ///		由于 Cloudflare 限制，请求会在 100 秒超时无返回后中断。
        ///		注：Agent模式下不允许blocking。
        /// </para>
        /// </summary>
        public ApplicationResponseMode ApplicationResponseMode { get; internal set; } =
            ApplicationResponseMode.Blocking;

        /// <summary>
        /// 用户标识，用于定义终端用户的身份，方便检索、统计。 \
        /// 由开发者定义规则，需保证用户标识在应用内唯一。
        /// </summary>
        public required string User { get; init; }

        /// <summary>
        /// （选填）会话 ID，
        /// 若需要基于之前的聊天记录继续对话，则必须传之前消息的 conversation_id。
        /// </summary>
        public string ConversationId { get; init; } = "";

        /// <summary>
        /// 上传的文件。
        /// </summary>
        public ICollection<File>? Files { get; init; }
    }

    public record File
    {
        /// <summary>
        /// 支持类型：图片 image（目前仅支持图片格式）
        /// </summary>
        public string? Type { get; init; }

        /// <summary>
        /// 传递方式
        /// <para>
        /// Possible values:
        /// <list type="bullet">
        /// <item> remote_url </item>
        /// <item> local_file </item>
        /// </list>
        /// </para>
        /// </summary>
        public string? TransferMethods { get; init; }

        /// <summary>
        /// 图片地址。
        /// </summary>
        /// <仅当>
        /// TransferMethods 为 remote_url 时
        /// </仅当>
        public string? Url { get; init; }

        /// <summary>
        /// 上传文件 ID。
        /// </summary>
        /// <仅当>
        /// TransferMethods 为 local_file 时
        /// </仅当>
        public string? UploadFileId { get; init; }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="MessageId">消息唯一 ID</param>
    /// <param name="Mode">App 模式，固定为 chat</param>
    /// <param name="Answer">完整回复内容</param>
    /// <param name="Metadata">元数据</param>
    /// <param name="CreateAt">消息创建时间戳，如：1705395332</param>
    public record ResponseBody(
        string   MessageId,
        string   Mode,
        string   Answer,
        Metadata Metadata,
        int      CreateAt
    );

    /// <summary>
    /// 
    /// </summary>
    /// <param name="Usage">模型用量信息</param>
    /// <param name="RetrieverResources">引用和归属分段列表</param>
    public record Metadata(
        JsonElement? Usage,
        JsonElement  RetrieverResources
    );
}