using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace Exam.Api.Infrastructure;

public sealed class ApiExceptionHandler(IProblemDetailsService problems, ILogger<ApiExceptionHandler> logger) : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(HttpContext context, Exception exception, CancellationToken cancellationToken)
    {
        var status = exception switch
        {
            ApiException api => api.StatusCode,
            BadHttpRequestException request => request.StatusCode,
            _ => StatusCodes.Status500InternalServerError
        };
        var message = status < 500 ? exception.Message : "Ocurrió un error al procesar la operación.";
        if (status >= 500) logger.LogError(exception, "Error en {Method} {Path}", context.Request.Method, context.Request.Path);
        context.Response.StatusCode = status;
        return await problems.TryWriteAsync(new ProblemDetailsContext
        {
            HttpContext = context,
            ProblemDetails = new ProblemDetails
            {
                Status = status,
                Title = message,
                Detail = message,
                Instance = context.Request.Path,
                Extensions = { ["message"] = message }
            }
        });
    }
}
