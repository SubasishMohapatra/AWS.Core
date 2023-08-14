namespace AWS.BatchJob.Core
{
    public class BatchJobConfigOptions
    {
        public string AWSRegion { get; set; } = string.Empty;
        public string AWSAccountId { get; set; } = string.Empty;
        public string IamAccessKey { get; set; }= string.Empty;
        public string IamSecretKey { get; set; } = string.Empty;
    }

}
