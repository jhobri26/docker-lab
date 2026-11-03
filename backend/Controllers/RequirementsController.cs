using Exam.Api.Configuration;
using Exam.Api.Contracts;
using Exam.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace Exam.Api.Controllers;

[ApiController]
[Route("api/requirements")]
public sealed class RequirementsController(ProgressService progress, EvidenceService evidence) : ControllerBase
{
    /// <summary>Guarda el estado y las notas de un componente.</summary>
    /// <remarks>Estados: pendiente, en-progreso o listo. El estado es una autoevaluación.</remarks>
    [HttpPut("{id:int}")]
    [ProducesResponseType<ProgressResponse>(StatusCodes.Status200OK)]
    [ProducesResponseType<ValidationProblemDetails>(StatusCodes.Status400BadRequest)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ProgressResponse>> Update(int id, UpdateProgressRequest request, CancellationToken cancellationToken) =>
        Ok(await progress.UpdateAsync(id, request, cancellationToken));

    /// <summary>Sube una captura PNG, JPG o WebP de hasta 8 MB.</summary>
    /// <remarks>Seleccionar un archivo y completar título y descripción opcional. Se guardan los metadatos en SQL y la imagen en un volumen.</remarks>
    [HttpPost("{id:int}/evidence")]
    [Consumes("multipart/form-data")]
    [RequestSizeLimit(ApiLimits.RequestBytes)]
    [ProducesResponseType<EvidenceCreatedResponse>(StatusCodes.Status201Created)]
    [ProducesResponseType<ValidationProblemDetails>(StatusCodes.Status400BadRequest)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status404NotFound)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status413PayloadTooLarge)]
    public async Task<ActionResult<EvidenceCreatedResponse>> Upload(int id, [FromForm] UploadEvidenceRequest request, CancellationToken cancellationToken)
    {
        var result = await evidence.UploadAsync(id, request, cancellationToken);
        return Created($"/api/evidence/{result.Id}/image", result);
    }
}
