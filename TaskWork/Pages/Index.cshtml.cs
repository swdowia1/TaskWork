using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using TaskWork.Models;

namespace TaskWork.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;

        private readonly IRepository<TaskItem> _tasks;
        private readonly IRepository<Company> _companies;
        private readonly IRepository<TimeEntry> _timeEntries;

        public List<TaskWithTotal> Tasks { get; set; } = new();
        public SelectList TaskSelectList { get; set; } = default!;
        public SelectList CompanySelectList { get; set; } = default!;

        [BindProperty]
        public NewTaskInput NewTask { get; set; } = new();

        [BindProperty]
        public TimeInput TimeForm { get; set; } = new();

        public class TaskWithTotal
        {
            public int Id { get; set; }
            public string Title { get; set; } = string.Empty;
            public string CompanyName { get; set; } = string.Empty;
            public int TotalMinutes { get; set; }
        }
      
        public class NewTaskInput
        {
            [Required, StringLength(200)]
            [Display(Name = "Temat")]
            public string Title { get; set; } = string.Empty;

            [Required]
            [Display(Name = "Firma")]
            public int CompanyId { get; set; }
        }

        public enum TimeUnit { Minuty, Godziny }

        public class TimeInput
        {
            [Required]
            [Display(Name = "Zadanie")]
            public int TaskItemId { get; set; }

            [Required]
            [Range(1, 100000)]
            [Display(Name = "Iloœæ")]
            public int Value { get; set; }

            [Required]
            [Display(Name = "Jednostka")]
            public TimeUnit Unit { get; set; } = TimeUnit.Minuty;
        }

        public IndexModel(ILogger<IndexModel> logger, IRepository<TaskItem> tasks, IRepository<Company> companies, IRepository<TimeEntry> timeEntries)
        {
            _logger = logger;
            _tasks = tasks;
            _companies = companies;
            _timeEntries = timeEntries;
        }

        public async Task OnGetAsync()
        {
          //  var items = await _tasks.GetAllAsync();
            var items = await _tasks.GetAllIncludingAsync<TaskItem>(
       t => t.Company,
       t => t.TimeEntries
   );
            Tasks = items.Select(t => new TaskWithTotal
            {
                Id = t.Id,
                Title = t.Title,
                CompanyName = t.Company?.Name ?? "-",
                TotalMinutes = t.TimeEntries.Sum(te => te.Minutes)
            }).ToList();

            var companies = await _companies.GetAllAsync();
            CompanySelectList = new SelectList(companies, nameof(Company.Id), nameof(Company.Name));
            TaskSelectList = new SelectList(items, nameof(TaskItem.Id), nameof(TaskItem.Title));

          
        }
    }
}
