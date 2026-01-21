using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using OnePass.API;
using OnePass.Domain;
using OnePass.Domain.Services;
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

builder.Services.AddHttpClient("Cashfree", client =>
{
    var cfg = builder.Configuration.GetSection("Cashfree");
    client.BaseAddress = new Uri(cfg["ApiBaseUrl"]);
    client.DefaultRequestHeaders.Add("x-client-id", cfg["ClientId"]);
    client.DefaultRequestHeaders.Add("x-client-secret", cfg["ClientSecret"]);
}).AddAsKeyed();

builder.Services.AddScoped<IDigilockerService, DigilockerService>();

builder.Services.AddScoped<IFaceVerificationService, FaceVerificationService>();

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
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        policy =>
        {
            policy.WithOrigins("http://localhost:5173", "https://seashell-app-dmof6.ondigitalocean.app",
"https://lionfish-app-6ymn6.ondigitalocean.app",
    "https://authiko.in")
                      .AllowAnyHeader()
                  .AllowAnyMethod()
                  .SetPreflightMaxAge(TimeSpan.FromHours(1));
        });
});

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"])
        ),
        NameClaimType = JwtRegisteredClaimNames.Sub,  // ✅ Important
        RoleClaimType = ClaimTypes.Role,
        ClockSkew = TimeSpan.Zero
    };      
});

// Bind Msg91Options from configuration (appsettings.json or environment variables)
builder.Services.Configure<Msg91Options>(builder.Configuration.GetSection("Msg91"));

// Register HttpClient for Msg91 service using IHttpClientFactory
builder.Services.AddHttpClient<Msg91SmsService>(client =>
{
    // optional: configure client timeouts
    client.Timeout = TimeSpan.FromSeconds(30);
    // don't set BaseAddress here — we use the ApiUrl from options inside the service
})
// THIS ensures HttpClientHandler will not use any system or env proxy (fixes 127.0.0.1:8888)
.ConfigurePrimaryHttpMessageHandler(() => new HttpClientHandler
{
    UseProxy = false
})
.AddTypedClient<Msg91SmsService>((http, provider) =>
{
    var opts = provider.GetRequiredService<Microsoft.Extensions.Options.IOptions<Msg91Options>>().Value;
    var logger = provider.GetRequiredService<ILogger<Msg91SmsService>>();
    return new Msg91SmsService(http, Microsoft.Extensions.Options.Options.Create(opts), logger);
});

// Register interface to implementation
builder.Services.AddScoped<ISmsService>(sp => sp.GetRequiredService<Msg91SmsService>());

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new DateOnlyJsonConverter());
    });

MapsterConfig.RegisterMappings();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.UseRouting();

app.UseCors("AllowAll");

app.MapMethods("{*path}", new[] { "OPTIONS" }, (HttpContext ctx) =>
{
    ctx.Response.StatusCode = StatusCodes.Status200OK;
    return ctx.Response.WriteAsync("OK");
})
.RequireCors("AllowAll");

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();
app.UseMiddleware<RequestContextMiddleware>();

app.MapControllers();
app.Run();
