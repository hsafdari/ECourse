using ECourse.Services.Course.Application.Common.Exceptions;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using FluentValidation;
using ECourse.Services.Course.Domain.Exceptions;

namespace ECourse.Services.Course.API.ExceptionHandling
{
    public sealed class GlobalExceptionHandler : IExceptionHandler
    {
        private readonly ILogger<GlobalExceptionHandler> _logger;
        private readonly IProblemDetailsService _problemDetailsService;

        public GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger, IProblemDetailsService problemDetailsService)
        {
            _logger = logger;
            _problemDetailsService = problemDetailsService;
        }
        public async ValueTask<bool> TryHandleAsync(
            HttpContext httpContext,
            Exception exception,
            CancellationToken cancellationToken)
        {
            ProblemDetails problemDetails = CreateProblemDetails(httpContext, exception);

            if (problemDetails.Status >= StatusCodes.Status500InternalServerError)
            {
                _logger.LogError(exception, "An unhandled exception occurred. Method: {Method}, Path: {Path} , Trace identifier: {TraceIdentifier}", httpContext.Request.Method, httpContext.Request.Path, httpContext.TraceIdentifier);
            }
            else
            {
                _logger.LogWarning(exception, "Request Failed with status {StatusCode}. Method: {Method}, Path: {Path} , Trace identifier: {TraceIdentifier}", problemDetails.Status, httpContext.Request.Method, httpContext.Request.Path, httpContext.TraceIdentifier);
            }
            httpContext.Response.StatusCode = problemDetails.Status ?? StatusCodes.Status500InternalServerError;
            bool wasWritten =
                await _problemDetailsService.TryWriteAsync(
                    new ProblemDetailsContext
                    {
                        HttpContext = httpContext,
                        ProblemDetails = problemDetails
                    });
            if (!wasWritten)
            {
                await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken);
            }
            return true;
        }

        private static ProblemDetails CreateProblemDetails(HttpContext httpContext, Exception exception)
        {
            return exception switch
            {
                ValidationException validationException => CreateValidationProblem(httpContext, validationException),
                ConflictException conflictException => CreateProblem(httpContext, StatusCodes.Status409Conflict, "Conflict occurred.", conflictException.Message),
                NotFoundException notFoundException => CreateProblem(httpContext, StatusCodes.Status404NotFound, "Resource not found.", notFoundException.Message),
                DomainException domainException => CreateProblem(httpContext, StatusCodes.Status400BadRequest, "Domain rules violation", domainException.Message),
                _ => CreateProblem
                (
                    httpContext,
                    StatusCodes.Status500InternalServerError,
                    "Internal server error",
                    "An unexpected error occurred."
                )
            };
        }
        private static HttpValidationProblemDetails CreateValidationProblem(HttpContext httpContext, ValidationException exception)
        {
            Dictionary<string, string[]> errors = exception.Errors
                .GroupBy(e => e.PropertyName)
                .ToDictionary(group => group.Key, group => group.Select(error => error.ErrorMessage)
                .Distinct().ToArray());
            return new HttpValidationProblemDetails(errors)
            {
                Status = StatusCodes.Status400BadRequest,
                Title = "Validation failed.",
                Detail = "One or more validation errors occurred.",
                Instance = httpContext.Request.Path
            };

        }
        private static ProblemDetails CreateProblem(
        HttpContext httpContext,
        int status,
        string title,
        string detail)
        {
            return new ProblemDetails
            {
                Status = status,
                Title = title,
                Detail = detail,
                Instance = httpContext.Request.Path
            };
        }
    }
}
