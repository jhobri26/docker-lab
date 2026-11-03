namespace Exam.Api.Models;

public class RequirementProgress
{
    public int Id { get; set; }
    public string Status { get; set; } = "pendiente";
    public string Notes { get; set; } = "";
    public DateTimeOffset UpdatedAt { get; set; } = DateTimeOffset.UtcNow;
}
