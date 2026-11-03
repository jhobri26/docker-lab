using Exam.Api.Services;
using Exam.Api.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace Exam.Api.Controllers;

[ApiController]
[Route("api/evidence")]
public sealed class EvidenceController(EvidenceService service) : ControllerBase
{
    /// <summary>Obtiene el detalle completo de una evidencia.</summary>
    [HttpGet("{id:int}")]
    [ProducesResponseType<EvidenceResponse>(StatusCodes.Status200OK)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<EvidenceResponse>> Get(int id, CancellationToken cancellationToken) =>
        Ok(await service.GetAsync(id, cancellationToken));

    /// <summary>Edita el título y la descripción conservando la imagen original.</summary>
    /// <remarks>Admite descripción en Markdown o HTML enriquecido. El servidor limpia el HTML antes de guardarlo.</remarks>
    [HttpPut("{id:int}")]
    [ProducesResponseType<EvidenceResponse>(StatusCodes.Status200OK)]
    [ProducesResponseType<ValidationProblemDetails>(StatusCodes.Status400BadRequest)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<EvidenceResponse>> Update(int id, UpdateEvidenceRequest request, CancellationToken cancellationToken) =>
        Ok(await service.UpdateAsync(id, request, cancellationToken));

    /// <summary>Obtiene la imagen original de una evidencia.</summary>
    [HttpGet("{id:int}/image")]
    [ProducesResponseType(typeof(FileStreamResult), StatusCodes.Status200OK, "image/png", "image/jpeg", "image/webp")]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Image(int id, CancellationToken cancellationToken)
    {
        var file = await service.GetImageAsync(id, cancellationToken);
        return PhysicalFile(file.Path, file.ContentType);
    }

    /// <summary>Elimina una evidencia y su archivo asociado.</summary>
    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
    {
        await service.DeleteAsync(id, cancellationToken);
        return NoContent();
    }
}
