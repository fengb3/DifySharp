using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace DifySharp.Test;

public static class Helper
{
    private static readonly JsonSerializerOptions Options = new JsonSerializerOptions
    {
        AllowTrailingCommas             = false,
        DefaultIgnoreCondition          = JsonIgnoreCondition.Never,
        Encoder                         = JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
        WriteIndented                   = true
    };

    public static string ToJson(this object obj) => JsonSerializer.Serialize(obj, Options);
}