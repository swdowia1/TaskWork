using Markdig;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace TaskWork.Helpers
{
    public static class MarkdownExtensions
    {
        private static readonly MarkdownPipeline _pipeline =
            new MarkdownPipelineBuilder()
                .UseAdvancedExtensions() // obsługa kodu, tabel, linków itd.
                .Build();

        public static IHtmlContent Markdown(this IHtmlHelper html, string markdownText)
        {
            if (string.IsNullOrWhiteSpace(markdownText))
                return HtmlString.Empty;

            var htmlText = Markdig.Markdown.ToHtml(markdownText, _pipeline);

            // Zwracamy jako bezpieczny HTML (uwaga: Markdig sam nie sanitizuje!)
            return new HtmlString(htmlText);
        }
    }
}
