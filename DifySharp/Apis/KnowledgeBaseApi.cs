using DifySharp.KnowledgeBase.Chunk;
using DifySharp.KnowledgeBase.Dataset;
using DifySharp.KnowledgeBase.Document;
using WebApiClientCore.Attributes;
using WebApiClientCore;

namespace DifySharp.Apis
{
    [LoggingFilter]
    [OAuthToken]
    public interface IKnowledgeBaseApi : IDatasetApi, IDocumentApi, IChunkApi;
}

namespace DifySharp.KnowledgeBase.Document
{
    public interface IDocumentApi
    {
        /// <summary>
        /// <para># 通过文本创建文档</para>
        /// <para>此接口基于已存在知识库，在此知识库的基础上通过文本创建新的文档</para>
        /// </summary>
        /// <param name="datasetId"></param>
        /// <param name="body"></param>
        /// <returns></returns>
        [HttpPost("/v1/datasets/{datasetId}/document/create_by_text")]
        public Task<CreateByText.ResponseBody> PostCreateDocumentByTextAsync(
            string                                 datasetId,
            [JsonContent] CreateByText.RequestBody body
        );

        /// <summary>
        /// <para># 通过文件创建文档</para>
        /// <para>此接口基于已存在知识库，在此知识库的基础上通过文件创建新的文档</para>
        /// </summary>
        /// <param name="datasetId"></param>
        /// <param name="body"></param>
        /// <param name="file"></param>
        /// <returns></returns>
        [HttpPost("/v1/datasets/{datasetId}/document/create_by_file")]
        public Task<CreateByFile.ResponseBody> PostCreateDocumentByFileAsync(
            string                                 datasetId,
            [FormContent] CreateByFile.RequestBody body,
            FileInfo                               file
        );

        /// <summary>
        /// <para># 通过文本更新文档</para>
        /// <para>此接口基于已存在知识库，在此知识库的基础上通过文本更新文档</para>
        /// </summary>
        /// <param name="datasetId"></param>
        /// <param name="documentId"></param>
        /// <param name="body"></param>
        /// <returns></returns>
        [HttpPost("/v1/datasets/{datasetId}/documents/{documentId}/update_by_text")]
        public Task<UpdateByText.ResponseBody> PostUpdateDocumentByTextAsync(
            string                                 datasetId,
            string                                 documentId,
            [JsonContent] UpdateByText.RequestBody body
        );

        /// <summary>
        /// <para># 通过文件更新文档</para>>
        /// </summary>
        /// <param name="datasetId"></param>
        /// <param name="documentId"></param>
        /// <param name="body"></param>
        /// <returns></returns>
        [HttpPost("/v1/datasets/{datasetId}/documents/{documentId}/update_by_file")]
        public Task<UpdateByFile.ResponseBody> PostUpdateDocumentByFileAsync(
            string                                 datasetId,
            string                                 documentId,
            [FormContent] UpdateByFile.RequestBody body
        );

        /// <summary>
        /// <para># 获取文档嵌入状态（进度）</para>
        /// </summary>
        /// <param name="datasetId"></param>
        /// <param name="batch"></param>
        /// <returns></returns>
        [HttpGet("/v1/datasets/{datasetId}/documents/{batch}/indexing-status")]
        public Task<Get.ResponseBody> GetIndexingStatus(
            string datasetId,
            int    batch
        );

        /// <summary>
        /// <para># 删除文档</para>
        /// </summary>
        /// <param name="datasetId"></param>
        /// <param name="documentId"></param>
        /// <returns></returns>
        [HttpDelete("/v1/datasets/{datasetId}/documents/{documentId}")]
        public Task<Delete.ResponseBody> DeleteDocument(
            string datasetId,
            string documentId
        );


        /// <summary>
        /// <para># 知识库文档列表</para>
        /// </summary>
        /// <param name="datasetId"></param>
        /// <param name="page"></param>
        /// <param name="limit"></param>
        /// <returns></returns>
        [HttpGet("/v1/datasets/{datasetId}/documents")]
        public Task<Get.ResponseBody> GetDocuments(
            string datasetId,
            int    page  = 1,
            int    limit = 20
        );
    }
}

