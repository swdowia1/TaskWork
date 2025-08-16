using System.ComponentModel.DataAnnotations;

namespace TaskWork.Models
{
    public class TaskItem
    {
        public int Id { get; set; }

        [Required, StringLength(100)]
        public string Title { get; set; }

        public bool IsDone { get; set; } = false;

        [DataType(DataType.Date)]
        public DateTime? DueDate { get; set; }

        // FK do firmy
        [Display(Name = "Firma")]
        public int CompanyId { get; set; }
        public Company Company { get; set; }
    }
}
