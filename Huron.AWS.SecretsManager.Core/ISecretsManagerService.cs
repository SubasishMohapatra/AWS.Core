using System.Net;

namespace Huron.AWS.SecretsManager.Core
{
    public interface ISecretsManagerService
    {
        Task<string> GetSecretAsync(string secretName);
    }
}