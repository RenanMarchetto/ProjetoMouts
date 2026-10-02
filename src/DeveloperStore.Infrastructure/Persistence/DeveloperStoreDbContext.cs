using DeveloperStore.Domain.Sales;
using Microsoft.EntityFrameworkCore;

namespace DeveloperStore.Infrastructure.Persistence
{
    public class DeveloperStoreDbContext : DbContext
    {
        public DeveloperStoreDbContext(DbContextOptions<DeveloperStoreDbContext> options) : base(options)
        {
        }

        public DbSet<Sale> Sales { get; set; }

        public DbSet<SaleItem> SaleItems { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(DeveloperStoreDbContext).Assembly);
            base.OnModelCreating(modelBuilder);
        }
    }
}
