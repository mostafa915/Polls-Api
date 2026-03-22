using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace SurveyBasket.Erorrs
{
    public class GlobalExpectionHandler(ILogger<GlobalExpectionHandler> logger) : IExceptionHandler
    {
        private readonly ILogger<GlobalExpectionHandler> _logger = logger;

        public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
        {
            _logger.LogError("Something Erorr Here: {Message}", exception.Message);
            var ProblemDetails = new ProblemDetails
            {
                Status = StatusCodes.Status500InternalServerError,
                Title = "InternalServerError",
                Type = "RFC 9110 – Section 15.6.1: 500 Internal Server Error"
            };
            httpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;
           await  httpContext.Response.WriteAsJsonAsync(ProblemDetails,cancellationToken);
            return true;
        }
    }
}
