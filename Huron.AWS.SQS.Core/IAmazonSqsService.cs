using System.Net;

namespace Huron.AWS.SQS.Core
{
    public interface IAmazonSqsService
    {
        Task<(bool isSuccess, HttpStatusCode httpStatusCode)> SendMessageAsync<T>(T message, string queueName);
        Task<(bool isSuccess, HttpStatusCode httpStatusCode)> SendMessageBatchAsync(List<string> messages, string queueName);
        Task<(bool isSuccess, HttpStatusCode httpStatusCode)> DeleteMessageBatchAsync(List<string> messages, string queueName);
    }
}