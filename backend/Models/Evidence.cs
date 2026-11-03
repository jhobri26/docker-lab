namespace Exam.Api.Models;

public class Evidence
{
    public int Id { get; set; }
    public int RequirementId { get; set; }
    public string Title { get; set; } = "";
    public string Description { get; set; } = "";
    public string DescriptionFormat { get; set; } = "markdown";
    public string FileName { get; set; } = "";
    public string ContentType { get; set; } = "";
    public long Size { get; set; }
    public DateTimeOffset UploadedAt { get; set; } = DateTimeOffset.UtcNow;
}
