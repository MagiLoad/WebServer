using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Server.Models
{
    public class AppDbContext : DbContext
    {

        private bool _isCreated = false;

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

            if (!_isCreated)
            {
                _isCreated = true;
                Database.EnsureCreated();
            }

        }

        public AppDbContext()
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Server=.;Database=Magiload;Trusted_Connection=True;");
        }

        protected override void OnModelCreating (ModelBuilder modelBuilder)
        {
        }

        public DbSet<File> Files { get; set; }
        public DbSet<User> Users { get; set; }
    }
}