namespace DifySharp.KnowledgeBase.Dataset
{
    public interface IDatasetApi
    {
        /// <summary>
        /// <para># 创建知识库</para>
        /// </summary>
        /// <param name="body"></param>
        /// <returns></returns>
        [HttpPost("/v1/datasets")]
        public Task<Dataset> PostCreateDatasetAsync(
            [JsonContent] Create.RequestBody body
        );

        /// <summary>
        /// <para># 获取知识库列表</para>
        /// </summary>
        /// <param name="page"></param>
        /// <param name="limit"></param>
        /// <returns></returns>
        [HttpGet("/v1/datasets")]
        public Task<Get.ResponseBody> GetDatasets(
            int page  = 1,
            int limit = 20
        );

        /// <summary>
        /// <para># 删除知识库</para>
        /// </summary>
        /// <param name="datasetId"></param>
        /// <returns></returns>
        [HttpDelete("/v1/datasets/{datasetId}")]
        public Task DeleteDataset(
            string datasetId
        );
    }
}

namespace DifySharp.KnowledgeBase.Chunk
{
    public interface IChunkApi
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="datasetId"></param>
        /// <param name="documentId"></param>
        /// <param name="body"></param>
        /// <returns></returns>
        [HttpPost("/v1/datasets/{datasetId}/documents/{documentId}/segments")]
        public Task<HttpResponseMessage> PostCreateSegmentAsync(
            string               datasetId,
            string               documentId,
            [JsonContent] object body
        );

        /// <summary>
        /// 
        /// </summary>
        /// <param name="datasetId"></param>
        /// <param name="documentId"></param>
        /// <param name="keyword"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        [HttpGet("/v1/datasets/{datasetId}/documents/{documentId}/segments")]
        public Task<HttpResponseMessage> GetSegments(
            string             datasetId,
            string             documentId,
            [PathQuery] string keyword,
            [PathQuery] string status
        );

        /// <summary>
        /// 
        /// </summary>
        /// <param name="datasetId"></param>
        /// <param name="documentId"></param>
        /// <param name="segmentId"></param>
        /// <returns></returns>
        [HttpDelete("/v1/datasets/{datasetId}/documents/{documentId}/segments/{segmentId}")]
        public Task<HttpResponseMessage> DeleteSegments(
            string datasetId,
            string documentId,
            string segmentId
        );

        /// <summary>
        /// 
        /// </summary>
        /// <param name="datasetId"></param>
        /// <param name="documentId"></param>
        /// <param name="segmentId"></param>
        /// <param name="body"></param>
        /// <returns></returns>
        [HttpPost("/v1/datasets/{datasetId}/documents/{documentId}/segments/{segmentId}")]
        public Task<HttpResponseMessage> PostUpdateSegment(
            string               datasetId,
            string               documentId,
            string               segmentId,
            [JsonContent] object body
        );

        /// <summary>
        /// 
        /// </summary>
        /// <param name="datasetId"></param>
        /// <param name="documentId"></param>
        /// <returns></returns>
        /// <link>https://cloud.dify.ai/datasets?category=api#get_upload_file</link>
        [HttpGet("/v1/datasets/{datasetId}/documents/{documentId}/upload-file")]
        public Task<HttpResponseMessage> GetUpLoadFile(
            string datasetId,
            string documentId
        );

        /// <summary>
        /// Retrieve Chunks from a Knowledge Base
        /// </summary>
        /// <param name="datasetId">Knowledge ID</param>
        /// <returns></returns>
        /// <link>https://cloud.dify.ai/datasets?category=api#dataset_retrieval</link>
        [HttpPost("/datasets/{datasetId}/retrieve")]
        public Task<HttpResponseMessage> PostRetrieve(
            string datasetId
        );
    }
}



