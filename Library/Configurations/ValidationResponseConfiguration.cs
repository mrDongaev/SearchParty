using Library.Models.HttpResponses;
using Library.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Library.Configurations
{
    public static class ValidationResponseConfiguration
    {
        public static IMvcBuilder AddValidationResponseConfiguration(this IMvcBuilder mvcBuilder)
        {
            mvcBuilder.ConfigureApiBehaviorOptions(options =>
            {
                options.InvalidModelStateResponseFactory = context =>
                {
                    var problemDetails = new ValidationProblemDetails(context.ModelState)
                    {
                        Type = "https://tools.ietf.org/html/rfc7231#section-6.5.1",
                        Title = "One or more validation errors occurred.",
                        Status = StatusCodes.Status400BadRequest,
                        Detail = "Please refer to the errors property for additional details.",
                        Instance = context.HttpContext.Request.Path
                    };

                    var response = new HttpResponseBody
                    {
                        IsSuccess = false,
                        Errors = problemDetails.Errors,
                    };

                    return new BadRequestObjectResult(response)
                    {
                        ContentTypes = { "application/json" }
                    };
                };
            });

            return mvcBuilder;
        }
    }
}
