using System.Net.Sockets;
using System.Text;

namespace Exam.Api.Infrastructure;

public sealed class RedisVisitCounter(IConfiguration configuration)
{
    public async Task<long?> IncrementAsync(CancellationToken cancellationToken)
    {
        var host = configuration["Redis:Host"];
        if (string.IsNullOrWhiteSpace(host)) return null;
        try
        {
            using var timeout = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
            timeout.CancelAfter(TimeSpan.FromSeconds(2));
            using var client = new TcpClient();
            await client.ConnectAsync(host, 6379, timeout.Token);
            using var stream = client.GetStream();
            await stream.WriteAsync(Encoding.ASCII.GetBytes("*2\r\n$4\r\nINCR\r\n$16\r\ndockerexam:visits\r\n"), timeout.Token);
            using var reader = new StreamReader(stream);
            var line = await reader.ReadLineAsync(timeout.Token);
            if (line?.StartsWith(':') == true && long.TryParse(line[1..], out var count)) return count;
        }
        catch (Exception exception) when (exception is SocketException or IOException or OperationCanceledException) { }
        return null;
    }
}
