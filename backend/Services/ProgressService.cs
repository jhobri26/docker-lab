using Exam.Api.Contracts;
using Exam.Api.Data;
using Exam.Api.Infrastructure;

namespace Exam.Api.Services;

public sealed class ProgressService(ExamDb db)
{
    public async Task<ProgressResponse> UpdateAsync(int id, UpdateProgressRequest request, CancellationToken cancellationToken)
    {
        var progress = await db.Progress.FindAsync([id], cancellationToken)
            ?? throw ApiException.NotFound("El componente del examen no existe.");
        progress.Status = request.Status;
        progress.Notes = request.Notes;
        progress.UpdatedAt = DateTimeOffset.UtcNow;
        await db.SaveChangesAsync(cancellationToken);
        return progress.ToResponse();
    }
}
