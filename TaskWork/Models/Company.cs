using System.ComponentModel.DataAnnotations;

namespace TaskWork.Models
{
    public class Company
    {
        public int Id { get; set; }

        [Required, StringLength(150)]
        [Display(Name = "Nazwa firmy")]
        public string Name { get; set; } = string.Empty;

        // Relacja 1..* (firma -> zadania)
        public ICollection<TaskItem> Tasks { get; set; } = new List<TaskItem>();
    }
}
