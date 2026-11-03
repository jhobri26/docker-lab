using System.Text.Json;
using Exam.Api.Infrastructure;
using Exam.Api.Models;

namespace Exam.Api.Services;

public sealed class ExamCatalog
{
    private readonly string contentPath;
    public IReadOnlyList<ExamRequirement> Requirements { get; }

    public ExamCatalog(IWebHostEnvironment environment)
    {
        contentPath = Path.Combine(environment.ContentRootPath, "Content");
        Requirements = JsonSerializer.Deserialize<ExamRequirement[]>(
            File.ReadAllText(Path.Combine(contentPath, "exam.json")), new JsonSerializerOptions(JsonSerializerDefaults.Web))
            ?? throw new InvalidDataException("El catálogo del examen no es válido.");
        if (Requirements.Count == 0 || Requirements.Select(x => x.Id).Distinct().Count() != Requirements.Count)
            throw new InvalidDataException("El catálogo debe tener componentes con identificadores únicos.");
    }

    public DownloadFile GetResource(string name)
    {
        var resource = name switch
        {
            "rubric" => new DownloadFile(Path.Combine(contentPath, "rubric.pdf"), "application/pdf", "rubrica-examen.pdf"),
            "cheatsheet" => new DownloadFile(Path.Combine(contentPath, "cheatsheet.md"), "text/markdown; charset=utf-8", "docker-cheatsheet.md"),
            "guide" => new DownloadFile(Path.Combine(contentPath, "guide.md"), "text/markdown; charset=utf-8", "guia-sustentacion.md"),
            _ => throw ApiException.NotFound("El recurso solicitado no existe.")
        };
        if (!File.Exists(resource.Path)) throw ApiException.NotFound("El archivo solicitado no está disponible.");
        return resource;
    }
}
