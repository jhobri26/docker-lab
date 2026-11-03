using System.Text.RegularExpressions;
using AngleSharp.Dom;
using AngleSharp.Html.Parser;
using Exam.Api.Infrastructure;
using Markdig;
using QuestPDF.Fluent;
using QuestPDF.Infrastructure;
using IElement = AngleSharp.Dom.IElement;

namespace Exam.Api.Reporting;

// El informe interpreta el contenido guardado; nunca descarga recursos externos.
public sealed class ReportRichText(EvidenceDescription descriptions)
{
    private readonly MarkdownPipeline markdown = new MarkdownPipelineBuilder().UseAdvancedExtensions().Build();

    public void Compose(IContainer container, string value, string format)
    {
        var html = format == "html" ? value : Markdown.ToHtml(value, markdown);
        var document = new HtmlParser().ParseDocument(descriptions.Prepare(html, "html"));
        container.Column(column => Blocks(column, document.Body!.ChildNodes));
    }

    private static void Blocks(ColumnDescriptor column, IEnumerable<INode> nodes)
    {
        var inline = new List<INode>();
        void Flush()
        {
            if (inline.Count == 0) return;
            var group = inline.ToArray();
            inline.Clear();
            if (group.All(node => node is IText text && string.IsNullOrWhiteSpace(text.Data))) return;
            column.Item().PaddingBottom(5).Text(text =>
            {
                foreach (var node in group) Inline(text, node, new());
            });
        }

        foreach (var node in nodes)
        {
            if (node is not IElement element || !IsBlock(element.LocalName))
            {
                inline.Add(node);
                continue;
            }
            Flush();
            var tag = element.LocalName;
            if (tag is "ul" or "ol")
            {
                var index = int.TryParse(element.GetAttribute("start"), out var start) ? start : 1;
                foreach (var child in element.Children.Where(x => x.LocalName == "li"))
                {
                    var label = tag == "ol" ? $"{index++}." : "•";
                    column.Item().Row(row =>
                    {
                        row.ConstantItem(23).Text(label);
                        row.RelativeItem().Column(nested => Blocks(nested, child.ChildNodes));
                    });
                }
            }
            else if (tag == "pre")
                column.Item().PaddingBottom(6).Background("#F0F3F8").Padding(9)
                    .Text(element.TextContent).FontSize(8).BreakAnywhere();
            else if (tag == "blockquote")
                column.Item().PaddingBottom(6).BorderLeft(2).BorderColor("#90A7C9").PaddingLeft(10)
                    .Column(nested => Blocks(nested, element.ChildNodes));
            else if (tag == "hr")
                column.Item().PaddingVertical(6).LineHorizontal(1).LineColor("#DCE4EF");
            else if (tag == "table")
            {
                // Una fila por bloque conserva también tablas anchas y celdas combinadas.
                foreach (var row in element.QuerySelectorAll("tr"))
                {
                    var cells = row.Children.Where(x => x.LocalName is "td" or "th").ToArray();
                    column.Item().BorderBottom(1).BorderColor("#DCE4EF").PaddingVertical(5).Text(text =>
                    {
                        for (var i = 0; i < cells.Length; i++)
                        {
                            if (i > 0) text.Span("  |  ").FontColor("#63748D");
                            Inline(text, cells[i], new(Bold: cells[i].LocalName == "th"));
                        }
                    });
                }
            }
            else if (tag.StartsWith('h') && tag.Length == 2 && char.IsDigit(tag[1]))
                column.Item().PaddingTop(5).PaddingBottom(5).Text(text =>
                {
                    text.DefaultTextStyle(style => style.FontSize(tag == "h1" ? 15 : 12).Bold());
                    foreach (var child in element.ChildNodes) Inline(text, child, new());
                });
            else
                column.Item().Column(nested => Blocks(nested, element.ChildNodes));
        }
        Flush();
    }

    private static bool IsBlock(string tag) => tag is "p" or "div" or "h1" or "h2" or "h3" or "h4"
        or "ul" or "ol" or "li" or "blockquote" or "pre" or "hr" or "table";

    private sealed record InlineStyle(bool Bold = false, bool Italic = false, bool Underline = false,
        bool Strike = false, bool Highlight = false, bool Code = false, string? Link = null);

    private static void Inline(TextDescriptor text, INode node, InlineStyle style)
    {
        if (node is IText value)
        {
            var content = Regex.Replace(value.Data, @"\s+", " ");
            var span = style.Link is null ? text.Span(content) : text.Hyperlink(content, style.Link);
            if (style.Bold) span.Bold();
            if (style.Italic) span.Italic();
            if (style.Underline) span.Underline();
            if (style.Strike) span.Strikethrough();
            if (style.Highlight) span.BackgroundColor("#FFF1AA");
            if (style.Code) span.BackgroundColor("#EEF2F7").FontSize(9);
            if (style.Link is not null) span.FontColor("#2457A7").Underline();
            return;
        }
        if (node is not IElement element) return;
        style = element.LocalName switch
        {
            "strong" or "b" or "th" => style with { Bold = true },
            "em" or "i" => style with { Italic = true },
            "u" => style with { Underline = true },
            "s" or "del" => style with { Strike = true },
            "mark" => style with { Highlight = true },
            "code" => style with { Code = true },
            "a" when SafeLink(element.GetAttribute("href")) => style with { Link = element.GetAttribute("href") },
            _ => style
        };
        if (element.LocalName == "br") text.Span("\n");
        foreach (var child in element.ChildNodes) Inline(text, child, style);
    }

    private static bool SafeLink(string? value) => Uri.TryCreate(value, UriKind.Absolute, out var uri)
        && uri.Scheme is "http" or "https";
}
