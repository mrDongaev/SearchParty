﻿using Microsoft.EntityFrameworkCore;
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
            var statusCode = (HttpStatusCode)context.Response.StatusCode;
            if (exception is DbUpdateException || exception is DbUpdateConcurrencyException || exception is SystemException)
            {
                statusCode = HttpStatusCode.InternalServerError;
            }
            if ((int)statusCode < 400)
            {
                statusCode = HttpStatusCode.BadRequest;
            }
            var errorMsg = string.IsNullOrEmpty(exception.Message) ? "An unforeseen error has occurred" : exception.Message;
            int codeNum = (int)statusCode;

            context.Response.StatusCode = codeNum;
            context.Response.ContentType = "application/json";

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
