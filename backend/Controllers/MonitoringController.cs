using Exam.Api.Contracts;
using Exam.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace Exam.Api.Controllers;

[ApiController]
[Route("api")]
public sealed class MonitoringController(MonitoringService service) : ControllerBase
{
    /// <summary>Comprueba que la API puede conectarse a SQL Server.</summary>
    [HttpGet("health")]
    [ProducesResponseType<HealthResponse>(StatusCodes.Status200OK)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status503ServiceUnavailable)]
    public async Task<ActionResult<HealthResponse>> Health(CancellationToken cancellationToken) =>
        Ok(await service.GetHealthAsync(cancellationToken));

    /// <summary>Consulta SQL y Redis e incrementa el contador de consultas.</summary>
    [HttpGet("services")]
    [ProducesResponseType<ServicesResponse>(StatusCodes.Status200OK)]
    public async Task<ActionResult<ServicesResponse>> Services(CancellationToken cancellationToken) =>
        Ok(await service.GetServicesAsync(cancellationToken));
}
