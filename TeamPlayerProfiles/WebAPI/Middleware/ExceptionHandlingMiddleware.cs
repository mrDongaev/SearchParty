using AutoMapper;
using Common.Exceptions;
using Library.Exceptions;
using Microsoft.EntityFrameworkCore;
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

        public async Task InvokeAsync(HttpContext context)
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

        private async Task HandleExceptionAsync(HttpContext context, Exception ex)
        {
            var statusCode = (HttpStatusCode)context.Response.StatusCode;
            if (ex is InvalidEnumMemberException || ex is InvalidClassMemberException ||
                ex is TeamCountOverflowException || ex is TeamOwnerNotPresentException ||
                ex is TeamPositionOverlapException || ex is TeamContainsPlayerException || ex is PendingMessageExistsException)
            {
                statusCode = HttpStatusCode.BadRequest;
            }
            else if (ex is DbUpdateException || ex is DbUpdateConcurrencyException ||
                ex is HttpRequestException || ex is AutoMapperMappingException || ex is SystemException)
            {
                statusCode = HttpStatusCode.InternalServerError;
            }
            if ((int)statusCode < 400)
            {
                statusCode = HttpStatusCode.BadRequest;
            }
            var errorMsg = string.IsNullOrEmpty(ex.Message) ? "An unforeseen error has occurred" : ex.Message;
            int codeNum = (int)statusCode;

            context.Response.StatusCode = codeNum;
            context.Response.ContentType = "application/json";

            var result = JsonSerializer.Serialize(new
            {
                StatusCode = statusCode,
                ErrorMsg = errorMsg,
            });

            _logger.Error(ex, "An error has occurred with code {codeNum}: {@statusCode}", codeNum, statusCode);

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
