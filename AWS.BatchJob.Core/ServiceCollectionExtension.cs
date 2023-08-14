using Amazon.Batch;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AWS.BatchJob.Core
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection AddEventBridgeConfig(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<BatchJobConfigOptions>(
                configuration.GetSection(nameof(BatchJobConfigOptions)));
            return services;
        }

        public static IServiceCollection AddS3Services(this IServiceCollection services)
        {
            services.AddSingleton<IAmazonBatchJobClientFactory, AmazonBatchJobClientFactory>();
            services.AddScoped<IAmazonBatchJobService, AmazonBatchJobService>();            
            return services;
        }
    }
}
