using Exam.Api.Configuration;
using Exam.Api.Contracts;
using Exam.Api.Data;
using Exam.Api.Infrastructure;

namespace Exam.Api.Services;

public sealed class MonitoringService(ExamDb db, RedisVisitCounter visits)
{
    public async Task<HealthResponse> GetHealthAsync(CancellationToken cancellationToken)
    {
        if (!await db.Database.CanConnectAsync(cancellationToken))
            throw new ApiException(StatusCodes.Status503ServiceUnavailable, "SQL Server no disponible.");
        return new HealthResponse("ok", ".NET 10", "SQL Server 2022", AppInfo.Version);
    }

    public async Task<ServicesResponse> GetServicesAsync(CancellationToken cancellationToken)
    {
        var database = await db.Database.CanConnectAsync(cancellationToken);
        var count = await visits.IncrementAsync(cancellationToken);
        return new ServicesResponse(true, database, count.HasValue, count, AppInfo.Version);
    }
}
