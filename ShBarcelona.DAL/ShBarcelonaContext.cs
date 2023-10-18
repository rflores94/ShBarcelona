using Microsoft.EntityFrameworkCore;
using ShBarcelona.DAL.Configurations;
using ShBarcelona.DAL.Entities;

namespace ShBarcelona.DAL
{
    public class ShBarcelonaContext : DbContext
    {
        public ShBarcelonaContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new AreaEntityConfiguration());
        }

        public DbSet<AreaEntity> Areas { get; set; }
    }
    
}
