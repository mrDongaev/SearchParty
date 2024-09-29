using Common.Exceptions;
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
            var statusCode = (HttpStatusCode) context.Response.StatusCode;
            var errorMsg = exception.Message.Equals(string.Empty) ? "An unforeseen error has occurred" : exception.Message;
            if (exception is InvalidEnumMemberException)
            {
                statusCode = HttpStatusCode.BadRequest;
                errorMsg = exception.Message;
            }
            else if (exception is InvalidClassMemberException)
            {
                statusCode = HttpStatusCode.BadRequest;
                errorMsg = exception.Message;
            }
            else if (exception is TeamCountOverflowException)
            {
                statusCode = HttpStatusCode.BadRequest;
                errorMsg = exception.Message;
            }
            else if (exception is TeamOwnerNotPresentException)
            {
                statusCode = HttpStatusCode.BadRequest;
                errorMsg = exception.Message;
            }
            else if (exception is TeamPositionOverlapException)
            {
                statusCode = HttpStatusCode.BadRequest;
                errorMsg = exception.Message;
            }
            context.Response.ContentType = "application/json";
            int codeNum = (int)statusCode;
            var result = JsonSerializer.Serialize(new
            {
                StatusCode = statusCode,
                ErrorMsg = errorMsg,
            });
            _logger.Error(exception, "An error has occurred with code {codeNum}: {@statusCode}", codeNum, statusCode);
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
