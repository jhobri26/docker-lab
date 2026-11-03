using Exam.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace Exam.Api.Data;

public class ExamDb(DbContextOptions<ExamDb> options) : DbContext(options)
{
    // Conservar los nombres mantiene la compatibilidad con la base existente.
    public DbSet<RequirementProgress> Progress => Set<RequirementProgress>();
    public DbSet<Evidence> Evidence => Set<Evidence>();
    public DbSet<StudentProfile> Profiles => Set<StudentProfile>();

    protected override void OnModelCreating(ModelBuilder modelBuilder) =>
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ExamDb).Assembly);
}
