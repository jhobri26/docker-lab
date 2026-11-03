using Exam.Api.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Exam.Api.Data.Configurations;

public sealed class ProgressConfiguration : IEntityTypeConfiguration<RequirementProgress>
{
    public void Configure(EntityTypeBuilder<RequirementProgress> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).ValueGeneratedNever();
        builder.Property(x => x.Status).HasMaxLength(30);
        builder.Property(x => x.Notes).HasMaxLength(4000);
    }
}
