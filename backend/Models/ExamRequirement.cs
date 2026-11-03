namespace Exam.Api.Models;

public record CommandExample(string Label, string Code);
public record ExamRequirement(
    int Id, string Title, string Summary, string Requirement,
    string[] Checklist, CommandExample[] Commands, string[] EvidenceGuide, string Note);
