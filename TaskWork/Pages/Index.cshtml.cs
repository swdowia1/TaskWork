using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

using TaskWork.Models;

namespace TaskWork.Pages
{
    public class IndexModel : PageModel
    {
        private readonly IRepository<Article> _repository;
        private readonly IRepository<Tag> _tagRepository;
        public List<Article> lista { get; set; }
        public List<Tag> AllTags { get; set; } = new();
        [BindProperty(SupportsGet = true)]
        public List<string>? SelectedTags { get; set; }

        public IndexModel(IRepository<Article> repository, IRepository<Tag> tagRepository)
        {
            _repository = repository;
            _tagRepository = tagRepository;
        }
        public async Task OnGetAsync()
        {
            // Pobierz wszystkie tagi do listy filtrów
            AllTags = await _tagRepository.GetAllAsync();

            // Pobierz wszystkie artyku³y wraz z tagami
            var allArticles = await _repository.GetAllIncludingAsync<Article>("ArticleTags.Tag");

            // Jeœli wybrano jakieœ tagi, filtruj artyku³y
            if (SelectedTags != null && SelectedTags.Any())
            {
                lista = allArticles
                    .Where(a => a.ArticleTags.Any(at => SelectedTags.Contains(at.Tag.Name)))
                    .ToList();
            }
            else
            {
                lista = allArticles.ToList();
            }
        }
    }
}
