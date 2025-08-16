using Microsoft.EntityFrameworkCore;

namespace TaskWork.Models
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<TaskItem> TaskItems { get; set; }
        public DbSet<Company> Companies { get; set; }
    }
}
