using Exam.Api.Data;
using Exam.Api.Infrastructure;
using Exam.Api.Reporting;
using Microsoft.EntityFrameworkCore;

namespace Exam.Api.Services;

public sealed class ExamReportService(
    ExamService exams,
    ExamDb db,
    EvidenceStorage storage,
    FinalReportDocument document,
    ReportGenerationGate gate)
{
    public async Task<GeneratedFinalReport> GenerateAsync(CancellationToken cancellationToken)
    {
        var exam = await exams.GetAsync(cancellationToken);

        if (!await gate.Semaphore.WaitAsync(0, cancellationToken))
            throw new ApiException(StatusCodes.Status429TooManyRequests,
                "Ya se está generando un informe. Espera unos segundos y vuelve a intentarlo.");

        try
        {
            var ids = exam.Requirements.SelectMany(x => x.Evidence).Select(x => x.Id).ToArray();
            var files = await db.Evidence.AsNoTracking().Where(x => ids.Contains(x.Id))
                .ToDictionaryAsync(x => x.Id, cancellationToken);
            var images = new Dictionary<int, byte[]>();
            foreach (var evidence in exam.Requirements.SelectMany(x => x.Evidence))
            {
                if (!files.TryGetValue(evidence.Id, out var file))
                    throw MissingEvidence(evidence.Title);
                try
                {
                    var stored = storage.GetImage(file.FileName, file.ContentType);
                    images.Add(evidence.Id, await File.ReadAllBytesAsync(stored.Path, cancellationToken));
                }
                catch (Exception exception) when (exception is IOException or UnauthorizedAccessException or ApiException)
                {
                    throw MissingEvidence(evidence.Title);
                }
            }

            var generatedAt = DateTimeOffset.UtcNow;
            var snapshot = new FinalReportSnapshot(exam, images, generatedAt);
            var bytes = document.Render(snapshot, cancellationToken);
            cancellationToken.ThrowIfCancellationRequested();
            var localTime = generatedAt.ToOffset(TimeSpan.FromHours(-5));
            var kind = snapshot.IsComplete ? "final" : "avance";
            return new GeneratedFinalReport(bytes, $"informe-{kind}-docker-{localTime:yyyyMMdd-HHmmss}.pdf");
        }
        finally
        {
            gate.Semaphore.Release();
        }
    }

    private static ApiException MissingEvidence(string title) => new(StatusCodes.Status409Conflict,
        $"No se puede incluir la evidencia «{title}». Comprueba que su imagen siga disponible y vuelve a generar el PDF.");
}
