using Amazon;
using Amazon.SQS;
using Microsoft.Extensions.Options;

namespace Huron.AWS.SQS.Core
{
    public class AmazonSqsClientFactory : IAmazonSqsClientFactory
    {
        private readonly string sqsRegion;
        private readonly string sqsAccountId;
        private readonly string iamAccessKey;
        private readonly string iamSecretKey;

        public AmazonSqsClientFactory(IOptions<SqsConfigOptions> options)
        {
            var sqsConfigOptions = options.Value;
            sqsRegion = sqsConfigOptions.SqsRegion;
            sqsAccountId = sqsConfigOptions.AccountId;

            //TODO: The accessKey and secretKey will be ideally fetched using the SecretsManager
            iamAccessKey = sqsConfigOptions.IamAccessKey;
            iamSecretKey = sqsConfigOptions.IamSecretKey;
        }

        public IAmazonSQS GetSqsClient()
        {
            var config = new AmazonSQSConfig
            {
                RegionEndpoint = RegionEndpoint.GetBySystemName(sqsRegion),
            };

            if (Environment.GetEnvironmentVariable("environment") == "dev")
                return new AmazonSQSClient(iamAccessKey, iamSecretKey, config); //for local debug

            return new AmazonSQSClient(config); // for publish on serve
        }

        public string GetSqsQueue(string queueName) =>
            $"https://sqs.{sqsRegion}.amazonaws.com/{sqsAccountId}/{queueName}";
    }

}
