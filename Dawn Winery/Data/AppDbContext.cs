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

        public DbSet<EndProducts>? EndProducts { get; set; }
        public DbSet<RawMaterials>? RawMaterials { get; set; }
        public DbSet<Receipes>? Receipes { get; set; }
        public DbSet<Employee>? Employees { get; set; }

        
    }
}
