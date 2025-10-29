using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TaskWork.Models;

namespace TaskWork.Pages
{
    public class CreateModel : PageModel
    {
        //AppDbContext
        private readonly AppDbContext _context;

        public CreateModel(AppDbContext context)
        {
            _context = context;
        }
        [BindProperty]
        public string NewTags { get; set; }
        [BindProperty]
        public Article Article { get; set; }
        [BindProperty]
        public List<string> SelectedTags { get; set; } = new();

        public List<Tag> AllTags { get; set; } = new();
        public void OnGet()
        {
            AllTags = _context.Tags.ToList();
        }
        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                AllTags = _context.Tags.ToList();
                return Page();
            }

            // 1?? Zapis artyku³u
            _context.Articles.Add(Article);
            _context.SaveChanges();

            // 2?? Przypisanie wybranych istniej¹cych tagów
            if (SelectedTags.Any())
            {
                foreach (var tagName in SelectedTags)
                {
                    var tag = _context.Tags.FirstOrDefault(t => t.Name == tagName);
                    if (tag != null)
                    {
                        _context.ArticleTags.Add(new ArticleTag
                        {
                            ArticleId = Article.Id,
                            TagId = tag.Id
                        });
                    }
                }
            }

            // 3?? Dodanie nowych tagów
            if (!string.IsNullOrWhiteSpace(NewTags))
            {
                var newTagNames = NewTags.Split(',', StringSplitOptions.RemoveEmptyEntries)
                                         .Select(t => t.Trim())
                                         .Where(t => t != "");

                foreach (var tagName in newTagNames)
                {
                    // Sprawdzenie czy tag ju¿ istnieje
                    var existingTag = _context.Tags.FirstOrDefault(t => t.Name.ToLower() == tagName.ToLower());
                    if (existingTag == null)
                    {
                        var newTag = new Tag { Name = tagName };
                        _context.Tags.Add(newTag);
                        _context.SaveChanges();

                        _context.ArticleTags.Add(new ArticleTag
                        {
                            ArticleId = Article.Id,
                            TagId = newTag.Id
                        });
                    }
                    else
                    {
                        _context.ArticleTags.Add(new ArticleTag
                        {
                            ArticleId = Article.Id,
                            TagId = existingTag.Id
                        });
                    }
                }
            }

            _context.SaveChanges();

            return RedirectToPage("/Index");
        }

    }
}
