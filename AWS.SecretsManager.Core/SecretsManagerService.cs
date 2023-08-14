using Amazon.SecretsManager.Model;
using AWS.SharedLib;
using HuRo.S3.Core;
using Polly;
using Serilog;

namespace AWS.SecretsManager.Core
{
    public class SecretsManagerService : ISecretsManagerService
    {
        private readonly ISecretsManagerClientFactory smClientFactory;
        private readonly ILogger logger;
        public SecretsManagerService(ISecretsManagerClientFactory smClientFactory, ILogger logger)
        {
            this.smClientFactory = smClientFactory;
            this.logger = logger;
        }

        public async Task<string> GetSecretAsync(string secretName)
        {
            var result = await PollyRetry.ExecuteAsync<string>(async () =>
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
            },
            SecretsManagerFallbackAsync, SecretsManagerFallbackActionOnFallbackAsync, this.logger);
            return result;
        }

        private Task SecretsManagerFallbackActionOnFallbackAsync(DelegateResult<string> response, Context context)
        {
            //Console.WriteLine("About to call the fallback action. This is a good place to do some logging");
            return Task.CompletedTask;
        }

        private Task<string> SecretsManagerFallbackAsync(DelegateResult<string> responseToFailedRequest, Context context, CancellationToken cancellationToken)
        {
            var ex = responseToFailedRequest.Exception;
            logger.Error("SecretsManagerFallbackAsync error: {Error}", ex.Message);
            return Task.FromResult(ex.Message);
        }
    }

}
