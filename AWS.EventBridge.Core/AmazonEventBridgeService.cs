namespace AWS.EventBridge.Core
{
    public class AmazonEventBridgeService : IAmazonEventBridgeService
    {
        private readonly IAmazonEventBridgeClientFactory amazonEventBridgeClientFactory;

        public AmazonEventBridgeService(IAmazonEventBridgeClientFactory amazonEventBridgeClientFactory)
        {
            this.amazonEventBridgeClientFactory = amazonEventBridgeClientFactory;
        }
    }

}
