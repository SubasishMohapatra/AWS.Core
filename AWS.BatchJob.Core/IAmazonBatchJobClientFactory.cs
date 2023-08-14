using Amazon.Batch;

namespace AWS.BatchJob.Core
{
    public interface IAmazonBatchJobClientFactory
    {
        IAmazonBatch GetBatchJobClient();        
    }    
}