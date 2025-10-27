using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using TaskWork.FUN;
using TaskWork.ModelApi;
using TaskWork.Models;

namespace TaskWork.Pages
{
    public class IndexModel : PageModel
    {
        private readonly IRepository<Article> _repository;
        public List<Article> lista { get; set; }
        public List<CompanyTimeSummaryVm> CompaniesSummary { get; set; } = new();

        public IndexModel(IRepository<Article> repository)
        {
            _repository = repository;
        }
        public async Task OnGetAsync()
        {
            // Pobierz artyku³y razem z tagami (zak³adam, ¿e ArticleTag ma w³aœciwoœæ Tag)
          lista= await _repository.GetAllIncludingAsync<Article>(
    "ArticleTags.Tag"
);

        }
    }
}
