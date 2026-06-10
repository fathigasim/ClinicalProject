
using ClinicProjectApplication.Common.Exceptions;
using ClinicProjectApplication.Exceptions;
using ClinicProjectDomain.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Net;
using System.Text.Json;

namespace DefaultAuthenticationApi.Middleware
{
    #region Exception Handling Old
    // API/Middleware/ExceptionHandlingMiddleware.cs
    //public class ExceptionHandlingMiddleware
    //{
    //    private readonly RequestDelegate _next;
    //    private readonly ILogger<ExceptionHandlingMiddleware> _logger;

    //    public ExceptionHandlingMiddleware(
    //        RequestDelegate next,
    //        ILogger<ExceptionHandlingMiddleware> logger)
    //    {
    //        _next = next;
    //        _logger = logger;
    //    }

    //    public async Task InvokeAsync(HttpContext context)
    //    {
    //        try
    //        {
    //            await _next(context);
    //        }
    //        catch (ApiValidationException ex)
    //        {
    //            await WriteProblem(context, 400, "Validation failed", ex.Errors);
    //        }
    //        catch (UnauthorizedException ex)
    //        {
    //            await WriteProblem(context, 401, ex.Message);
    //        }
    //        catch (ForbiddenAccessException ex)
    //        {
    //            await WriteProblem(context, 403, ex.Message);
    //        }
    //        catch (NotFoundException ex)
    //        {
    //            await WriteProblem(context, 404, ex.Message);
    //        }
    //        catch (Exception ex)
    //        {
    //            _logger.LogError(ex, "Unhandled exception");
    //            await WriteProblem(context, 500, "An unexpected error occurred.");
    //        }
    //    }

    //    private static Task WriteProblem(
    //        HttpContext context, int status, string detail, object? errors = null)
    //    {
    //        context.Response.StatusCode = status;
    //        context.Response.ContentType = "application/problem+json";

    //        var problem = new
    //        {
    //            type = $"https://httpstatuses.com/{status}",
    //            title = ReasonPhrases.GetReasonPhrase(status),
    //            status,
    //            detail,
    //            errors
    //        };

    //        return context.Response.WriteAsJsonAsync(problem);
    //    }
    //} 
    #endregion

    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHandlingMiddleware> _logger;
      
        public ExceptionHandlingMiddleware(
            RequestDelegate next,
            ILogger<ExceptionHandlingMiddleware> logger
         
            )
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

        private async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            // ADD THIS - see exactly what type is coming in
            Console.WriteLine($"Exception type: {exception.GetType().FullName}");
            Console.WriteLine($"Inner exception type: {exception.InnerException?.GetType().FullName}");
            Console.WriteLine($"Inner inner: {exception.InnerException?.InnerException?.GetType().FullName}");
            _logger.LogError(exception, "An error occurred: {Message}", exception.Message);

            context.Response.ContentType = "application/json";

            object response;
            int statusCode = StatusCodes.Status500InternalServerError;

            //  Use fully qualified name to be 100% sure
            if (exception is FluentValidation.ValidationException validationException)
            {
                Console.WriteLine(" Matched FluentValidation.ValidationException");

                var modelState = new ModelStateDictionary();
                foreach (var error in validationException.Errors)
                {
                    Console.WriteLine($"   Adding: {error.PropertyName} = {error.ErrorMessage}");
                    modelState.AddModelError(
                        error.PropertyName ?? string.Empty,
                        error.ErrorMessage);
                }

                response = new ValidationProblemDetails(modelState)
                {
                    Status = StatusCodes.Status400BadRequest,
                    Title = "ValidationError",//_localizer["ValidationError"],
                    Type = "https://tools.ietf.org/html/rfc7231#section-6.5.1",

                };
                statusCode = StatusCodes.Status400BadRequest;
            }
            else if (exception is NotFoundException notFoundException)
            {
                response = new ProblemDetails
                {
                    Title = notFoundException.Message,
                    Status = StatusCodes.Status404NotFound
                };
                statusCode = StatusCodes.Status404NotFound;
            }
            
