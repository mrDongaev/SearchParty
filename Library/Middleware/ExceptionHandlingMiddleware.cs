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
                Messages = [],
            });

            _logger.Error(ex, "An error has occurred with code {codeNum}: {@statusCode}", codeNum, statusCode);

            await context.Response.WriteAsync(result);
        }

        private List<string> GetExceptionInfo(Exception exception, out HttpStatusCode statusCode)
        {
            List<string> errors = new();

            if (exception is AggregateException aggEx)
            {
                errors = ParseAggregateException(aggEx, out statusCode);
            }
            else
            {
                var error = ParseException(exception, out statusCode);
                errors.Add(error);
            }
            return errors;
        }

        private List<string> ParseAggregateException(AggregateException aggregateException, out HttpStatusCode statusCode)
        {
            List<string> errors = [];
            HttpStatusCode firstStatusCode = HttpStatusCode.InternalServerError;
            for (var i = 0; i < aggregateException.InnerExceptions.Count; i++)
            {
                var ex = aggregateException.InnerExceptions[i];
                string error;
                if (i == 0)
                {
                    error = ParseException(ex, out firstStatusCode);
                }
                else
                {
                    error = ParseException(ex, out _);
                }
                errors.Add(error);
            }
            statusCode = firstStatusCode;
            return errors;
        }

        private static string ParseException(Exception exception, out HttpStatusCode statusCode)
        {
            statusCode = HttpStatusCode.InternalServerError;
            statusCode = exception switch
            {
                InvalidEnumMemberException or InvalidClassMemberException => HttpStatusCode.BadRequest,
                DbUpdateException or DbUpdateConcurrencyException or HttpRequestException or AutoMapperMappingException or SystemException => HttpStatusCode.InternalServerError,
                _ => HttpStatusCode.InternalServerError,

            };
            return string.IsNullOrEmpty(exception.Message) ? "An unforeseen error has occurred" : exception.Message;
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
