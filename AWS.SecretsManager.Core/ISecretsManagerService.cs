using System.Net;

namespace AWS.SecretsManager.Core
{
    public interface ISecretsManagerService
    {
        Task<string> GetSecretAsync(string secretName);
    }
}