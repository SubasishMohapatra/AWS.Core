using Amazon.EventBridge;

namespace AWS.EventBridge.Core
{
    public interface IAmazonEventBridgeClientFactory
    {
        IAmazonEventBridge GetEventBridgeClient();        
    }    
}