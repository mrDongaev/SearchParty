using AutoMapper;
using Library.Exceptions;
using Library.Models.HttpResponses;
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
            HttpStatusCode statusCode;

            var errors = GetExceptionInfo(ex, out statusCode);

            int codeNum = (int)statusCode;

            context.Response.StatusCode = codeNum;
            context.Response.ContentType = "application/json";

            var result = JsonSerializer.Serialize(new HttpResponseBody
            {
                IsSuccess = false,
                Errors = errors,
                Messages = errors,
            });

            _logger.Error(ex, "An error has occurred with code {codeNum}: {@statusCode}", codeNum, statusCode);

            await context.Response.WriteAsync(result);
        }

        private Dictionary<string, string[]> GetExceptionInfo(Exception exception, out HttpStatusCode statusCode)
        {
            Dictionary<string, string[]> errors = [];

            if (exception is AggregateException aggEx)
            {
                errors = ParseAggregateException(aggEx, out statusCode);
            }
            else
            {
                var error = ParseException(exception, out statusCode);
                errors.Add(error.Key, [error.Value]);
            }
            return errors;
        }

        private Dictionary<string, string[]> ParseAggregateException(AggregateException aggregateException, out HttpStatusCode statusCode)
        {
            Dictionary<string, List<string>> errors = [];
            HttpStatusCode firstStatusCode = HttpStatusCode.InternalServerError;
            for (var i = 0; i < aggregateException.InnerExceptions.Count; i++)
            {
                var ex = aggregateException.InnerExceptions[i];
                KeyValuePair<string, string> error;
                if (i == 0)
                {
                    error = ParseException(ex, out firstStatusCode);
                }
                else
                {
                    error = ParseException(ex, out _);
                }
                if (errors.TryGetValue(error.Key, out var list))
                {
                    list.Add(error.Value);
                }
                else
                {
                    errors.Add(error.Key, new List<string> { error.Value });
                }
            }
            statusCode = firstStatusCode;
            return errors.Select(d => KeyValuePair.Create(d.Key, d.Value.ToArray())).ToDictionary();
        }

        private static KeyValuePair<string, string> ParseException(Exception exception, out HttpStatusCode statusCode)
        {
            statusCode = HttpStatusCode.InternalServerError;
            statusCode = exception switch
            {
                InvalidEnumMemberException or InvalidClassMemberException => HttpStatusCode.BadRequest,
                DbUpdateException or DbUpdateConcurrencyException or HttpRequestException or AutoMapperMappingException or SystemException => HttpStatusCode.InternalServerError,
                _ => HttpStatusCode.InternalServerError,

            };
            var message = string.IsNullOrEmpty(exception.Message) ? "An unforeseen error has occurred" : exception.Message;
            return KeyValuePair.Create(nameof(exception), message);
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
