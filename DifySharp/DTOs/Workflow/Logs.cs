namespace DifySharp.Workflow.Run;

public class Logs
{
    public record ResponseBody(
        int               Page,
        int               Limit,
        int               Total,
        bool              HasMore,
        ICollection<Data> Data
    );

    public record Data(
        string           Id,
        string           Version,
        WorkflowRun      WorkflowRun,
        string           CreateFrom,
        string           CreatedByRole,
        string           CreatedByAccount,
        CreatedByEndUser CreatedByEndUser,
        string           CreatedAt,
 
    );

    public record WorkflowRun(
        string Id,
        string Version,
        String Status,
        String Error,
        float            ElapsedTime,
        int  TotalTokens,
        
    );

    public record CreatedByEndUser(
    );
}