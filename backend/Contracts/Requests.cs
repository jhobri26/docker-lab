using System.ComponentModel.DataAnnotations;
using Exam.Api.Configuration;

namespace Exam.Api.Contracts;

public sealed class UpdateProgressRequest
{
    /// <example>en-progreso</example>
    [Required, RegularExpression("^(pendiente|en-progreso|listo)$", ErrorMessage = "El estado debe ser pendiente, en-progreso o listo.")]
    public string Status { get; init; } = "";

    [Required(AllowEmptyStrings = true), StringLength(4000)]
    public string Notes { get; init; } = "";
}

public sealed class UpdateProfileRequest
{
    [Required, StringLength(160)]
    public string Student { get; init; } = "";

    [Required(AllowEmptyStrings = true), StringLength(500)]
    public string RepositoryUrl { get; init; } = "";
}

public sealed class UploadEvidenceRequest
{
    [Required]
    public IFormFile File { get; init; } = default!;
    [Required, StringLength(160)]
    public string Title { get; init; } = "";
    [StringLength(ApiLimits.DescriptionCharacters)]
    public string? Description { get; init; }
    [Required, RegularExpression("^(markdown|html)$")]
    public string DescriptionFormat { get; init; } = "markdown";
}

public sealed class UpdateEvidenceRequest
{
    [Required, StringLength(160)]
    public string Title { get; init; } = "";
    [StringLength(ApiLimits.DescriptionCharacters)]
    public string? Description { get; init; }
    [Required, RegularExpression("^(markdown|html)$")]
    public string DescriptionFormat { get; init; } = "markdown";
}
