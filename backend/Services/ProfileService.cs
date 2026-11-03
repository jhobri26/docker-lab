using Exam.Api.Contracts;
using Exam.Api.Data;
using Exam.Api.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace Exam.Api.Services;

public sealed class ProfileService(ExamDb db)
{
    public async Task<ProfileResponse> UpdateAsync(UpdateProfileRequest request, CancellationToken cancellationToken)
    {
        var repositoryUrl = request.RepositoryUrl.Trim();
        if (repositoryUrl.Length > 0 &&
            (!Uri.TryCreate(repositoryUrl, UriKind.Absolute, out var uri) || uri.Scheme != Uri.UriSchemeHttps))
            throw ApiException.Invalid("El repositorio debe ser una URL HTTPS.");

        var profile = await db.Profiles.FirstAsync(cancellationToken);
        profile.Student = request.Student.Trim();
        profile.RepositoryUrl = repositoryUrl;
        await db.SaveChangesAsync(cancellationToken);
        return profile.ToResponse();
    }
}
