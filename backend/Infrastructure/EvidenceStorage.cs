using System.Text;
using Exam.Api.Configuration;

namespace Exam.Api.Infrastructure;

public record DownloadFile(string Path, string ContentType, string? DownloadName = null);
public record StoredEvidence(string FileName, string ContentType, long Size);

public sealed class EvidenceStorage
{
    private readonly string root;
    private readonly ILogger<EvidenceStorage> logger;

    public EvidenceStorage(IConfiguration configuration, ILogger<EvidenceStorage> logger)
    {
        root = Path.GetFullPath(configuration["EvidencePath"] ?? "../tmp/evidence");
        this.logger = logger;
        Directory.CreateDirectory(root);
    }

    public async Task<StoredEvidence> SaveAsync(IFormFile file, CancellationToken cancellationToken)
    {
        if (file.Length is <= 0 or > ApiLimits.ImageBytes)
            throw ApiException.Invalid("La imagen debe pesar entre 1 byte y 8 MB.");

        using var buffer = new MemoryStream();
        await file.CopyToAsync(buffer, cancellationToken);
        var bytes = buffer.ToArray();
        var (extension, mime) = DetectType(bytes);
        if (extension is null) throw ApiException.Invalid("Solo se admiten imágenes PNG, JPG o WebP válidas.");
        var fileName = Guid.NewGuid().ToString("N") + extension;
        try { await File.WriteAllBytesAsync(Path.Combine(root, fileName), bytes, cancellationToken); }
        catch { Delete(fileName); throw; }
        return new StoredEvidence(fileName, mime!, bytes.Length);
    }

    public DownloadFile GetImage(string fileName, string contentType)
    {
        var path = Path.Combine(root, fileName);
        if (!File.Exists(path)) throw ApiException.NotFound("La imagen no está disponible.");
        return new DownloadFile(path, contentType);
    }

    public void Delete(string fileName)
    {
        try { File.Delete(Path.Combine(root, fileName)); }
        catch (Exception exception) when (exception is IOException or UnauthorizedAccessException)
        {
            logger.LogWarning(exception, "No se pudo retirar el archivo de evidencia {FileName}", fileName);
        }
    }

    private static (string?, string?) DetectType(byte[] bytes)
    {
        if (bytes.Length > 24 && bytes.AsSpan(0, 8).SequenceEqual(new byte[] { 137, 80, 78, 71, 13, 10, 26, 10 }) && Encoding.ASCII.GetString(bytes, 12, 4) == "IHDR") return (".png", "image/png");
        if (bytes.Length > 4 && bytes[0] == 255 && bytes[1] == 216 && bytes[2] == 255 && bytes[^2] == 255 && bytes[^1] == 217) return (".jpg", "image/jpeg");
        if (bytes.Length > 16 && Encoding.ASCII.GetString(bytes, 0, 4) == "RIFF" && Encoding.ASCII.GetString(bytes, 8, 4) == "WEBP") return (".webp", "image/webp");
        return (null, null);
    }
}
