using System.Diagnostics.Metrics;
using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace OnePass.API
{
    public class RequestTracingMiddleware
    {
        private const string CorrelationIdHeader = "X-Correlation-ID";
        private readonly RequestDelegate _next;
        private readonly ILogger<RequestTracingMiddleware> _logger;
        private readonly long _slowRequestThresholdMs = 1000;

        // ✅ OpenTelemetry Metrics
        private static readonly Meter _meter = new("OnePass.API.Requests", "1.0.0");
        private static readonly Counter<long> _requestCounter = _meter.CreateCounter<long>("http_requests_total", "requests", "Total HTTP requests");
        private static readonly Histogram<double> _requestDuration = _meter.CreateHistogram<double>("http_request_duration_ms", "ms", "Duration of HTTP requests");
        private static readonly Counter<long> _errorCounter = _meter.CreateCounter<long>("http_requests_errors_total", "requests", "Total HTTP request errors");

        public RequestTracingMiddleware(RequestDelegate next, ILogger<RequestTracingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            // ✅ Correlation ID
            string correlationId = context.Request.Headers.ContainsKey(CorrelationIdHeader)
                ? context.Request.Headers[CorrelationIdHeader]!
                : Guid.NewGuid().ToString();

            // ✅ Add correlation ID to response
            context.Response.OnStarting(() =>
            {
                context.Response.Headers[CorrelationIdHeader] = correlationId;
                return Task.CompletedTask;
            });

            // ✅ Stopwatch for latency
            var stopwatch = Stopwatch.StartNew();

            // ✅ OpenTelemetry Activity for distributed tracing
            var activity = new Activity("HTTP Request");
            activity.AddTag("CorrelationId", correlationId);
            activity.AddTag("HttpMethod", context.Request.Method);
            activity.AddTag("Path", context.Request.Path);
            activity.Start();

            using (_logger.BeginScope(new Dictionary<string, object> { ["CorrelationId"] = correlationId }))
            {
                try
                {
                    // 🔢 Increment request count
                    _requestCounter.Add(1, new KeyValuePair<string, object?>("method", context.Request.Method),
                                           new KeyValuePair<string, object?>("path", context.Request.Path));

                    _logger.LogInformation("➡️ Request Started: {Method} {Path} | CorrelationId={CorrelationId}",
                        context.Request.Method, context.Request.Path, correlationId);

                    await _next(context);

                    stopwatch.Stop();

                    // 🎯 Record duration to histogram
                    _requestDuration.Record(stopwatch.Elapsed.TotalMilliseconds,
                        new KeyValuePair<string, object?>("method", context.Request.Method),
                        new KeyValuePair<string, object?>("path", context.Request.Path),
                        new KeyValuePair<string, object?>("status_code", context.Response.StatusCode));

                    _logger.LogInformation("✅ Request Completed: {Method} {Path} | Status={StatusCode} | Duration={ElapsedMs} ms | CorrelationId={CorrelationId}",
                        context.Request.Method, context.Request.Path, context.Response.StatusCode, stopwatch.ElapsedMilliseconds, correlationId);

                    if (stopwatch.ElapsedMilliseconds > _slowRequestThresholdMs)
                    {
                        _logger.LogWarning("⚠️ Slow Request: {Method} {Path} took {ElapsedMs} ms | CorrelationId={CorrelationId}",
                            context.Request.Method, context.Request.Path, stopwatch.ElapsedMilliseconds, correlationId);
                    }
                }
                catch (Exception ex)
                {
                    stopwatch.Stop();

                    // 🔢 Increment error count
                    _errorCounter.Add(1, new KeyValuePair<string, object?>("method", context.Request.Method),
                                         new KeyValuePair<string, object?>("path", context.Request.Path));

                    _logger.LogError(ex, "❌ Exception during {Method} {Path} | Duration={ElapsedMs} ms | CorrelationId={CorrelationId}",
                        context.Request.Method, context.Request.Path, stopwatch.ElapsedMilliseconds, correlationId);

                    throw; // rethrow to let exception handler middleware process it
                }
                finally
                {
                    activity.Stop();
                }
            }
        }
    }
}
