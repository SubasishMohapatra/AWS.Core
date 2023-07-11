using Amazon.S3.Model;
using Newtonsoft.Json;
using System.Net;

namespace AWS.S3.Core
{
    public interface IAmazonS3Service
    {
        Task<(bool, HttpStatusCode)> UploadFileAsync(string key, object content);
        Task<(bool, HttpStatusCode)> UploadFileAsync(string key, Stream stream);

        Task<(bool, HttpStatusCode)> DeleteNonVersionedObjectAsync(string keyName);
        Task<(string fileData, HttpStatusCode httpStatusCode)> GetS3FileContentAsync(string filePath);
        Task<(bool isSuccess, HttpStatusCode httpStatusCode)> DeleteNonVersionedObjectsAsync(List<string> keyList);
    }
}