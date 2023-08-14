using Amazon.S3;
using Amazon.S3.Model;
using AWS.SharedLib;
using Newtonsoft.Json;
using Polly;
using Serilog;
using System.Net;

namespace AWS.S3.Core
{
    public class AmazonS3Service : IAmazonS3Service
    {
        private readonly IAmazonS3ClientFactory amazonS3ClientFactory;
        private readonly ILogger logger;

        public AmazonS3Service(IAmazonS3ClientFactory amazonS3ClientFactory, ILogger logger)
        {
            this.amazonS3ClientFactory = amazonS3ClientFactory;
            this.logger = logger;
        }

        public async Task<(bool, HttpStatusCode)> UploadFileAsync(string key, object content)
        {
            var result = await PollyRetry.ExecuteAsync<(bool isSuccess, HttpStatusCode httpStatusCode)>(async () =>
            {
                PutObjectResponse result;
                var amazonS3client = this.amazonS3ClientFactory.GetAmazonS3Client();
                string json = JsonConvert.SerializeObject(content);
                byte[] bytes = System.Text.Encoding.UTF8.GetBytes(json);
                using (MemoryStream stream = new MemoryStream(bytes))
                {
                    PutObjectRequest request = new PutObjectRequest
                    {
                        BucketName = this.amazonS3ClientFactory.GetS3BucketName(),
                        Key = key,
                        InputStream = stream
                    };
                    result = await amazonS3client.PutObjectAsync(request);
                }
                return result.HttpStatusCode == HttpStatusCode.OK ? (true, result.HttpStatusCode) :
                    (false, result.HttpStatusCode);
            },
             S3FallbackAsync, S3FallbackActionOnFallbackAsync, this.logger);
            return result;
        }

        public async Task<(bool, HttpStatusCode)> UploadFileAsync(string key, Stream stream)
        {
            var result = await PollyRetry.ExecuteAsync<(bool isSuccess, HttpStatusCode httpStatusCode)>(async () =>
            {
                PutObjectResponse result;
                var amazonS3client = this.amazonS3ClientFactory.GetAmazonS3Client();
                PutObjectRequest request = new PutObjectRequest
                {
                    BucketName = this.amazonS3ClientFactory.GetS3BucketName(),
                    Key = key,
                    InputStream = stream
                };
                result = await amazonS3client.PutObjectAsync(request);
                return result.HttpStatusCode == HttpStatusCode.OK ? (true, result.HttpStatusCode) :
                    (false, result.HttpStatusCode);
            },
             S3FallbackAsync, S3FallbackActionOnFallbackAsync, this.logger);
            return result;
        }

        public async Task<(bool, HttpStatusCode)> DeleteNonVersionedObjectAsync(string keyName)
        {
            var result = await PollyRetry.ExecuteAsync<(bool isSuccess, HttpStatusCode httpStatusCode)>(async () =>
            {
                DeleteObjectResponse result;
                var amazonS3client = this.amazonS3ClientFactory.GetAmazonS3Client();
                var deleteObjectRequest = new DeleteObjectRequest
                {
                    BucketName = this.amazonS3ClientFactory.GetS3BucketName(),
                    Key = keyName
                };
                result = await amazonS3client.DeleteObjectAsync(deleteObjectRequest);
                return result.HttpStatusCode == HttpStatusCode.OK ? (true, result.HttpStatusCode) :
                    (false, result.HttpStatusCode);
            },
             S3FallbackAsync, S3FallbackActionOnFallbackAsync, this.logger);
            return result;
        }

        public async Task<(bool isSuccess, HttpStatusCode httpStatusCode)> DeleteNonVersionedObjectsAsync(List<string> keyList)
        {
            var result = await PollyRetry.ExecuteAsync<(bool isSuccess, HttpStatusCode httpStatusCode)>(async () =>
            {
                var keys = keyList.Select(item => new KeyVersion { Key = item }).ToList();
                var multiObjectDeleteRequest = new DeleteObjectsRequest()
                {
                    BucketName = amazonS3ClientFactory.GetS3BucketName(),
                    Objects = keys
                };

                var amazonS3client = amazonS3ClientFactory.GetAmazonS3Client();
                var result = await amazonS3client.DeleteObjectsAsync(multiObjectDeleteRequest);

                return result.HttpStatusCode == HttpStatusCode.OK ? (true, result.HttpStatusCode) :
                    (false, result.HttpStatusCode);
            },
            S3FallbackAsync, S3FallbackActionOnFallbackAsync, this.logger);
            return result;
        }

        public async Task<(string fileData, HttpStatusCode httpStatusCode)> GetS3FileContentAsync(string filePath)
        {
            var result = await PollyRetry.ExecuteAsync<(string fileData, HttpStatusCode httpStatusCode)>(async () =>
            {
                var amazonS3client = this.amazonS3ClientFactory.GetAmazonS3Client();
                var getObjectRequest = new GetObjectRequest
                {
                    BucketName = amazonS3ClientFactory.GetS3BucketName(),
                    Key = filePath
                };
                using (var response = await amazonS3client.GetObjectAsync(getObjectRequest))
                {
                    using (var responseStream = response.ResponseStream)
                    {
                        using (var reader = new StreamReader(responseStream))
                        {
                            var fileContent = await reader.ReadToEndAsync();
                            return (fileContent, response.HttpStatusCode);
                        }
                    }
                }
            },
            GetS3FileContentFallbackAsync, GetS3FileContentFallbackActionOnFallbackAsync, this.logger);
            return result;
        }

        private Task GetS3FileContentFallbackActionOnFallbackAsync(DelegateResult<(string fileContent, HttpStatusCode httpStatusCode)> response, Context context)
        {
            //Console.WriteLine("About to call the fallback action. This is a good place to do some logging");
            return Task.CompletedTask;
        }

        private Task<(string fileContent, HttpStatusCode httpStatusCode)> GetS3FileContentFallbackAsync(DelegateResult<(string fileContent, HttpStatusCode httpStatusCode)> responseToFailedRequest, Context context, CancellationToken cancellationToken)
        {
            var ex = responseToFailedRequest.Exception;
            logger.Error("GetS3FileContentFallbackAsync error: {Error}", ex.Message);
            if (ex is not AmazonS3Exception s3Exception)
            {
                return Task.FromResult((fileContent: ex.Message, httpStatusCode: HttpStatusCode.InternalServerError));
            }
            return Task.FromResult((fileContent: ex.Message, httpStatusCode: (HttpStatusCode)s3Exception.StatusCode));
        }

        private Task S3FallbackActionOnFallbackAsync(DelegateResult<(bool isSuccess, HttpStatusCode httpStatusCode)> response, Context context)
        {
            //Console.WriteLine("About to call the fallback action. This is a good place to do some logging");
            return Task.CompletedTask;
        }

        private Task<(bool isSuccess, HttpStatusCode httpStatusCode)> S3FallbackAsync(DelegateResult<(bool isSuccess, HttpStatusCode httpStatusCode)> responseToFailedRequest, Context context, CancellationToken cancellationToken)
        {
            var ex = responseToFailedRequest.Exception;
            logger.Error("S3FallbackAsync error: {Error}", ex.Message);
            if (ex is not AmazonS3Exception s3Exception)
            {
                return Task.FromResult((isSuccess: false, httpStatusCode: HttpStatusCode.InternalServerError));
            }
            return Task.FromResult((isSuccess: false, httpStatusCode: s3Exception.StatusCode));
        }
    }

}
