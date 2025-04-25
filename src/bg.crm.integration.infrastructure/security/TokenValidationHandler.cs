using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Logging;

namespace bg.crm.integration.infrastructure.security
{
    public static class TokenValidationHandler
    {
        public static IServiceCollection SetupAuthenticationServices(this IServiceCollection services, WebApplicationBuilder builder)
        {
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.Audience = builder.Configuration["AzureAuth:Audience"];
                    options.Authority = builder.Configuration["AzureAuth:Authority"];
                    options.TokenValidationParameters.ValidAudiences = new string[] { options.Audience!, $"api://{options.Audience}", "https://login.microsoftonline.com/{options.Audience}/v2.0" };
                    options.TokenValidationParameters.ValidateLifetime = true;
                    options.TokenValidationParameters.ValidateIssuer = true;
                    options.TokenValidationParameters.ValidateAudience = true;
                    options.TokenValidationParameters.ClockSkew = TimeSpan.Zero;
                    options.Events = new JwtBearerEvents();
                    options.RequireHttpsMetadata = false;
                });
            IdentityModelEventSource.ShowPII = true;
            return services;
        }
    }
}