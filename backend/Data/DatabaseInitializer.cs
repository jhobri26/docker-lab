using Exam.Api.Models;
using Exam.Api.Services;
using Exam.Api.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace Exam.Api.Data;

public static class DatabaseInitializer
{
    public static async Task InitializeDatabaseAsync(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<ExamDb>();
        var catalog = scope.ServiceProvider.GetRequiredService<ExamCatalog>();
        await db.Database.EnsureCreatedAsync();
        await EvidenceSchemaUpgrade.ApplyAsync(db);
        var existing = await db.Progress.Select(x => x.Id).ToListAsync();
        db.Progress.AddRange(catalog.Requirements.Where(x => !existing.Contains(x.Id))
            .Select(x => new RequirementProgress { Id = x.Id }));
        if (!await db.Profiles.AnyAsync()) db.Profiles.Add(new StudentProfile());
        await db.SaveChangesAsync();
        _ = scope.ServiceProvider.GetRequiredService<EvidenceStorage>();
    }
}
