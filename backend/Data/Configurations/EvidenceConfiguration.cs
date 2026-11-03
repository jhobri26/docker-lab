using Exam.Api.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Exam.Api.Data.Configurations;

public sealed class EvidenceConfiguration : IEntityTypeConfiguration<Evidence>
{
    public void Configure(EntityTypeBuilder<Evidence> builder)
    {
        builder.HasOne<RequirementProgress>().WithMany().HasForeignKey(x => x.RequirementId);
        builder.Property(x => x.Title).HasMaxLength(160);
        builder.Property(x => x.Description).HasColumnType("nvarchar(max)");
        builder.Property(x => x.DescriptionFormat).HasMaxLength(16).HasDefaultValue("markdown");
        builder.Property(x => x.FileName).HasMaxLength(100);
        builder.Property(x => x.ContentType).HasMaxLength(40);
    }
}
