using Microsoft.EntityFrameworkCore;

namespace Exam.Api.Data;

public static class EvidenceSchemaUpgrade
{
    public static Task ApplyAsync(ExamDb db) => db.Database.ExecuteSqlRawAsync("""
        IF COL_LENGTH(N'dbo.Evidence', N'DescriptionFormat') IS NULL
        BEGIN
            ALTER TABLE [dbo].[Evidence] ADD [DescriptionFormat] nvarchar(16) NOT NULL
                CONSTRAINT [DF_Evidence_DescriptionFormat] DEFAULT N'markdown';
        END;
        IF COL_LENGTH(N'dbo.Evidence', N'Description') <> -1
        BEGIN
            ALTER TABLE [dbo].[Evidence] ALTER COLUMN [Description] nvarchar(max) NOT NULL;
        END;
        """);
}
