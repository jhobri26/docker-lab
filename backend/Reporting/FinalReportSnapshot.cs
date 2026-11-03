using Exam.Api.Contracts;

namespace Exam.Api.Reporting;

public sealed record FinalReportSnapshot(
    ExamResponse Exam,
    IReadOnlyDictionary<int, byte[]> Images,
    DateTimeOffset GeneratedAt)
{
    public int TotalCases => Exam.Requirements.Count;
    public int ReadyCases => Exam.Requirements.Count(item => item.Progress.Status == "listo");
    public int Completion => TotalCases == 0 ? 0 : (int)Math.Round(100d * ReadyCases / TotalCases, MidpointRounding.AwayFromZero);
    public bool IsComplete => TotalCases == 10 && ReadyCases == 10;
}

public sealed record GeneratedFinalReport(byte[] Content, string FileName);

// Una generación por proceso evita que varias descargas simultáneas multipliquen el uso de memoria.
public sealed class ReportGenerationGate : IDisposable
{
    public SemaphoreSlim Semaphore { get; } = new(1, 1);
    public void Dispose() => Semaphore.Dispose();
}
