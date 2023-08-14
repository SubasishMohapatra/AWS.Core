namespace AWS.EventBridge.Core
{
    public class EventBridgeConfigOptions
    {
        public string AWSRegion { get; set; } = string.Empty;
        public string AWSAccountId { get; set; } = string.Empty;
        public string IamAccessKey { get; set; }= string.Empty;
        public string IamSecretKey { get; set; } = string.Empty;
    }

}
