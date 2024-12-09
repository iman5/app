namespace App.API.Middlewares;

/// <summary>
/// Middleware for logging request and response details.
/// </summary>
public class RequestResponseLoggingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<RequestResponseLoggingMiddleware> _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="RequestResponseLoggingMiddleware"/> class.
    /// </summary>
    /// <param name="next">The next middleware in the pipeline.</param>
    /// <param name="logger">Logger for logging messages.</param>
    public RequestResponseLoggingMiddleware(RequestDelegate next, ILogger<RequestResponseLoggingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    /// <summary>
    /// Invokes the middleware to log request and response details.
    /// </summary>
    /// <param name="context">The HTTP context.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    public async Task Invoke(HttpContext context)
    {
        // Log Request details
        _logger.LogInformation("Handling request: {Method} {Path} at {Time}", context.Request.Method, context.Request.Path, DateTime.UtcNow);

        var originalBodyStream = context.Response.Body;

        using (var responseBody = new MemoryStream())
        {
            context.Response.Body = responseBody;

            await _next(context);

            // Log Response details
            _logger.LogInformation("Response: {StatusCode} at {Time}", context.Response.StatusCode, DateTime.UtcNow);

            await responseBody.CopyToAsync(originalBodyStream);
        }
    }
}

/// <summary>
/// Extension methods for adding the <see cref="RequestResponseLoggingMiddleware"/> to the application pipeline.
/// </summary>
public static class RequestResponseLoggingMiddlewareExtensions
{
    /// <summary>
    /// Adds the <see cref="RequestResponseLoggingMiddleware"/> to the application pipeline.
    /// </summary>
    /// <param name="builder">The application builder.</param>
    /// <returns>The application builder.</returns>
    public static IApplicationBuilder UseRequestResponseLogging(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<RequestResponseLoggingMiddleware>();
    }
}
