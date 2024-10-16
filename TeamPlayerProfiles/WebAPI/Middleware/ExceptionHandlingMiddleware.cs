using System.Net;
using System.Text.Json;
using ILogger = Serilog.ILogger;

namespace WebAPI.Middleware
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;

        private readonly ILogger _logger;

        public ExceptionHandlingMiddleware(ILogger logger, RequestDelegate next)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            var code = HttpStatusCode.BadRequest;
            var errorMsg = "An unforeseen error has occurred";
            if (exception is ArgumentException)
            {
                code = HttpStatusCode.InternalServerError;
                errorMsg = exception.Message;
            }

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)code;
            var statusCode = (int)code;
            var result = JsonSerializer.Serialize(new
            {
                StatusCode = statusCode,
                ErrorMsg = errorMsg,
            });
            _logger.Error(exception, "An error has occurred with code {statusCode}: {@code}", statusCode, code);
            await context.Response.WriteAsync(result);
        }
    }

    public static class ExceptionHandlingMiddlewareExtensions
    {
        public static IApplicationBuilder UseExceptionHandling(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<ExceptionHandlingMiddleware>();
        }
    }
}
