using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

namespace ECourse.Shared.Observability;
public static class OpenTelemetryExtensions
{
    public static IServiceCollection AddECourseObservability(
        this IServiceCollection services,
        IConfiguration configuration,
        string environmentName,
        string serviceName, string activitySourceName, string serviceVersion)
    {
        services.AddOpenTelemetry()
            .ConfigureResource(resource =>
            {
                resource
                .AddService(serviceName: serviceName, serviceVersion: serviceVersion)
                .AddAttributes(new Dictionary<string, object>
                {
                    ["deployment.environment.name"] = environmentName
                });

            })
 .WithTracing(tracing =>
 {
     tracing.AddAspNetCoreInstrumentation(options =>
     {
         options.Filter = httpCpntext =>
         !httpCpntext.Request.Path.StartsWithSegments("/scalar") &&
         !httpCpntext.Request.Path.StartsWithSegments("/health");
     })
     .AddHttpClientInstrumentation()
     .AddSource(activitySourceName)
     .AddOtlpExporter(option =>
     {
         option.Endpoint = new Uri(configuration["OtlpExporter:Endpoint"]!);
     });
 })
 .WithMetrics(metrics =>
 {
     metrics
         .AddAspNetCoreInstrumentation()
         .AddHttpClientInstrumentation()
         .AddRuntimeInstrumentation()
         .AddMeter(activitySourceName)
         .AddOtlpExporter(option =>
         {
             option.Endpoint = new Uri(configuration["OtlpExporter:Endpoint"]!);
         });
 });

        return services;
    }
}
