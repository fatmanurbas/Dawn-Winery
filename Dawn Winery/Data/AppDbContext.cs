using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Dawn_Winery.Models;

namespace Dawn_Winery.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<EndProduct>? EndProduct { get; set; }
        public DbSet<RawMaterial>? RawMaterial { get; set; }
        public DbSet<Receipe>? Receipe { get; set; }
        public DbSet<Employee>? Employees { get; set; }

        
    }
}
