using DeveloperStore.Domain.Common;
using DeveloperStore.Domain.Exceptions;

namespace DeveloperStore.Domain.Sales
{
    public class SaleItem : Entity
    {
        public Guid ProductId { get; private set; }
        public string ProductName { get; private set; } = string.Empty;
        public int Quantity { get; private set; }
        public decimal UnitPrice { get; private set; }
        public decimal Discount { get; private set; }
        public decimal TotalAmount { get; private set; }
        public bool IsCancelled { get; private set; }

        private SaleItem()
        {

        }

        public SaleItem(Guid productId, string productName, int quantity, decimal unitPrice)
        {
            if(productId == Guid.Empty)
            {
                throw new DomainException("Product is required.");
            }

            if (string.IsNullOrWhiteSpace(productName))
            {
                throw new DomainException("Product name is required.");
            }

            if (quantity <= 0)
            {
                throw new DomainException("Quantity must be greater than zero.");
            }

            if (quantity > 20)
            {
                throw new DomainException("It is not possible to sell more than 20 identical items.");
            }

            if (unitPrice <= 0)
            {
                throw new DomainException("Unit price must be greater than zero.");
            }

            ProductId = productId;
            ProductName = productName;
            Quantity = quantity;
            UnitPrice = unitPrice;

            Discount = CalculateDiscount(quantity);
            TotalAmount = CalculateTotalAmount();
        }

        internal void Update(string productName, int quantity, decimal unitPrice)
        {
            if (IsCancelled)
            {
                throw new DomainException("Cannot update a cancelled item.");
            }
            if (quantity <= 0)
            {
                throw new DomainException("Quantity must be greater than zero.");
            }
            if (quantity > 20)
            {
                throw new DomainException("It is not possible to sell more than 20 identical items.");
            }
            if (unitPrice <= 0)
            {
                throw new DomainException("Unit price must be greater than zero.");
            }
            if (string.IsNullOrWhiteSpace(productName))
            {
                throw new DomainException("Product name is required.");
            }
            Quantity = quantity;
            UnitPrice = unitPrice;
            Discount = CalculateDiscount(quantity);
            TotalAmount = CalculateTotalAmount();
        }

        public void Cancel()
        {
            if (IsCancelled)
                throw new DomainException("Cannot cancel an already cancelled item.");

            IsCancelled = true;
        }

        private decimal CalculateTotalAmount()
        {
            var grossAmount = UnitPrice * Quantity;

            return grossAmount * (1 - Discount);
        }

        private decimal CalculateDiscount(int quantity)
        {
            if (quantity >= 10)
                return 0.20m;

            if (quantity >= 4)
                return 0.10m;

            return 0m;
        }
    }
}
