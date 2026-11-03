using System.Globalization;
using Exam.Api.Infrastructure;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace Exam.Api.Reporting;

public sealed class FinalReportDocument(ReportRichText richText)
{
    private const string Blue = "#163C70";
    private static readonly CultureInfo Spanish = CultureInfo.GetCultureInfo("es-PE");

    public byte[] Render(FinalReportSnapshot snapshot, CancellationToken cancellationToken = default)
    {
        var landscapeImages = new Dictionary<int, bool>();
        // Validar cada imagen antes de componer evita entregar un PDF con capturas omitidas.
        foreach (var evidence in snapshot.Exam.Requirements.SelectMany(x => x.Evidence))
        {
            cancellationToken.ThrowIfCancellationRequested();
            try
            {
                if (!snapshot.Images.TryGetValue(evidence.Id, out var bytes)) throw new InvalidDataException();
                using var image = QuestPDF.Infrastructure.Image.FromBinaryData(bytes);
                landscapeImages[evidence.Id] = image.Size.Width >= image.Size.Height;
            }
            catch (Exception exception) when (exception is not OperationCanceledException)
            {
                throw new ApiException(409, $"La imagen de la evidencia «{evidence.Title}» no se puede leer. Vuelve a cargarla antes de generar el PDF.");
            }
        }

        return Document.Create(document =>
        {
            document.Page(page =>
            {
                Configure(page);
                page.Content().Column(column =>
                {
                    column.Spacing(14);
                    column.Item().PaddingTop(28).Text(snapshot.IsComplete ? "INFORME FINAL" : "INFORME DE AVANCE").FontSize(13).FontColor(Blue).Bold();
                    column.Item().Text("Docker & Docker Compose\nTrabajo realizado").FontSize(28).Bold().FontColor(Blue);
                    column.Item().Text(snapshot.Exam.Profile.Student).FontSize(18).Bold();
                    column.Item().Text("Curso CURS-000662");
                    if (!string.IsNullOrWhiteSpace(snapshot.Exam.Profile.RepositoryUrl))
                        column.Item().Text(text =>
                        {
                            text.Span("Repositorio: ");
                            var repository = snapshot.Exam.Profile.RepositoryUrl;
                            if (Uri.TryCreate(repository, UriKind.Absolute, out var uri) && uri.Scheme is "https" or "http")
                                text.Hyperlink(repository, repository).FontColor(Blue);
                            else text.Span(repository);
                        });
                    column.Item().Text($"Generado: {LocalDate(snapshot.GeneratedAt)} (Lima)").FontSize(9);
                    column.Item().Background(snapshot.IsComplete ? "#EAF4EF" : "#EEF3FA").Padding(14).Text(
                        $"{snapshot.Completion} % de avance declarado · {snapshot.ReadyCases} de {snapshot.TotalCases} casos listos para revisión\n{snapshot.Images.Count} evidencias adjuntas").Bold();
                    column.Item().Text("El estado corresponde al avance registrado por el alumno. La evaluación y la calificación corresponden al docente.")
                        .FontSize(9).FontColor("#55647A");
                    column.Item().Text("Contenido").FontSize(15).Bold().FontColor(Blue);
                    foreach (var item in snapshot.Exam.Requirements.OrderBy(x => x.Id))
                        column.Item().BorderBottom(1).BorderColor("#E1E7EF").PaddingBottom(5).Row(row =>
                        {
                            row.RelativeItem().Text($"{item.Id:00}. {item.Title}");
                            row.ConstantItem(90).AlignRight().Text(item.Evidence.Count == 1 ? "1 evidencia" : $"{item.Evidence.Count} evidencias").FontSize(9);
                        });
                });
            });

            foreach (var item in snapshot.Exam.Requirements.OrderBy(x => x.Id))
            {
                cancellationToken.ThrowIfCancellationRequested();
                document.Page(page =>
                {
                    Configure(page);
                    page.Content().Column(column =>
                    {
                        column.Spacing(12);
                        column.Item().Text($"CASO {item.Id:00}").FontColor(Blue).Bold();
                        column.Item().Text(item.Title).FontSize(23).Bold().FontColor(Blue);
                        column.Item().Text(StatusLabel(item.Progress.Status) + " · Avance guardado: " + LocalDate(item.Progress.UpdatedAt)).FontSize(9);
                        column.Item().Text("Lo solicitado").FontSize(13).Bold();
                        column.Item().Text(item.Requirement);
                        column.Item().Text("Desarrollo y resultados").FontSize(13).Bold();
                        column.Item().Text(string.IsNullOrWhiteSpace(item.Progress.Notes)
                            ? "No se registraron notas de desarrollo para este caso." : item.Progress.Notes);
                        column.Item().Text(item.Evidence.Count > 0
                            ? $"Evidencias adjuntas: {item.Evidence.Count}. Las capturas y sus descripciones se presentan a continuación."
                            : "No se adjuntaron evidencias para este caso.").FontSize(10).FontColor("#55647A");
                    });
                });

                foreach (var evidence in item.Evidence.OrderBy(x => x.Id))
                    document.Page(page =>
                    {
                        var landscape = landscapeImages[evidence.Id];
                        Configure(page, landscape);
                        page.Content().Column(column =>
                        {
                            column.Spacing(10);
                            column.Item().Text($"CASO {item.Id:00} · EVIDENCIA {evidence.Id}").FontColor(Blue).Bold();
                            column.Item().Text(evidence.Title).FontSize(17).Bold();
                            column.Item().Text($"Cargada: {LocalDate(evidence.UploadedAt)} (Lima)").FontSize(8).FontColor("#55647A");
                            column.Item().Height(landscape ? 300 : 440).AlignCenter().Image(snapshot.Images[evidence.Id]).FitArea().UseOriginalImage();
                            column.Item().Text("Descripción de la evidencia").Bold().FontColor(Blue);
                            if (string.IsNullOrWhiteSpace(evidence.Description))
                                column.Item().Text("Sin descripción registrada.");
                            else richText.Compose(column.Item(), evidence.Description, evidence.DescriptionFormat);
                        });
                    });
            }
        }).WithMetadata(new DocumentMetadata
        {
            Title = snapshot.IsComplete ? "Informe final de los casos de Docker" : "Informe de avance de los casos de Docker",
            Author = snapshot.Exam.Profile.Student,
            Subject = "Desarrollo, descripciones y evidencias del examen final"
        }).GeneratePdf();
    }

