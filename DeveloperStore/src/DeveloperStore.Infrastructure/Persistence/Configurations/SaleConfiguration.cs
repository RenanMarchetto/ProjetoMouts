using DeveloperStore.Domain.Sales;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DeveloperStore.Infrastructure.Persistence.Configurations
{
    public class SaleConfiguration : IEntityTypeConfiguration<Sale>
    {
        public void Configure(EntityTypeBuilder<Sale> builder)
        {
            builder.ToTable("Sales");
            builder.HasKey(s => s.Id);
            builder.Property(s => s.SaleNumber).IsRequired().HasMaxLength(50);
            builder.HasIndex(s => s.SaleNumber).IsUnique();
            builder.Property(s => s.SaleDate).IsRequired();
            builder.Property(s => s.CustomerId).IsRequired();
            builder.Property(s => s.CustomerName).IsRequired().HasMaxLength(200);
            builder.Property(s => s.BranchId).IsRequired();
            builder.Property(s => s.BranchName).IsRequired().HasMaxLength(200);
            builder.Property(s => s.IsCancelled).IsRequired();
            builder.Ignore(s => s.TotalAmount);

            builder.HasMany(s => s.Items)
                   .WithOne()
                   .HasForeignKey("SaleId")
                   .OnDelete(DeleteBehavior.Cascade);

            builder.Navigation(sale => sale.Items).UsePropertyAccessMode(PropertyAccessMode.Field);
        }
    }
}
