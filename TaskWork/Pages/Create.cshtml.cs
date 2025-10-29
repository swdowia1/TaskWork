using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TaskWork.Models;

namespace TaskWork.Pages
{
    public class CreateModel : PageModel
    {
        [BindProperty]
        public Article Article { get; set; }
        public List<Tag> AllTags { get; set; } = new();
        public void OnGet()
        {
        }
    }
}