    private static string StatusLabel(string status) => status switch
    {
        "listo" => "Listo para revisión",
        "en-progreso" => "En progreso",
        "pendiente" => "Pendiente",
        _ => status
    };

    private static string LocalDate(DateTimeOffset value) =>
        value.ToOffset(TimeSpan.FromHours(-5)).ToString("dd MMM yyyy, HH:mm", Spanish);

    private static void Configure(PageDescriptor page, bool landscape = false)
    {
        page.Size(landscape ? PageSizes.A4.Landscape() : PageSizes.A4);
        page.Margin(34);
        page.DefaultTextStyle(style => style.FontFamily("Lato").FontSize(10).LineHeight(1.35f));
        page.Header().PaddingBottom(12).BorderBottom(1).BorderColor("#DCE4EF")
            .PaddingBottom(8).Text("DOCKER LAB  /  EXAMEN FINAL").FontColor(Blue).FontSize(8).Bold();
        page.Footer().PaddingTop(12).Row(row =>
        {
            row.RelativeItem().Text("CURS-000662 · Informe de evidencias").FontSize(8).FontColor("#63748D");
            row.AutoItem().Text(text =>
            {
                text.DefaultTextStyle(style => style.FontSize(8).FontColor("#63748D"));
                text.Span("Página "); text.CurrentPageNumber(); text.Span(" de "); text.TotalPages();
            });
        });
    }
}
