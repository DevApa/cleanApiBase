using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using Prometheus;
using Serilog;

namespace bg.crm.integration.infrastructure.observability
{
    public static class Monitoring
    {
        public static IServiceCollection AddLoggingService(this IServiceCollection services, IConfiguration configuration)
        {
            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(configuration)
                .CreateLogger();

            services.AddLogging(loggingBuilder => loggingBuilder.AddSerilog(dispose: true));
            return services;
        }

        public static IServiceCollection AddOpenTelemetry(this IServiceCollection services, IConfiguration configuration)
        {
            _ = int.TryParse(configuration["Jaeger:Telemetry:Port"], out var portNumber);

            services.AddOpenTelemetry().WithTracing(tracerBuilder =>
            {
                tracerBuilder
                .AddSource(configuration["Jaeger:Telemetry:Source"] ?? typeof(Monitoring).Namespace ?? string.Empty)
                .SetResourceBuilder(ResourceBuilder.CreateDefault().AddService(configuration["Jaeger:Telemetry:ServiceName"] ?? string.Empty))
                .AddHttpClientInstrumentation()
                .AddAspNetCoreInstrumentation(options =>
                {
                    options.EnrichWithHttpRequest = (activity, httpRequest) =>
                    {
                        if (httpRequest != null)
                        {
                            var context = httpRequest.HttpContext;
                            var traceId = context.TraceIdentifier;
                            if (!string.IsNullOrEmpty(traceId))
                                activity.SetTag("traceId", traceId);

                            var forwardedFor = context.Request.Headers["X-Forwarded-For"].FirstOrDefault()!.Split(',').Select(x => x.Trim()).FirstOrDefault();
                            if (!string.IsNullOrEmpty(forwardedFor))
                                activity.SetTag("X-Forwarded-For", forwardedFor);

                            var remoteIp = context.Connection.RemoteIpAddress?.ToString();
                            var remoteAddHeader = context.Request.Headers["REMOTE_ADDR"].FirstOrDefault()!.Split(',').Select(x => x.Trim()).FirstOrDefault();
                            var clientAddress = string.IsNullOrWhiteSpace(remoteIp) ? remoteAddHeader : remoteIp;

                            if (!string.IsNullOrEmpty(clientAddress))
                                activity.SetTag("client-Address", clientAddress);
                        }
                    };
                })
                .AddSqlClientInstrumentation(options =>
                {
                    options.EnableConnectionLevelAttributes = true;
                    options.SetDbStatementForText = true;
                    options.SetDbStatementForStoredProcedure = true;
                    options.RecordException = false;
                    options.Enrich = (activity, x, y) => activity.SetTag("db.type", "sql");

                })
                .AddJaegerExporter(jaegerOptions =>
                {
                    jaegerOptions.AgentHost = configuration["Jaeger:Telemetry:Host"] ?? string.Empty;
                    jaegerOptions.AgentPort = portNumber;
                });
            });

            return services;
        }

        public static IServiceCollection AddHealthChecksService(this IServiceCollection services)
        {
            services.AddHealthChecks();
            return services;
        }

        public static IApplicationBuilder ConfigureMetricServerApp(this IApplicationBuilder app)
        {
            app.UseMetricServer();
            app.UseHttpMetrics();
            return app;
        }

        public static IApplicationBuilder ConfigureHealthChecksApp(this IApplicationBuilder app)
        {
            app.UseRouting();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapHealthChecks("/health/readiness", new HealthCheckOptions
                {
                    AllowCachingResponses = false,
                    ResultStatusCodes =
                    {
                        [HealthStatus.Healthy] = StatusCodes.Status200OK,
                        [HealthStatus.Degraded] = StatusCodes.Status200OK,
                        [HealthStatus.Unhealthy] = StatusCodes.Status503ServiceUnavailable
                    }
                });

                endpoints.MapHealthChecks("/health/liveness", new HealthCheckOptions
                {
                    AllowCachingResponses = false,
                    ResultStatusCodes =
                    {
                        [HealthStatus.Healthy] = StatusCodes.Status200OK,
                        [HealthStatus.Degraded] = StatusCodes.Status200OK,
                        [HealthStatus.Unhealthy] = StatusCodes.Status503ServiceUnavailable
                    },
                    Predicate = _ => false
                });
            });
            return app;
        }

    }
}