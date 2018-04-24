using Microsoft.EntityFrameworkCore;
using MyVideoManager.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyVideoManager.Models
{
    public class UserWorkContext : DbContext
    {

        DbSet<UserWork> UserWorks { get; set; }
        public UserWorkContext(DbContextOptions<UserWorkContext> options)
            : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserWork>().HasIndex(b => b.UserId).IsUnique();
            modelBuilder.Entity<UserWork>().HasIndex(b => b.WorkId).IsUnique();
        }
    }
}
