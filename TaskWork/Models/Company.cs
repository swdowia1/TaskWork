using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TaskWork.Models
{
    public class Company
    {
        public int Id { get; set; }

        [Required, StringLength(100)]
        public string Name { get; set; }
    }
}
