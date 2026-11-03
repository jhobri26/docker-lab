using Exam.Api.Contracts;
using Exam.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace Exam.Api.Controllers;

[ApiController]
[Route("api/profile")]
public sealed class ProfileController(ProfileService service) : ControllerBase
{
    /// <summary>Actualiza el alumno y el enlace HTTPS del repositorio.</summary>
    [HttpPut]
    [ProducesResponseType<ProfileResponse>(StatusCodes.Status200OK)]
    [ProducesResponseType<ValidationProblemDetails>(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<ProfileResponse>> Update(UpdateProfileRequest request, CancellationToken cancellationToken) =>
        Ok(await service.UpdateAsync(request, cancellationToken));
}
