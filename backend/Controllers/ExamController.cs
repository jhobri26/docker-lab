using Exam.Api.Contracts;
using Exam.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace Exam.Api.Controllers;

[ApiController]
[Route("api/exam")]
public sealed class ExamController(ExamService service) : ControllerBase
{
    /// <summary>Obtiene el perfil, los diez componentes, sus avances y evidencias.</summary>
    [HttpGet]
    [ProducesResponseType<ExamResponse>(StatusCodes.Status200OK)]
    public async Task<ActionResult<ExamResponse>> Get(CancellationToken cancellationToken) =>
        Ok(await service.GetAsync(cancellationToken));
}
