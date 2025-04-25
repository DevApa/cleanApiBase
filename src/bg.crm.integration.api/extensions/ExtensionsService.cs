using Microsoft.AspNetCore.Mvc;

namespace bg.crm.integration.api.extensions
{
    public static class Extensions
    {
        public static IServiceCollection RegisterDependencies(this IServiceCollection services)
        {
            services.AddApiVersioning(options =>
            {
                options.ReportApiVersions = true;
                options.AssumeDefaultVersionWhenUnspecified = true;
                options.DefaultApiVersion = new ApiVersion(1, 0);
            });
            return services;
        }
    }
}