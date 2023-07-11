using Amazon;
using Amazon.Runtime;
using Amazon.SecretsManager;
using Microsoft.Extensions.Options;
using static Org.BouncyCastle.Math.EC.ECCurve;

namespace Huron.AWS.SecretsManager.Core
{
    public class SecretsManagerClientFactory : ISecretsManagerClientFactory
    {
        private readonly string s3Region;
        private readonly string iamAccessKey;
        private readonly string iamSecretKey;

        public SecretsManagerClientFactory(IOptions<SecretsManagerConfigOptions> options)
        {
            var amazonS3Options = options.Value;
            this.s3Region = amazonS3Options.S3Region;
            this.iamAccessKey = amazonS3Options.IamAccessKey;
            this.iamSecretKey = amazonS3Options.IamSecretKey;
        }

        public IAmazonSecretsManager GetSecretsManagerClient()
        {
            var config = new AmazonSecretsManagerConfig
            {
                RegionEndpoint = RegionEndpoint.GetBySystemName(s3Region)
            };

            if (Environment.GetEnvironmentVariable("environment") == "dev")
                return new AmazonSecretsManagerClient(iamAccessKey, iamSecretKey, config); //for local debug

            return new AmazonSecretsManagerClient(config);
        }
    }
}
