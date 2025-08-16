using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using TaskWork.Models;

namespace TaskWork.Pages
{
    public class CreateModel : PageModel
    {
        private readonly AppDbContext _context;
        [BindProperty]
        public TaskItem TaskItem { get; set; }

        public SelectList CompanyList { get; set; }
        public CreateModel(AppDbContext context)
        {
            _context = context;
        }

        public void OnGet()
        {
            CompanyList = new SelectList(_context.Companies.ToList(), "Id", "Name");
        }
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                CompanyList = new SelectList(_context.Companies.ToList(), "Id", "Name");
                return Page();
            }

            _context.TaskItems.Add(TaskItem);
            await _context.SaveChangesAsync();
            return RedirectToPage("/Tasks/Index");
        }
    }
}
