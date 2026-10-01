using DeveloperStore.Domain.Exceptions;
using DeveloperStore.Domain.Sales;
using DeveloperStore.Domain.Sales.Events;

namespace DeveloperStore.UnitTests
{
    public class SaleTests
    {
        private static Sale CreateSale()
        {
            return new Sale("SALE-01", DateTime.UtcNow, Guid.NewGuid(), "Test Customer", Guid.NewGuid(), "Test Branch");
        }

        [Fact]
        public void AddItem_ShouldAddItemToSale()
        {
            var sale = CreateSale();
            var productId = Guid.NewGuid();

            sale.AddItem(productId, "Test Product", 2, 100m);

            Assert.Single(sale.Items);

            var item = sale.Items.First();

            Assert.Equal(productId, item.ProductId);
            Assert.Equal("Test Product", item.ProductName);
            Assert.Equal(2, item.Quantity);
        }

        [Fact]
        public void TotalAmount_ShouldSumActiveItems()
        {
            var sale = CreateSale();

            sale.AddItem(Guid.NewGuid(), "Test Product A", 2, 100m);
            sale.AddItem(Guid.NewGuid(), "Test Product B", 5, 100m);

            var total = sale.TotalAmount;

            Assert.Equal(650m, total);
        }

        [Fact]
        public void AddItem_ShouldThrow_WhenProductAlreadyExists()
        {
            var sale = CreateSale();
            var productId = Guid.NewGuid();

            sale.AddItem(productId, "Test Product", 2, 100m);

            var action = () => sale.AddItem(productId, "Test Product", 5, 100m);

            Assert.Throws<DomainException>(action);
        }

        [Fact]
        public void CancelItem_ShouldRemoveItemFromSaleTotal()
        {
            var sale = CreateSale();

            sale.AddItem(Guid.NewGuid(), "Test Product A", 2, 100m);
            sale.AddItem(Guid.NewGuid(), "Test Product B", 1, 300m);

            var itemToCancel = sale.Items.Last();

            Assert.Equal(500m, sale.TotalAmount);

            sale.CancelItem(itemToCancel.Id);

            Assert.True(itemToCancel.IsCancelled);
            Assert.Equal(200m, sale.TotalAmount);
        }

        [Fact]
        public void Cancel_ShouldMarkSaleAsCancelled()
        {
            var sale = CreateSale();

            sale.Cancel();

            Assert.True(sale.IsCancelled);
        }

        [Fact]
        public void AddItem_ShouldThrow_WhenSaleIsCanceled()
        {
            var sale = CreateSale();

            sale.Cancel();

            var action = () => sale.AddItem(Guid.NewGuid(), "Test Product", 1, 100m);

            Assert.Throws<DomainException>(action);
        }

        [Fact]
        public void Constructor_ShouldStoreExternalIdentityDescription()
        {
            var customerId = Guid.NewGuid();
            var branchId = Guid.NewGuid();

            var sale = new Sale("SALE-01", DateTime.UtcNow, customerId, "Renan Marchetto", branchId, "São Paulo");

            Assert.Equal(customerId, sale.CustomerId);
            Assert.Equal("Renan Marchetto", sale.CustomerName);
            Assert.Equal(branchId, sale.BranchId);
            Assert.Equal("São Paulo", sale.BranchName);
        }

        [Fact]
        public void Constructor_ShouldRaiseSaleCreatedEvent()
        {
            var sale = CreateSale();

            Assert.Single(sale.DomainEvents, e => e is SaleCreatedEvent);
        }

        [Fact]
        public void Constructor_ShouldRaiseSaleCancelledEvent()
        {
            var sale = CreateSale();

            sale.ClearDomainEvents();
            sale.Cancel();

            Assert.Single(sale.DomainEvents, e => e is SaleCancelledEvent);
        }

        [Fact]
        public void Constructor_ShouldRaiseItemCancelledEvent()
        {
            var sale = CreateSale();

            sale.AddItem(Guid.NewGuid(), "Test Product", 1, 100m);

            var item = sale.Items.First();
            sale.ClearDomainEvents();
            sale.CancelItem(item.Id);

            Assert.Single(sale.DomainEvents, e => e is ItemCancelledEvent);
        }
    }
}
