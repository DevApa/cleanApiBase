using System.Reflection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using bg.crm.integration.infrastructure.observability;
using bg.crm.integration.shared.extensions;

namespace bg.crm.integration.infrastructure.ioc
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddMemoryCache();
            services.AddHttpClient();
            services.AddOpenTelemetry(configuration);
            services.AddLoggingService(configuration);
            services.AddHealthChecks();

            services.AddAutoMapper(Assembly.GetExecutingAssembly());
            services.AddServicesFromAssembly(Assembly.GetExecutingAssembly());
            return services;
        }
    }
}