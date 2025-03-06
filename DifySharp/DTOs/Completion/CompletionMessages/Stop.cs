namespace DifySharp.Completion.CompletionMessages;

public record Stop
{
    /// <param name="User">
    /// Required User identifier,
    /// used to define the identity of the end-user,
    /// must be consistent with the user passed in the send message interface
    /// </param>
    public record RequestBody(string User);


    /// <param name="Result">
    /// Always returns "success"
    /// </param>
    public record ResponseBody(string Result);
}