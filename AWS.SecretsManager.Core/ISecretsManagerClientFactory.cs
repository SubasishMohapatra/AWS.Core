using Amazon.SecretsManager;

namespace AWS.SecretsManager.Core
{
    public interface ISecretsManagerClientFactory
    {
        IAmazonSecretsManager GetSecretsManagerClient();
    }
}