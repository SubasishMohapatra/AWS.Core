using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AWS.EventBridge.Core
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection AddEventBridgeConfig(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<EventBridgeConfigOptions>(
                configuration.GetSection(nameof(EventBridgeConfigOptions)));
            return services;
        }

        public static IServiceCollection AddS3Services(this IServiceCollection services)
        {
            services.AddSingleton<IAmazonEventBridgeClientFactory, AmazonEventBridgeClientFactory>();
            services.AddScoped<IAmazonEventBridgeService, AmazonEventBridgeService>();            
            return services;
        }
    }
}
