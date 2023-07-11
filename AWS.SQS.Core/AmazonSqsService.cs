using Amazon.SQS.Model;
using System.Net;
using System.Text.Json;

namespace AWS.SQS.Core
{
    public class AmazonSqsService : IAmazonSqsService
    {
        private readonly IAmazonSqsClientFactory sqsClientFactory;
        public AmazonSqsService(IAmazonSqsClientFactory sqsClientFactory)
        {
            this.sqsClientFactory = sqsClientFactory;
        }

        public async Task<(bool isSuccess, HttpStatusCode httpStatusCode)> SendMessageAsync<T>(T message, string queueName)
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
        }

        public async Task<(bool isSuccess, HttpStatusCode httpStatusCode)> SendMessageBatchAsync(List<string> messages, string queueName)
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
        }

        public async Task<(bool isSuccess, HttpStatusCode httpStatusCode)> DeleteMessageBatchAsync(List<string> messages, string queueName)
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
        }
    }

}
