using Microsoft.EntityFrameworkCore;

namespace MyVideoManager.Models
{
    public class WorkContext : DbContext
    {
        public WorkContext(DbContextOptions<WorkContext> options)
            : base(options)
        {
        }

        public DbSet<Work> Works { get; set; }

    }
}