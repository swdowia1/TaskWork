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
        private readonly AppDbContext _context;

        public List<CompanyTimeSummaryVm> CompaniesSummary { get; set; } = new();

        public IndexModel(AppDbContext context)
        {
            _context = context;
        }
        public async Task OnGetAsync()
        {
            
        }
    }
}
