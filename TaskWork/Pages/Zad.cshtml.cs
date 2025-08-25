using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System;
using TaskWork.FUN;
using TaskWork.ModelApi;
using TaskWork.Models;

namespace TaskWork.Pages
{
    public class ZadModel : PageModel
    {
        private readonly AppDbContext _context;
        public List<TaskSummaryDto> lista { get; set; }
        public ZadModel(AppDbContext context)
        {
            _context = context;
        }

        // Parametry s¹ przekazywane automatycznie z URL
        public async Task OnGetAsync(int companyid, string typ)
        {
            int CompanyId = companyid;
            string Typ = typ;
            DateRange d = new DateRange();
            DateTime DayTo=classFun.CurrentTimeUTC().AddDays(1);
            DateTime DayFrom =d.DateFrom(typ);
            if(typ=="mp")
                DayTo = d.Month;

            var query = await _context.Tasks
       .Where(t => t.CompanyId == companyid)
       .Select(t => new TaskSummaryDto
       {
           TaskId = t.Id,
           Title = t.Title,
           TimeEntries = t.TimeEntries
               .Where(te => te.CreatedAt >= DayFrom && te.CreatedAt < DayTo)
               .Select(te => new TimeEntryDto
               {
                   Id = te.Id,
                   Minutes = te.Minutes,
                   CreatedAt = te.CreatedAt
               })
               .ToList(),
           TotalMinutes = t.TimeEntries
               .Where(te => te.CreatedAt >= DayFrom && te.CreatedAt < DayTo)
               .Sum(te => te.Minutes)
       })
       .Where(x => x.TotalMinutes > 0) // tylko zadania, które maj¹ wpisy w zakresie
       .ToListAsync();


            lista = query;
        }
   
    }
}
