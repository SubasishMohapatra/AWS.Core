namespace AWS.SecretsManager.Core
{
    public class SecretsManagerConfigOptions
    {
        public string S3Region { get; set; } = string.Empty;
        public string IamAccessKey { get; set; } = string.Empty;
        public string IamSecretKey { get; set; } = string.Empty;
    }

    public class SecretsManagerNames
    {
        public string SecretDBName { get; init; }
    }
}
