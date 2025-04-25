using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using bg.crm.integration.shared.extensions;

namespace bg.crm.integration.application.ioc
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddServicesFromAssembly(Assembly.GetExecutingAssembly());
            services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
            return services;
        }
    }
}