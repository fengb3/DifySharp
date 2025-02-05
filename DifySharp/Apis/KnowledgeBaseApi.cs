using DifySharp.Attributes;
using DifySharp.KnowledgeBase.Chunk;
using DifySharp.KnowledgeBase.Dataset;
using DifySharp.KnowledgeBase.Document;
using WebApiClientCore.Attributes;
using WebApiClientCore;

namespace DifySharp.Apis
{
    [LoggingFilter]
    // [OAuthToken]
    [DifyAuth]
    public interface IKnowledgeBaseApi : IDatasetApi, IDocumentApi, IChunkApi;
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
        public Task<Create.ResponseBody> PostCreateSegmentAsync(
            string                           datasetId,
            string                           documentId,
            [JsonContent] Create.RequestBody body
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
        public Task<Get.ResponseBody> GetSegments(
            string             datasetId,
            string             documentId,
            [PathQuery] string keyword,
            [PathQuery] string status
        );

        /// <summary>
        /// Deletes a specific segment from a dataset document.
        /// </summary>
        /// <param name="datasetId">The unique identifier for the dataset.</param>
        /// <param name="documentId">The unique identifier for the document within the dataset.</param>
        /// <param name="segmentId">The unique identifier for the segment to be deleted.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the response body of the delete operation.</returns>
        [HttpDelete("/v1/datasets/{datasetId}/documents/{documentId}/segments/{segmentId}")]
        public Task<Delete.ResponseBody> DeleteSegments(
            string datasetId,
            string documentId,
            string segmentId
        );

        /// <summary>
        /// Updates an existing segment with the provided data.
        /// </summary>
        /// <param name="datasetId">The identifier of the dataset to which the segment belongs.</param>
        /// <param name="documentId">The identifier of the document within the dataset containing the segment.</param>
        /// <param name="segmentId">The identifier of the segment to be updated.</param>
        /// <param name="body">The object containing the updated information for the segment.</param>
        /// <returns>A task representing the asynchronous operation. The result contains the HTTP response message.</returns>
        [HttpPost("/v1/datasets/{datasetId}/documents/{documentId}/segments/{segmentId}")]
        public Task<HttpResponseMessage> PostUpdateSegment(
            string               datasetId,
            string               documentId,
            string               segmentId,
            [JsonContent] object body
        );

        /// <summary>
        /// Retrieves the upload file for a specific dataset and document.
        /// </summary>
        /// <param name="datasetId">The identifier of the dataset.</param>
        /// <param name="documentId">The identifier of the document within the dataset.</param>
        /// <returns>A task representing the asynchronous operation, containing the HTTP response message.</returns>
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