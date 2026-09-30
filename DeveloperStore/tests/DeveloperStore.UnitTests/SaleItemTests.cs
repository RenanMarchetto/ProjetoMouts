using DeveloperStore.Domain.Exceptions;
using DeveloperStore.Domain.Sales;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeveloperStore.UnitTests
{
    public class SaleItemTests
    {
        [Theory]
        [InlineData(1, 0)]
        [InlineData(3, 0)]
        [InlineData(4, 0.10)]
        [InlineData(9, 0.10)]
        [InlineData(10, 0.20)]
        [InlineData(20, 0.20)]
        public void Constructor_ShouldApplyCorrectDiscount(int quantity, decimal expectedDiscount)
        {
            var productId = Guid.NewGuid();
            const decimal unitPrice = 100m;

            var item = new SaleItem(productId, "Test Product", quantity, unitPrice);

            Assert.Equal(expectedDiscount, item.Discount);
        }

        [Fact]
        public void Constructor_ShouldThrow_WhenQuantityIsGreaterThan20()
        {
            var productId = Guid.NewGuid();
            var action = () => new SaleItem(productId, "Test Product", 21, 100m);

            var exception = Assert.Throws<DomainException>(action);

            Assert.Equal("It is not possible to sell more than 20 identical items.", exception.Message);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        public void Constructor_ShouldThrow_WhenQuantityIsNotPositive(int quantity)
        {
            var action = () => new SaleItem(Guid.NewGuid(), "Test Product", quantity, 100m);

            Assert.Throws<DomainException>(action);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-10)]
        public void Constructor_ShouldThrow_WhenUnitPriceIsNotPositive(decimal unitiPrice)
        {
            var action = () => new SaleItem(Guid.NewGuid(), "Test Product", 1, unitiPrice);

            Assert.Throws<DomainException>(action);
        }

        [Fact]
        public void Constructor_ShouldCalculateTotalAmountWithDiscount()
        {
            const int quantity = 5;
            const decimal unitPrice = 100m;

            var item = new SaleItem(Guid.NewGuid(), "Test Product", quantity, unitPrice);

            Assert.Equal(0.10m, item.Discount);
            Assert.Equal(450m, item.TotalAmount);
        }

        [Fact]
        public void Constructor_ShouldCalculateTotalAmountWithTwentyPercentDiscount()
        {
            var item = new SaleItem(Guid.NewGuid(), "Test Product", 10, 100m);

            Assert.Equal(0.20m, item.Discount);
            Assert.Equal(800m, item.TotalAmount);
        }

        [Fact]
        public void Constructor_ShouldMarkItemAsCancelled()
        {
            var item = new SaleItem(Guid.NewGuid(), "Test Product", 1, 100m);
            item.Cancel();

            Assert.True(item.IsCancelled);
        }
    }
}
