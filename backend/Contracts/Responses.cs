using Exam.Api.Models;

namespace Exam.Api.Contracts;

public record ProfileResponse(int Id, string Student, string RepositoryUrl);
public record ProgressResponse(int Id, string Status, string Notes, DateTimeOffset UpdatedAt);
public record EvidenceResponse(int Id, string Title, string Description, string DescriptionFormat, long Size, DateTimeOffset UploadedAt, string Url);
public record EvidenceCreatedResponse(int Id);
public record ExamResponse(ProfileResponse Profile, IReadOnlyList<RequirementResponse> Requirements);
public record RequirementResponse(
    int Id, string Title, string Summary, string Requirement,
    string[] Checklist, CommandExample[] Commands, string[] EvidenceGuide, string Note,
    int Points, ProgressResponse Progress, IReadOnlyList<EvidenceResponse> Evidence);
public record HealthResponse(string Status, string Api, string Database, string Version);
public record ServicesResponse(bool Api, bool Database, bool Redis, long? Visits, string Version);

public static class ResponseMapping
{
    public static ProfileResponse ToResponse(this StudentProfile profile) =>
        new(profile.Id, profile.Student, profile.RepositoryUrl);
    public static ProgressResponse ToResponse(this RequirementProgress progress) =>
        new(progress.Id, progress.Status, progress.Notes, progress.UpdatedAt);
    public static EvidenceResponse ToResponse(this Evidence evidence) =>
        new(evidence.Id, evidence.Title, evidence.Description, evidence.DescriptionFormat, evidence.Size, evidence.UploadedAt,
            $"/api/evidence/{evidence.Id}/image");
}
