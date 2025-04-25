using System.Reflection;
using Microsoft.Extensions.DependencyInjection;

namespace bg.crm.integration.shared.extensions
{
    public static class DependencyInjectionExtensions
    {
        public static IServiceCollection AddServicesFromAssembly(this IServiceCollection services, Assembly assembly)
        {
            services.Scan(scan => scan
                .FromAssemblies(assembly)
                .AddClasses(classes => classes.AssignableTo<IServiceScoped>())
                    .AsImplementedInterfaces(i => i != typeof(IServiceScoped))
                    .WithScopedLifetime()
                .AddClasses(classes => classes.AssignableTo<IServiceSingleton>())
                    .AsImplementedInterfaces(i => i != typeof(IServiceSingleton))
                    .WithSingletonLifetime()
                .AddClasses(classes => classes.AssignableTo<IServiceTransient>())
                    .AsImplementedInterfaces(i => i != typeof(IServiceTransient))
                    .WithTransientLifetime()
            );

            return services;
        }
    }
}