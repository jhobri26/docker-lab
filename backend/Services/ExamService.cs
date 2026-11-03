using Exam.Api.Contracts;
using Exam.Api.Data;
using Microsoft.EntityFrameworkCore;

namespace Exam.Api.Services;

public sealed class ExamService(ExamDb db, ExamCatalog catalog)
{
    public async Task<ExamResponse> GetAsync(CancellationToken cancellationToken)
    {
        var progress = await db.Progress.AsNoTracking().ToDictionaryAsync(x => x.Id, cancellationToken);
        var evidence = (await db.Evidence.AsNoTracking().ToListAsync(cancellationToken)).ToLookup(x => x.RequirementId);
        var profile = await db.Profiles.AsNoTracking().FirstAsync(cancellationToken);
        return new ExamResponse(profile.ToResponse(), catalog.Requirements.Select(requirement =>
            new RequirementResponse(
                requirement.Id, requirement.Title, requirement.Summary, requirement.Requirement,
                requirement.Checklist, requirement.Commands, requirement.EvidenceGuide, requirement.Note,
                2, progress[requirement.Id].ToResponse(),
                evidence[requirement.Id].OrderByDescending(x => x.Id).Select(x => x.ToResponse()).ToArray()))
            .ToArray());
    }
}
