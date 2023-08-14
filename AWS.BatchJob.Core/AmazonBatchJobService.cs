namespace AWS.BatchJob.Core
{
    public class AmazonBatchJobService : IAmazonBatchJobService
    {
        private readonly IAmazonBatchJobClientFactory amazonBatchJobClientFactory;

        public AmazonBatchJobService(IAmazonBatchJobClientFactory amazonBatchJobClientFactory)
        {
            this.amazonBatchJobClientFactory = amazonBatchJobClientFactory;
        }
    }

}
