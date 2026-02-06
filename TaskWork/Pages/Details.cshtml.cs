using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TaskWork.Models;

namespace TaskWork.Pages
{
    public class DetailsModel : PageModel
    {
        private readonly IRepository<Article> _repository;

        public DetailsModel(IRepository<Article> repository)
        {
            _repository = repository;
        }

        public Article Article { get; set; }
        public string RenderMarkdown(string markdown)
        {
            return Markdig.Markdown.ToHtml(markdown ?? string.Empty);
        }
        public async Task<IActionResult> OnGet(int id)
        {
            Article = await _repository.GetByIdIncludingAsync(
        id,
        "ArticleTags.Tag"
    );
            if (Article == null)
                return NotFound();

            return Page();
        }
    }
}
