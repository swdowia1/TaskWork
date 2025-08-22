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
            var now = DateTime.UtcNow;
            var today = now.Date;
            var firstDayOfWeek = today.AddDays(-(int)today.DayOfWeek + (int)DayOfWeek.Monday);
            var firstDayOfMonth = new DateTime(today.Year, today.Month, 1);
            var firstDayOfLastMonth = firstDayOfMonth.AddMonths(-1);
            var lastDayOfLastMonth = firstDayOfMonth.AddDays(-1);

            CompaniesSummary = await _context.Companies
                .Select(c => new CompanyTimeSummaryVm
                {
                    CompanyName = c.Name,
                    CompanyId = c.Id,
                    Today =classFun.FormatMinutes(c.Tasks
                        .SelectMany(t => t.TimeEntries)
                        .Where(te => te.CreatedAt.Date == today)
                        .Sum(te => te.Minutes)),

                    ThisWeek = classFun.FormatMinutes(c.Tasks
                        .SelectMany(t => t.TimeEntries)
                        .Where(te => te.CreatedAt.Date >= firstDayOfWeek && te.CreatedAt.Date <= today)
                        .Sum(te => te.Minutes)),

                    ThisMonth = classFun.FormatMinutes(c.Tasks
                        .SelectMany(t => t.TimeEntries)
                        .Where(te => te.CreatedAt.Date >= firstDayOfMonth && te.CreatedAt.Date <= today)
                        .Sum(te => te.Minutes)),

                    LastMonth = classFun.FormatMinutes(c.Tasks
                        .SelectMany(t => t.TimeEntries)
                        .Where(te => te.CreatedAt.Date >= firstDayOfLastMonth && te.CreatedAt.Date <= lastDayOfLastMonth)
                        .Sum(te => te.Minutes))
                })
                .ToListAsync();
        }
    }
}
