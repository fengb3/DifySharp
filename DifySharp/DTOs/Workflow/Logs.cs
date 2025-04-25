namespace DifySharp.Workflow;

public class Logs
{
    public record ResponseBody(
        int               Page,
        int               Limit,
        int               Total,
        bool              HasMore,
        ICollection<Data> Data
    )
    {
        public int               Page    { get; init; } = Page;
        public int               Limit   { get; init; } = Limit;
        public int               Total   { get; init; } = Total;
        public bool              HasMore { get; init; } = HasMore;
        public ICollection<Data> Data    { get; init; } = Data;
    }

    public record Data(
        string           Id,
        string           Version,
        WorkflowRun      WorkflowRun,
        string           CreateFrom,
        string           CreatedByRole,
        string           CreatedByAccount,
        CreatedByEndUser CreatedByEndUser,
        string           CreatedAt
    )
    {
        public string           Id               { get; init; } = Id;
        public string           Version          { get; init; } = Version;
        public WorkflowRun      WorkflowRun      { get; init; } = WorkflowRun;
        public string           CreateFrom       { get; init; } = CreateFrom;
        public string           CreatedByRole    { get; init; } = CreatedByRole;
        public string           CreatedByAccount { get; init; } = CreatedByAccount;
        public CreatedByEndUser CreatedByEndUser { get; init; } = CreatedByEndUser;
        public string           CreatedAt        { get; init; } = CreatedAt;

        public void Deconstruct(out string id, out string version, out WorkflowRun workflowRun, out string createFrom, out string createdByRole, out string                         createdByAccount, out CreatedByEndUser               createdByEndUser, out string                         createdAt)
        {
            id               = this.Id;
            version          = this.Version;
            workflowRun      = this.WorkflowRun;
            createFrom       = this.CreateFrom;
            createdByRole    = this.CreatedByRole;
            createdByAccount = this.CreatedByAccount;
            createdByEndUser = this.CreatedByEndUser;
            createdAt        = this.CreatedAt;
        }
    }

    public record WorkflowRun(
        string Id,
        string Version,
        string Status,
        string Error,
        float  ElapsedTime,
        int    TotalTokens
    )
    {
        public string Id          { get; init; } = Id;
        public string Version     { get; init; } = Version;
        public string Status      { get; init; } = Status;
        public string Error       { get; init; } = Error;
        public float  ElapsedTime { get; init; } = ElapsedTime;
        public int    TotalTokens { get; init; } = TotalTokens;

        public void Deconstruct(out string id, out string version, out string status, out string error, out float  elapsedTime, out int                            totalTokens)
        {
            id          = this.Id;
            version     = this.Version;
            status      = this.Status;
            error       = this.Error;
            elapsedTime = this.ElapsedTime;
            totalTokens = this.TotalTokens;
        }
    }

    public class CreatedByEndUser(
    );
}