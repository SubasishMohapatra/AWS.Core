using Amazon.SQS;
using Amazon.SQS.Model;
using AWS.SharedLib;
using Polly;
using Serilog;
using System.Net;
using System.Text.Json;

namespace AWS.SQS.Core
{
    public class AmazonSqsService : IAmazonSqsService
    {
        private readonly IAmazonSqsClientFactory sqsClientFactory;
        private readonly ILogger logger;
        public AmazonSqsService(IAmazonSqsClientFactory sqsClientFactory, ILogger logger)
        {
            this.sqsClientFactory = sqsClientFactory;
            this.logger = logger;
        }

        public async Task<(bool isSuccess, HttpStatusCode httpStatusCode)> SendMessageAsync<T>(T message, string queueName)
        {
            var result = await PollyRetry.ExecuteAsync<(bool isSuccess, HttpStatusCode httpStatusCode)>(async () =>
            {
                SendMessageResponse response;
                var request = new SendMessageRequest
                {
                    MessageBody = JsonSerializer.Serialize(message),
                    QueueUrl = sqsClientFactory.GetSqsQueue(queueName),
                };
                using (var client = sqsClientFactory.GetSqsClient())
                {
                    response = await client.SendMessageAsync(request);
                }
                return response.HttpStatusCode == HttpStatusCode.OK ? (true, response.HttpStatusCode) :
                 (false, response.HttpStatusCode);
            },
            SqsFallbackAsync, SqsFallbackActionOnFallbackAsync, logger);
            return result;
        }

        public async Task<(bool isSuccess, HttpStatusCode httpStatusCode)> SendMessageBatchAsync(List<string> messages, string queueName)
        {
            var result = await PollyRetry.ExecuteAsync<(bool isSuccess, HttpStatusCode httpStatusCode)>(async () =>
            {
                SendMessageBatchResponse response;
                var request = new SendMessageBatchRequest
                {
                    QueueUrl = sqsClientFactory.GetSqsQueue(queueName),
                    Entries = messages.Select((message, index) => new SendMessageBatchRequestEntry
                    {
                        Id = Guid.NewGuid().ToString(),
                        MessageBody = message
                    }).ToList()
                };

                using (var client = sqsClientFactory.GetSqsClient())
                {
                    response = await client.SendMessageBatchAsync(request);
                }

                return response.HttpStatusCode == HttpStatusCode.OK ? (true, response.HttpStatusCode) :
                 (false, response.HttpStatusCode);
            },
            SqsFallbackAsync, SqsFallbackActionOnFallbackAsync, logger);
            return result;
        }

        public async Task<(bool isSuccess, HttpStatusCode httpStatusCode)> DeleteMessageBatchAsync(List<string> messages, string queueName)
        {
            var result = await PollyRetry.ExecuteAsync<(bool isSuccess, HttpStatusCode httpStatusCode)>(async () =>
            {
                DeleteMessageBatchResponse response;
                var request = new DeleteMessageBatchRequest
                {
                    QueueUrl = sqsClientFactory.GetSqsQueue(queueName),
                    Entries = messages.Select((message, index) => new DeleteMessageBatchRequestEntry
                    {
                        Id = Guid.NewGuid().ToString(),
                        ReceiptHandle = message
                    }).ToList()
                };

                using (var client = sqsClientFactory.GetSqsClient())
                {
                    response = await client.DeleteMessageBatchAsync(request);
                }

                return response.HttpStatusCode == HttpStatusCode.OK ? (true, response.HttpStatusCode) :
                 (false, response.HttpStatusCode);
            },
            SqsFallbackAsync, SqsFallbackActionOnFallbackAsync, logger);
            return result;
        }

        private Task SqsFallbackActionOnFallbackAsync(DelegateResult<(bool isSuccess, HttpStatusCode httpStatusCode)> response, Context context)
        {
            //Console.WriteLine("About to call the fallback action. This is a good place to do some logging");
            return Task.CompletedTask;
        }

        private Task<(bool isSuccess, HttpStatusCode httpStatusCode)> SqsFallbackAsync(DelegateResult<(bool isSuccess, HttpStatusCode httpStatusCode)> responseToFailedRequest, Context context, CancellationToken cancellationToken1)
        {
            var ex = responseToFailedRequest.Exception;
            logger.Error("SqsFallbackAsync error: {Error}", ex.Message);
            if (ex is not AmazonSQSException sqsException)
            {
                return Task.FromResult((isSuccess: false, httpStatusCode: HttpStatusCode.InternalServerError));
            }
            return Task.FromResult((isSuccess: false, httpStatusCode: (HttpStatusCode)sqsException.StatusCode));
        }
    }

}
