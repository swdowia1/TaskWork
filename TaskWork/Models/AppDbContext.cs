using Microsoft.EntityFrameworkCore;

namespace TaskWork.Models
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<TaskItem> Tasks => Set<TaskItem>();
        public DbSet<TimeEntry> TimeEntries => Set<TimeEntry>();
        public DbSet<Company> Companies => Set<Company>();
    }
}
