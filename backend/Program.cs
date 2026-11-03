using Exam.Api.Configuration;
using Exam.Api.Data;
using Exam.Api.Infrastructure;

if (args.Contains("--healthcheck"))
    return await ContainerHealthcheck.RunAsync();

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddExamApi(builder.Configuration);
builder.WebHost.ConfigureKestrel(options => options.Limits.MaxRequestBodySize = ApiLimits.RequestBytes);

var app = builder.Build();
app.UseExceptionHandler();
app.UseExamDocumentation();
await app.InitializeDatabaseAsync();
app.MapControllers();
await app.RunAsync();
return 0;