                 else if (exception is KeyNotFoundException keyNotFoundException)
            {
                response = new ProblemDetails
                {
                    Title = keyNotFoundException.Message,
                    Status = StatusCodes.Status404NotFound
                };
                statusCode = StatusCodes.Status404NotFound;
            }
            else if (exception is DbUpdateException dbUpdateException &&
          dbUpdateException.InnerException is SqlException sqlEx)
            {
                response = new ProblemDetails
                {
                    Title = "Database conflict",
                    Status = StatusCodes.Status409Conflict,
                    Detail = sqlEx.Number switch
                    {
                        2601 or 2627 => "Duplicate key — a record with this value already exists.",
                        547 => "Foreign key violation — related record not found.",
                        _ => sqlEx.Message
                    }
                };
                statusCode = StatusCodes.Status409Conflict;
                _logger.LogError("SQL Conflict Exception error {Error}", sqlEx.Message);
            }
            else if (exception is SqlException sqlException)  // keep this for direct SQL exceptions
            {
                response = new ProblemDetails
                {
                    Title = sqlException.Message,
                    Status = StatusCodes.Status409Conflict,
                    Detail = "Error duplicate key found"
                };
                statusCode = StatusCodes.Status409Conflict;
            }
            else if (exception is UnauthorizedAccessException)
            {
                response = new ProblemDetails
                {
                    Title = exception.Message,
                    Status = StatusCodes.Status401Unauthorized
                };
                statusCode = StatusCodes.Status401Unauthorized;
            }
            else if (exception is CashLimitExceededException apiException)
            {
                response = new ProblemDetails
                {
                    Title = apiException.Message,
                    Status = StatusCodes.Status409Conflict,
                    Detail=" Invalid Operation"
                };
              
                
                statusCode = StatusCodes.Status409Conflict;
                _logger.LogError("Conflict Exception error {Error}", apiException.Message);
            }

            else if (exception is HttpRequestException httpException)
            {
                bool isNetworkDown = httpException.InnerException is System.Net.Sockets.SocketException;
                // If StatusCode is null, it's usually a network/DNS failure (504)
                // If it has a value, the external service (GOSI/Yakeen) returned an error (502)
                statusCode = httpException.StatusCode switch
                {
                    HttpStatusCode.Unauthorized => StatusCodes.Status401Unauthorized,
                    HttpStatusCode.Forbidden => StatusCodes.Status403Forbidden,
                    HttpStatusCode.NotFound => StatusCodes.Status404NotFound,
                    null => isNetworkDown ? StatusCodes.Status503ServiceUnavailable : StatusCodes.Status504GatewayTimeout,
                    _ => StatusCodes.Status502BadGateway
                };

                response = new ProblemDetails
                {
                    Status = statusCode,
                    Title = statusCode switch
                    {
                        StatusCodes.Status503ServiceUnavailable => "ExternalServiceUnavailable",
                        StatusCodes.Status504GatewayTimeout => "ExternalServiceTimeout",
                        _ => "ExternalServiceError"
                    },
                    Detail = "The request could not be completed due to an external connectivity issue."
                };
                _logger.LogError("Exception Middleware Error: {Message}", httpException.Message);
            }
            else if (exception is InvalidWebhookSignatureException webhookException)
            {
                response = new ProblemDetails
                {
                    Title = "Invalid Webhook Signature",
                    Status = StatusCodes.Status400BadRequest,
                    Detail = webhookException.Message
                };
                statusCode = StatusCodes.Status400BadRequest;
                _logger.LogError("Exception Middleware Webhook Error: {Message}", webhookException.Message);
            }
            else
            {
                Console.WriteLine($" Unhandled: {exception.GetType().FullName}");

                response = new ProblemDetails
                {
                    Title = "InternalServerError",//_localizer["InternalServerError"],
                    Status = StatusCodes.Status500InternalServerError,
                    Detail="Internal Server Error"
                };
                statusCode = StatusCodes.Status500InternalServerError;
            }

            context.Response.StatusCode = statusCode;

            var json = JsonSerializer.Serialize(response, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            });

            await context.Response.WriteAsync(json);
        }

        private static T? GetInnerException<T>(Exception exception) where T : Exception
        {
            var current = exception;
            while (current != null)
            {
                if (current is T match) return match;
                current = current.InnerException;
            }
            return null;
        }
    }
}
