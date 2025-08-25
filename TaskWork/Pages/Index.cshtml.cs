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
            DateRange d=new DateRange();

            CompaniesSummary = await _context.Companies
                .Select(c => new CompanyTimeSummaryVm
                {
                    CompanyName = c.Name,
                    CompanyId = c.Id,
                    Today =classFun.FormatMinutes(c.Tasks
                        .SelectMany(t => t.TimeEntries)
                        .Where(te => te.CreatedAt>d.Day)
                        .Sum(te => te.Minutes)),

                    ThisWeek = classFun.FormatMinutes(c.Tasks
                        .SelectMany(t => t.TimeEntries)
                        .Where(te => te.CreatedAt>=d.Week)
                        .Sum(te => te.Minutes)),

                    ThisMonth = classFun.FormatMinutes(c.Tasks
                        .SelectMany(t => t.TimeEntries)
                        .Where(te => te.CreatedAt>= d.Month)
                        .Sum(te => te.Minutes)),

                    LastMonth = classFun.FormatMinutes(c.Tasks
                        .SelectMany(t => t.TimeEntries)
                        .Where(te => te.CreatedAt>d.MonthPrev && te.CreatedAt<d.Month)
                        .Sum(te => te.Minutes))
                })
                .ToListAsync();
        }
    }
}
