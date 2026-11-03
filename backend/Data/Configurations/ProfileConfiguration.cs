using Exam.Api.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Exam.Api.Data.Configurations;

public sealed class ProfileConfiguration : IEntityTypeConfiguration<StudentProfile>
{
    public void Configure(EntityTypeBuilder<StudentProfile> builder)
    {
        builder.Property(x => x.Id).ValueGeneratedNever();
        builder.Property(x => x.Student).HasMaxLength(160);
        builder.Property(x => x.RepositoryUrl).HasMaxLength(500);
    }
}
