using Exam.Api.Data;
using Exam.Api.Infrastructure;
using Exam.Api.Reporting;
using Exam.Api.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi;

namespace Exam.Api.Configuration;

public static class ServiceRegistration
{
    public static IServiceCollection AddExamApi(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddControllers().ConfigureApiBehaviorOptions(options =>
        {
            options.InvalidModelStateResponseFactory = context =>
            {
                var problem = new ValidationProblemDetails(context.ModelState)
                {
                    Status = StatusCodes.Status400BadRequest,
                    Title = "Revisa los datos enviados.",
                    Instance = context.HttpContext.Request.Path
                };
                problem.Extensions["message"] = string.Join(" ", problem.Errors.Values.SelectMany(x => x));
                return new BadRequestObjectResult(problem);
            };
        });
        services.AddProblemDetails();
        services.AddExceptionHandler<ApiExceptionHandler>();
        var connectionString = BuildConnectionString(configuration);
        services.AddDbContext<ExamDb>(options => options.UseSqlServer(
            connectionString, sql => sql.EnableRetryOnFailure()));
        services.AddSingleton<ExamCatalog>();
        services.AddSingleton<EvidenceStorage>();
        services.AddSingleton<EvidenceDescription>();
        services.AddSingleton<RedisVisitCounter>();
        services.AddScoped<ExamService>();
        services.AddScoped<ProgressService>();
        services.AddScoped<ProfileService>();
        services.AddScoped<EvidenceService>();
        services.AddScoped<MonitoringService>();
        // Uso educativo de QuestPDF: licencia Community para aprendizaje/entrenamiento.
        QuestPDF.Settings.License = QuestPDF.Infrastructure.LicenseType.Community;
        services.AddSingleton<ReportGenerationGate>();
        services.AddSingleton<ReportRichText>();
        services.AddSingleton<FinalReportDocument>();
        services.AddScoped<ExamReportService>();
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "Docker Lab API",
                Version = AppInfo.Version,
                Description = "API del examen final: perfil, diez componentes, avances, evidencias y estado del stack."
            });
            options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, "Exam.Api.xml"));
            options.SupportNonNullableReferenceTypes();
        });
        return services;
    }

    public static WebApplication UseExamDocumentation(this WebApplication app)
    {
        app.UseSwagger(options => options.RouteTemplate = "api/swagger/{documentName}/swagger.json");
        app.UseSwaggerUI(options =>
        {
            options.RoutePrefix = "api/swagger";
            options.SwaggerEndpoint("v1/swagger.json", "Docker Lab API v1");
            options.DocumentTitle = "Docker Lab · Swagger";
            options.DisplayRequestDuration();
        });
        return app;
    }

    private static string BuildConnectionString(IConfiguration configuration)
    {
        var passwordFile = configuration["Database:PasswordFile"] ?? "../secrets/db_password.txt";
        return new SqlConnectionStringBuilder
        {
            DataSource = configuration["Database:Server"] ?? "localhost,14330",
            InitialCatalog = configuration["Database:Name"] ?? "DockerExam",
            UserID = "sa",
            Password = File.ReadAllText(passwordFile).Trim(),
            TrustServerCertificate = true,
            Encrypt = SqlConnectionEncryptOption.Mandatory
        }.ConnectionString;
    }
}
