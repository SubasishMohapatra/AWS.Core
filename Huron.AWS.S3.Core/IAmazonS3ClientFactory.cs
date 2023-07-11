using Amazon.S3;

namespace Huron.AWS.S3.Core
{
    public interface IAmazonS3ClientFactory
    {
        IAmazonS3 GetAmazonS3Client();
        string GetS3BucketName();
    }
}