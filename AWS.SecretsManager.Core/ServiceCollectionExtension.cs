using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
namespace AWS.SecretsManager.Core
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection AddSecretsManagerConfig(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<SecretsManagerConfigOptions>(
               configuration.GetSection(nameof(SecretsManagerConfigOptions)));
            return services;
        }

        public static IServiceCollection AddSecretsManagerServices(this IServiceCollection services)
        {
            services.AddSingleton<ISecretsManagerClientFactory, SecretsManagerClientFactory>();
            services.AddScoped<ISecretsManagerService, SecretsManagerService>();
            return services;
        }
    }
}
