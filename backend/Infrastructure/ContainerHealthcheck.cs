namespace Exam.Api.Infrastructure;

public static class ContainerHealthcheck
{
    public static async Task<int> RunAsync()
    {
        try
        {
            using var client = new HttpClient { Timeout = TimeSpan.FromSeconds(7) };
            using var response = await client.GetAsync("http://127.0.0.1:8080/api/health");
            return response.IsSuccessStatusCode ? 0 : 1;
        }
        catch (Exception exception) when (exception is HttpRequestException or TaskCanceledException)
        {
            return 1;
        }
    }
}
