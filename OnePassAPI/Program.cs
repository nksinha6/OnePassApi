using OnePass.API;
using OnePass.Infrastructure;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services
    .AddApplicationServices()
    .AddInfrastructure(builder.Configuration);

builder.Services.AddOpenTelemetry()
    .WithTracing(tracerProviderBuilder =>
    {
        tracerProviderBuilder
            .SetResourceBuilder(ResourceBuilder.CreateDefault().AddService("OnePass.API"))
            .AddAspNetCoreInstrumentation()
            .AddHttpClientInstrumentation()
            .AddConsoleExporter(); // Replace or add OTLP/Jaeger here
    });
builder.Services.AddSingleton<Tracer>(sp =>
{
    var tracerProvider = sp.GetRequiredService<TracerProvider>();
    return tracerProvider.GetTracer("OnePass.API");
});
builder.Services.AddControllers();
builder.Services.AddMemoryCache();
// ✅ Add OpenTelemetry Exporter (Prometheus, OTLP, etc.)
builder.Services.AddOpenTelemetry()
    .WithMetrics(metrics =>
    {
        metrics.AddAspNetCoreInstrumentation();
        metrics.AddHttpClientInstrumentation();
        metrics.AddMeter("OnePass.API.Requests");
        // 👉 Choose where to export: Prometheus, OTLP, Console, etc.
        metrics.AddPrometheusExporter();
    })
    .WithTracing(tracing =>
    {
        tracing.AddAspNetCoreInstrumentation();
        tracing.AddHttpClientInstrumentation();
        tracing.AddOtlpExporter(); // or Jaeger/Zipkin
    });

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
MapsterConfig.RegisterMappings();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.UseRequestTracing();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
