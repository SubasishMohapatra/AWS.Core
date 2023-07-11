using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Huron.AWS.SQS.Core
{

    public static class ServiceCollectionExtension
    {
        public static IServiceCollection AddSqsDependencies(this IServiceCollection services)
        {
            services.AddSingleton<IAmazonSqsClientFactory, AmazonSqsClientFactory>();
            services.AddTransient<IAmazonSqsService, AmazonSqsService>();
            return services;
        }
        public static IServiceCollection AddSqsConfig(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<SqsConfigOptions>(
                configuration.GetSection(nameof(SqsConfigOptions)));
            return services;
        }
    }
}
