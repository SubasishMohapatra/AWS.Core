using Amazon.SecretsManager;

namespace Huron.AWS.SecretsManager.Core
{
    public interface ISecretsManagerClientFactory
    {
        IAmazonSecretsManager GetSecretsManagerClient();
    }
}