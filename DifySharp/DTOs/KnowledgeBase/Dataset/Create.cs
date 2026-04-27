using System.Text.Json.Serialization;

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
        string             Name,
        string?            Description            = null,
        IndexingTechnique? IndexTechnique         = null,
        Permission         Permission             = Permission.AllTeamMembers,
        Provider?          Provider               = null,
        string?            ExternalKnowledgeApiId = null,
        string?            ExternalKnowledgeId    = null
    );


    /// <summary>
    /// 权限（选填，默认 only_me）
    /// </summary>
    [JsonConverter(typeof(JsonStringEnumConverter<Permission>))]
    public enum Permission
    {
        [JsonPropertyName("only_me")] OnlyMe = 0,
        [JsonPropertyName("all_team_members")] AllTeamMembers = 1,
        [JsonPropertyName("partial_members")] PartialMembers = 2
    }

    /// <summary>
    /// Provider（选填，默认 vendor）
    /// </summary>
    [JsonConverter(typeof(JsonStringEnumConverter<Provider>))]
    public enum Provider
    {
        [JsonPropertyName("vendor")] Vendor = 0,
        [JsonPropertyName("external")] External = 1
    }
}