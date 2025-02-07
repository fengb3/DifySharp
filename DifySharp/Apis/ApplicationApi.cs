using WebApiClientCore.Attributes;
using WebApiClientCore.Parameters;

namespace DifySharp.Apis;

public interface IApplicationApi
{
    [HttpPost("files/upload")]
    public Task<HttpResponseMessage> PostFilesUpload(
        [FormDataContent] string user,
        FileInfo                 file
    );

    [HttpPost("/messages/{messageId}/feedbacks")]
    public Task<HttpResponseMessage> PostMessagesFeedbacks(
        string               messageId,
        [JsonContent] object requestBody
    );

    [HttpPost("/text-to-audio")]
    public Task<HttpResponseMessage> PostTextToAudio(
        [JsonContent] object requestBody
    );

    [HttpGet("/info")]
    public Task<HttpResponseMessage> GetInfo();

    [HttpGet("/parameters")]
    public Task<HttpResponseMessage> GetParameters();

    [HttpGet("/meta")]
    public Task<HttpResponseMessage> GetMeta();
}