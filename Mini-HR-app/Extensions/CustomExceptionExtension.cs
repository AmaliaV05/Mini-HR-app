using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Mini_HR_app.Exceptions;
using Mini_HR_app.ViewModels;
using System;
using System.Net;
using System.Threading.Tasks;

namespace Mini_HR_app.Extensions
{
    public class CustomExceptionExtension
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<CustomExceptionExtension> _logger;

        public CustomExceptionExtension(RequestDelegate next, ILogger<CustomExceptionExtension> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch (AccessViolationException avEx)
            {
                _logger.LogError($"A new violation exception has been thrown: {avEx}");
                await HandleExceptionAsync(httpContext, avEx);
            }
            catch (InvalidCompanyException icEx)
            {
                _logger.LogError($"A new company exception has been thrown: {icEx}");
                await HandleExceptionAsync(httpContext, icEx);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong: {ex}");
                await HandleExceptionAsync(httpContext, ex);
            }
        }

        public async Task HandleExceptionAsync(HttpContext httpContext, Exception exception)
        {
            httpContext.Response.ContentType = "application/json";
            var errorCode = (int)HttpStatusCode.InternalServerError;
            var msg = "";

            switch (exception)
            {
                case AccessViolationException:
                    msg = "Access violation error from the custom middleware";
                    break;
                case InvalidCompanyException:
                    msg = "Company model violation from the custom middleware";
                    break;
                default:
                    break;
            }
            
            var message = exception switch
            {
                AccessViolationException => "Access violation error from the custom middleware",
                _ => "Internal Server Error from the custom middleware"
            };

            httpContext.Response.StatusCode = errorCode;

            await httpContext.Response.WriteAsync(new ErrorDetails
            {
                StatusCode = httpContext.Response.StatusCode,
                Message = msg
            }.ToString());
        }        
    }

    public static class CustomExceptionMiddleware
    {
        public static void UseCustomException(this IApplicationBuilder app)
        {
            app.UseMiddleware<CustomExceptionExtension>();
        }
    }
}
