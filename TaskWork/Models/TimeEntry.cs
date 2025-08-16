using System.ComponentModel.DataAnnotations;

namespace TaskWork.Models
{
    public class TimeEntry
    {
        public int Id { get; set; }

        [Required]
        public int TaskItemId { get; set; }

        public TaskItem? TaskItem { get; set; }

        // Przechowujemy czas w minutach jako liczbę całkowitą
        [Range(1, 24 * 60 * 7)] // np. maks. tydzień na pojedynczy wpis (dowolnie zmień)
        [Display(Name = "Czas [min]")]
        public int Minutes { get; set; }

        [DataType(DataType.DateTime)]
        [Display(Name = "Data")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
