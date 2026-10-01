using DeveloperStore.Domain.Sales;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DeveloperStore.Infrastructure.Persistence.Configurations
{
    public class SaleItemConfiguration : IEntityTypeConfiguration<SaleItem>
    {
        public void Configure(EntityTypeBuilder<SaleItem> builder)
        {
            builder.ToTable("SaleItems");
            builder.HasKey(si => si.Id);
            builder.Property(si => si.ProductId).IsRequired();
            builder.Property(si => si.ProductName).IsRequired().HasMaxLength(200);
            builder.Property(si => si.Quantity).IsRequired();
            builder.Property(si => si.UnitPrice).IsRequired().HasPrecision(15, 2);
            builder.Property(si => si.Discount).IsRequired().HasPrecision(5, 4);
            builder.Property(si => si.TotalAmount).IsRequired().HasPrecision(15, 2);
            builder.Property(si => si.IsCancelled).IsRequired();
        }
    }
}
