using Amazon;
using Amazon.Batch;
using Microsoft.Extensions.Options;

namespace AWS.BatchJob.Core
{
    public class AmazonBatchJobClientFactory : IAmazonBatchJobClientFactory
    {
        private readonly string awsRegion;
        private readonly string accountId;
        private readonly string iamAccessKey;
        private readonly string iamSecretKey;

        public AmazonBatchJobClientFactory(IOptions<BatchJobConfigOptions> options)
        {
            var batchJobConfigOptions = options.Value;
            this.awsRegion = batchJobConfigOptions.AWSRegion;
            this.accountId = batchJobConfigOptions.AWSAccountId;
            this.iamAccessKey = batchJobConfigOptions.IamAccessKey;
            this.iamSecretKey = batchJobConfigOptions.IamSecretKey;
        }

        public IAmazonBatch GetBatchJobClient()
        {
            var config = new AmazonBatchConfig
            {
                RegionEndpoint = RegionEndpoint.GetBySystemName(this.awsRegion),
            };

            if (Environment.GetEnvironmentVariable("environment") == "dev")
                return new AmazonBatchClient(iamAccessKey, iamSecretKey, config); //for local debug

            return new AmazonBatchClient(config); // for publish on serve
        }
    }
}
