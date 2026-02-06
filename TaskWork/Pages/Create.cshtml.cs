using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TaskWork.Models;
using TaskWork.Serwices;

namespace TaskWork.Pages
{
    public class CreateModel : PageModel
    {
        //AppDbContext
        private readonly AppDbContext _context;
        private readonly TagPredictionService _tagPredictionService;

     
        public CreateModel(AppDbContext context, TagPredictionService tagPredictionService)
        {
            _tagPredictionService = tagPredictionService;
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
        // AJAX endpoint do predykcji tagów
        public IActionResult OnPostPredict([FromBody] string content)
        {
            var predictedTags = _tagPredictionService.PredictTags(content ?? "");
            return new JsonResult(predictedTags);
        }
        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                AllTags = _context.Tags.ToList();
                return Page();
            }

            // 1️⃣ Zapis artykułu
            _context.Articles.Add(Article);
            _context.SaveChanges();

            // 2️⃣ Przypisanie wybranych istniejących tagów
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

            // 3️⃣ Dodanie nowych tagów
            if (!string.IsNullOrWhiteSpace(NewTags))
            {
                var newTagNames = NewTags.Split(',', StringSplitOptions.RemoveEmptyEntries)
                                         .Select(t => t.Trim())
                                         .Where(t => t != "");

                foreach (var tagName in newTagNames)
                {
                    // Sprawdzenie czy tag już istnieje
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
