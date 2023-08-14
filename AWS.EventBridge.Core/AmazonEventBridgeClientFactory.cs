using Amazon;
using Amazon.EventBridge;
using Microsoft.Extensions.Options;

namespace AWS.EventBridge.Core
{
    public class AmazonEventBridgeClientFactory : IAmazonEventBridgeClientFactory
    {
        private readonly string awsRegion;
        private readonly string accountId;
        private readonly string iamAccessKey;
        private readonly string iamSecretKey;

        public AmazonEventBridgeClientFactory(IOptions<EventBridgeConfigOptions> options)
        {
            var eventBridgeConfigOptions = options.Value;
            this.awsRegion = eventBridgeConfigOptions.AWSRegion;
            this.accountId = eventBridgeConfigOptions.AWSAccountId;
            this.iamAccessKey = eventBridgeConfigOptions.IamAccessKey;
            this.iamSecretKey = eventBridgeConfigOptions.IamSecretKey;
        }

        public IAmazonEventBridge GetEventBridgeClient()       
        {
            var config = new AmazonEventBridgeConfig
            {
                RegionEndpoint = RegionEndpoint.GetBySystemName(this.awsRegion),
            };

            if (Environment.GetEnvironmentVariable("environment") == "dev")
                return new AmazonEventBridgeClient(iamAccessKey, iamSecretKey, config); //for local debug

            return new AmazonEventBridgeClient(config); // for publish on serve
        }
    }
}
