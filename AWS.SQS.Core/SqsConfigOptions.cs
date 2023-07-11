namespace AWS.SQS.Core
{
    public class SqsConfigOptions
    {
        public string SqsRegion { get; set; } = string.Empty;
        public string AccountId { get; set; } = string.Empty;
        public string IamAccessKey { get; set; } = string.Empty;
        public string IamSecretKey { get; set; } = string.Empty;
    }
}
