using Microsoft.EntityFrameworkCore;
using portfolioApi.Models;
namespace portfolioApi.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<Projects> Projects {get;set;}
        public DbSet<AboutMe> AboutMe { get; set; }

        public AppDbContext (DbContextOptions<AppDbContext> options) : base(options)
        {}
        
    }
};
