using Exam.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace Exam.Api.Controllers;

[ApiController]
[Route("api/resources")]
public sealed class ResourcesController(ExamCatalog catalog) : ControllerBase
{
    /// <summary>Descarga la rúbrica, el Cheat Sheet o la guía de sustentación.</summary>
    /// <param name="name">Valores admitidos: rubric, cheatsheet, guide.</param>
    [HttpGet("{name}")]
    [ProducesResponseType(typeof(FileStreamResult), StatusCodes.Status200OK, "application/pdf", "text/markdown")]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status404NotFound)]
    public IActionResult Download(string name)
    {
        var file = catalog.GetResource(name);
        return PhysicalFile(file.Path, file.ContentType, file.DownloadName);
    }
}
