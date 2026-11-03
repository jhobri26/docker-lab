using Ganss.Xss;

namespace Exam.Api.Infrastructure;

public sealed class EvidenceDescription
{
    private readonly HtmlSanitizer sanitizer;

    public EvidenceDescription()
    {
        sanitizer = new HtmlSanitizer();
        sanitizer.AllowedTags.Clear();
        foreach (var tag in new[] { "p", "div", "strong", "b", "em", "i", "u", "s", "del", "mark", "br", "h1", "h2", "h3", "h4", "ul", "ol", "li", "blockquote", "pre", "code", "hr", "span", "a", "table", "thead", "tbody", "tr", "th", "td" })
            sanitizer.AllowedTags.Add(tag);
        sanitizer.AllowedAttributes.Clear();
        foreach (var attribute in new[] { "href", "title", "colspan", "rowspan", "start" })
            sanitizer.AllowedAttributes.Add(attribute);
        sanitizer.AllowedSchemes.Clear();
        sanitizer.AllowedSchemes.Add("https");
        sanitizer.AllowedSchemes.Add("http");
    }

    public string Prepare(string? description, string format)
    {
        var value = description?.Trim() ?? "";
        // Markdown se conserva como texto. El HTML del editor se limpia también en servidor.
        return format == "html" ? sanitizer.Sanitize(value) : value;
    }
}
