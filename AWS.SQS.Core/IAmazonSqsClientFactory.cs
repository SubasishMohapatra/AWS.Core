using Amazon.Runtime;
using Amazon.SQS;

namespace AWS.SQS.Core
{
    public interface IAmazonSqsClientFactory
    {
        IAmazonSQS GetSqsClient();
        string GetSqsQueue(string queueName);
    }
}