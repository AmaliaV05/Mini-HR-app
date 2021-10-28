using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
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
            catch (InvalidCompanyException icEx)
            {
                _logger.LogError($"Adding a new valid company exception has been thrown: {icEx}");
                await HandleExceptionAsync(httpContext, icEx);
            }
            catch (GetCompanyException gcEx)
            {
                _logger.LogError($"Getting companies list/details exception has been thrown: { gcEx}");
                await HandleExceptionAsync(httpContext, gcEx);
            }
            catch (PutCompanyException pcEx)
            {
                _logger.LogError($"Modifying a company's details exception has been thrown: {pcEx}");
                await HandleExceptionAsync(httpContext, pcEx);
            }
            catch (PostCompanyException pcEx)
            {
                _logger.LogError($"Creating a new company exception has been thrown: {pcEx}");
                await HandleExceptionAsync(httpContext, pcEx);
            }
            catch (InvalidEmployeeException ieEx)
            {
                _logger.LogError($"Adding a new valid employee exception has been thrown: {ieEx}");
                await HandleExceptionAsync(httpContext, ieEx);
            }
            catch (GetEmployeeException geEx)
            {
                _logger.LogError($"Getting employees list/details exception has been thrown: { geEx}");
                await HandleExceptionAsync(httpContext, geEx);
            }
            catch (PutEmployeeException peEx)
            {
                _logger.LogError($"Modifying an employee's details exception has been thrown: {peEx}");
                await HandleExceptionAsync(httpContext, peEx);
            }
            catch (PostEmployeeException peEx)
            {
                _logger.LogError($"Creating a new employee exception has been thrown: {peEx}");
                await HandleExceptionAsync(httpContext, peEx);
            }
            catch (DbUpdateConcurrencyException ducEx)
            {
                _logger.LogError($"A database update concurrency exception has been thrown: {ducEx}");
                await HandleExceptionAsync(httpContext, ducEx);
            }
            catch (ArgumentNullException anEx)
            {
                _logger.LogError($"A argument null exception has been thrown: {anEx}");
                await HandleExceptionAsync(httpContext, anEx);
            }
            catch (AccessViolationException avEx)
            {
                _logger.LogError($"A new violation exception has been thrown: {avEx}");
                await HandleExceptionAsync(httpContext, avEx);
            }            
            catch (NullReferenceException nrEx)
            {
                _logger.LogError($"A null reference exception has been thrown: {nrEx}");
                await HandleExceptionAsync(httpContext, nrEx);
            }
            catch (InvalidOperationException ioEx)
            {
                _logger.LogError($"An invalid operation exception has been thrown: {ioEx}");
                await HandleExceptionAsync(httpContext, ioEx);
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
                case InvalidCompanyException:
                    msg = "Company model violation from the custom middleware";
                    break;
                case GetCompanyException:
                    msg = "Company listing/detailed showing violation from the custom middleware";
                    break;
                case PutCompanyException:
                    msg = "Company details' modification violation from the custom middleware";
                    break;
                case PostCompanyException:
                    msg = "Company creation violation from the custom middleware";
                    break;
                case InvalidEmployeeException:
                    msg = "Employee model violation from the custom middleware";
                    break;
                case GetEmployeeException:
                    msg = "Employee listing/detailed showing violation from the custom middleware";
                    break;
                case PutEmployeeException:
                    msg = "Employee details' modification violation from the custom middleware";
                    break;
                case PostEmployeeException:
                    msg = "Employee creation violation from the custom middleware";
                    break;
                case DbUpdateConcurrencyException:
                    msg = "Database update concurrency violation from the custom middleware";
                    break;
                case ArgumentNullException:
                    msg = "Argument violation from the custom middleware";
                    break;
                case AccessViolationException:
                    msg = "Access violation error from the custom middleware";
                    break;
                case NullReferenceException:
                    msg = "Reference violation from the custom middleware";
                    break;
                case InvalidOperationException:
                    msg = "Operation violation from the custom middleware";
                    break;
                default:
                    break;
            }

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
