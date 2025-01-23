namespace DifySharp.KnowledgeBase.Dataset;

public record Create
{
    ///<summary>
    /// 
    /// </summary>
    /// <param name="Name">Knowledge name</param>
    /// <param name="Description">Knowledge description (optional)</param>
    /// <param name="IndexTechnique">Index technique (optional)</param>
    /// <param name="Permission">Permission</param>
    /// <param name="Provider">Provider (optional, default: vendor)</param>
    /// <param name="ExternalKnowledgeApiId">External knowledge API ID (optional)</param>
    /// <param name="ExternalKnowledgeId">External knowledge ID (optional)</param>
    /// <link>https://cloud.dify.ai/datasets?category=api#create_empty_dataset</link>
    public record RequestBody(
        string          Name,
        string?         Description            = null,
        IndexTechnique? IndexTechnique         = null,
        Permission      Permission             = Permission.AllTeamMembers,
        Provider?       Provider               = null,
        string?         ExternalKnowledgeApiId = null,
        string?         ExternalKnowledgeId    = null
    );


    /// <summary>
    /// 权限（选填，默认 only_me）
    /// </summary>
    public enum Permission
    {
        /// <summary>
        /// Only me
        /// </summary>
        OnlyMe = 0,

        /// <summary>
        /// all team members
        /// </summary>
        AllTeamMembers = 1,

        /// <summary>
        /// partial members
        /// </summary>
        PartialMembers = 2
    }

    /// <summary>
    /// Provider（选填，默认 vendor）
    /// </summary>
    public enum Provider
    {
        Vendor   = 0,
        External = 1
    }
};