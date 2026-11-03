using Exam.Api.Contracts;
using Exam.Api.Data;
using Exam.Api.Infrastructure;
using Exam.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace Exam.Api.Services;

public sealed class EvidenceService(ExamDb db, EvidenceStorage storage, EvidenceDescription descriptions)
{
    public async Task<EvidenceCreatedResponse> UploadAsync(int requirementId, UploadEvidenceRequest request, CancellationToken cancellationToken)
    {
        if (!await db.Progress.AnyAsync(x => x.Id == requirementId, cancellationToken))
            throw ApiException.NotFound("El componente del examen no existe.");

        var description = descriptions.Prepare(request.Description, request.DescriptionFormat);
        var stored = await storage.SaveAsync(request.File, cancellationToken);
        var evidence = new Evidence
        {
            RequirementId = requirementId,
            Title = request.Title.Trim(),
            Description = description,
            DescriptionFormat = request.DescriptionFormat,
            FileName = stored.FileName,
            ContentType = stored.ContentType,
            Size = stored.Size
        };
        try
        {
            db.Evidence.Add(evidence);
            await db.SaveChangesAsync(cancellationToken);
        }
        catch
        {
            storage.Delete(stored.FileName);
            throw;
        }
        return new EvidenceCreatedResponse(evidence.Id);
    }

    public async Task<DownloadFile> GetImageAsync(int id, CancellationToken cancellationToken)
    {
        var evidence = await db.Evidence.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id, cancellationToken)
            ?? throw ApiException.NotFound("La evidencia no existe.");
        return storage.GetImage(evidence.FileName, evidence.ContentType);
    }

    public async Task<EvidenceResponse> GetAsync(int id, CancellationToken cancellationToken)
    {
        var evidence = await db.Evidence.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id, cancellationToken)
            ?? throw ApiException.NotFound("La evidencia no existe.");
        return evidence.ToResponse();
    }

    public async Task<EvidenceResponse> UpdateAsync(int id, UpdateEvidenceRequest request, CancellationToken cancellationToken)
    {
        var evidence = await db.Evidence.FindAsync([id], cancellationToken)
            ?? throw ApiException.NotFound("La evidencia no existe.");
        evidence.Title = request.Title.Trim();
        evidence.Description = descriptions.Prepare(request.Description, request.DescriptionFormat);
        evidence.DescriptionFormat = request.DescriptionFormat;
        await db.SaveChangesAsync(cancellationToken);
        return evidence.ToResponse();
    }

    public async Task DeleteAsync(int id, CancellationToken cancellationToken)
    {
        var evidence = await db.Evidence.FindAsync([id], cancellationToken)
            ?? throw ApiException.NotFound("La evidencia no existe.");
        db.Evidence.Remove(evidence);
        await db.SaveChangesAsync(cancellationToken);
        storage.Delete(evidence.FileName);
    }
}
