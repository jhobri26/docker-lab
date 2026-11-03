using Exam.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace Exam.Api.Controllers;

[ApiController]
[Route("api/reports")]
public sealed class ReportsController(ExamReportService reports) : ControllerBase
{
    /// <summary>Descarga un PDF con todo el trabajo guardado, su avance, notas y evidencias.</summary>
    /// <remarks>Admite avances parciales. Incluye todos los casos con su estado real, capturas y descripciones HTML/Markdown. Se conserva la ruta existente final.pdf.</remarks>
    [HttpGet("final.pdf")]
    [ProducesResponseType(typeof(FileContentResult), StatusCodes.Status200OK, "application/pdf")]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status409Conflict)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status429TooManyRequests)]
    [ResponseCache(NoStore = true, Location = ResponseCacheLocation.None)]
    public async Task<IActionResult> Final(CancellationToken cancellationToken)
    {
        var report = await reports.GenerateAsync(cancellationToken);
        return File(report.Content, "application/pdf", report.FileName);
    }
}
