using Amazon.SecretsManager.Model;
using HuRo.S3.Core;
using MySql.Data.MySqlClient;

namespace AWS.SecretsManager.Core
{
    public class SecretsManagerService : ISecretsManagerService
    {
        private readonly ISecretsManagerClientFactory smClientFactory;

        public SecretsManagerService(ISecretsManagerClientFactory smClientFactory)
        {
            this.smClientFactory = smClientFactory;
        }

        public async Task<string> GetSecretAsync(string secretName)
        {
            var smClient = smClientFactory.GetSecretsManagerClient();

            GetSecretValueRequest request = new GetSecretValueRequest
            {
                SecretId = secretName,
                VersionStage = SecretsManagerConstants.AWSCURRENT,
            };

            GetSecretValueResponse response;
            response = await smClient.GetSecretValueAsync(request);

            var secret = response.SecretString;

            return secret;
        }
    }

}
