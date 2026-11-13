using Microsoft.EntityFrameworkCore;
using portfolioApi.Models;

namespace portfolioApi.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<Projects> Projects { get; set; }
        public DbSet<AboutMe> AboutMe { get; set; }
        public DbSet<Contacts> Contacts { get; set; }
        public DbSet<User> Users { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>(entity =>
            {
                entity.HasIndex(u => u.Email).IsUnique();
                entity.HasIndex(u => u.Username).IsUnique();
            });
        }
    }
}
