using System.ComponentModel.DataAnnotations;

namespace TaskWork.Models
{
    public class TaskItem
    {
        public int Id { get; set; }

        [Required, StringLength(200)]
        [Display(Name = "Temat")]
        public string Title { get; set; } = string.Empty;

        [Required]
        [Display(Name = "Firma")]
        public int CompanyId { get; set; }

        public Company? Company { get; set; }

        public ICollection<TimeEntry> TimeEntries { get; set; } = new List<TimeEntry>();
    }
}
