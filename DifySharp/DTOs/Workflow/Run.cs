using System.Text.Json;

namespace DifySharp.Workflow.Run;

public class Run
{
    public record RequestBody
    {
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
        public ApplicationResponseMode ResponseMode { get; internal set; } =
            ApplicationResponseMode.Blocking;

        /// <summary>
        /// 用户标识，用于定义终端用户的身份，方便检索、统计。 \
        /// 由开发者定义规则，需保证用户标识在应用内唯一。
        /// </summary>
        public required string User { get; init; }

        /// <summary>
        /// 上传的文件。
        /// </summary>
        public ICollection<File>? Files { get; init; }
    }

    public record File
    {
        /// <summary>
        /// 支持类型:
        /// <list type="bullet">
        /// <item>document 具体类型包含：'TXT', 'MD', 'MARKDOWN', 'PDF', 'HTML', 'XLSX', 'XLS', 'DOCX', 'CSV', 'EML', 'MSG', 'PPTX', 'PPT', 'XML', 'EPUB'</item>
        /// <item>image  具体类型包含：'JPG', 'JPEG', 'PNG', 'GIF', 'WEBP', 'SVG'</item>
        /// <item>audio  具体类型包含：'MP3', 'M4A', 'WAV', 'WEBM', 'AMR'</item>
        /// <item>video  具体类型包含：'MP4', 'MOV', 'MPEG', 'MPGA'</item>
        /// <item>custom 具体类型包含：其他文件类型</item>
        /// </list>
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

    public record ResponseBody(
        string? WorkflowRunId,
        string  TaskId,
        Data    Data
    );

    /// <param name="Id"> workflow 执行 ID</param>
    /// <param name="WorkflowId">关联 Workflow ID</param>
    /// <param name="Status">执行状态, running / succeeded / failed / stopped</param>
    /// <param name="Outputs">Optional 输出内容</param>
    /// <param name="Error">Optional 错误原因</param>
    /// <param name="ElapsedTime">Optional 耗时(s)</param>
    /// <param name="TotalTokens">Optional 总使用 tokens</param>
    /// <param name="TotalSteps">总步数（冗余），默认 0</param>
    /// <param name="CreatedAt">开始时间</param>
    /// <param name="FinishedAt">结束时间</param>
    public record Data(
        string       Id,
        string       WorkflowId,
        string?      Status,
        JsonElement? Outputs,
        string?      Error,
        float?       ElapsedTime,
        int?         TotalTokens,
        int          TotalSteps,
        long         CreatedAt,
        long         FinishedAt
    );
}