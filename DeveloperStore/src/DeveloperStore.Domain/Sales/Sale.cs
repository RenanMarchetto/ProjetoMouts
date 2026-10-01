using DeveloperStore.Domain.Common;
using DeveloperStore.Domain.Exceptions;

namespace DeveloperStore.Domain.Sales
{
    public class Sale : Entity
    {
        private readonly List<SaleItem> _items = [];

        public string SaleNumber { get; private set; }
        public DateTime SaleDate { get; private set; }
        public Guid CustomerId { get; private set; }
        public string CustomerName { get; private set; } = string.Empty;
        public Guid BranchId { get; private set; }
        public string BranchName { get; private set; } = string.Empty;

        public IReadOnlyCollection<SaleItem> Items => _items.AsReadOnly();
        public decimal TotalAmount => _items.Where(item => !item.IsCancelled).Sum(item => item.TotalAmount);
        public bool IsCancelled { get; private set; }

        private Sale()
        {

        }

        public Sale(string saleNumber, DateTime saleDate, Guid customerId, string customerName, Guid branchId,string branchName)
        {
            if (string.IsNullOrWhiteSpace(saleNumber))
            {
                throw new DomainException("Sale number is required.");
            }

            if (customerId == Guid.Empty)
            {
                throw new DomainException("Customer is required.");
            }

            if (string.IsNullOrWhiteSpace(customerName))
            {
                throw new DomainException("Customer name is required.");
            }

            if (branchId == Guid.Empty)
            {
                throw new DomainException("Branch is required.");
            }

            if (string.IsNullOrWhiteSpace(branchName))
            {
                throw new DomainException("Branch name is required.");
            }

            SaleNumber = saleNumber;
            SaleDate = saleDate;
            CustomerId = customerId;
            CustomerName = customerName;
            BranchId = branchId;
            BranchName = branchName;

        }
        
        public void Update(DateTime saleDate, Guid customerId, string customerName, Guid branchId, string branchName)
        {
            EnsureNotCancelled();
            if (customerId == Guid.Empty)
            {
                throw new DomainException("Customer is required.");
            }
            if (string.IsNullOrWhiteSpace(customerName))
            {
                throw new DomainException("Customer name is required.");
            }
            if (branchId == Guid.Empty)
            {
                throw new DomainException("Branch is required.");
            }
            if (string.IsNullOrWhiteSpace(branchName))
            {
                throw new DomainException("Branch name is required.");
            }

            SaleDate = saleDate;
            CustomerId = customerId;
            CustomerName = customerName;
            BranchId = branchId;
            BranchName = branchName;
        }

        public void UpdateItem(Guid itemId, string productName, int quantity, decimal unitPrice)
        {
            EnsureNotCancelled();
            var item = _items.FirstOrDefault(x => x.Id == itemId);
            if (item is null)
                throw new DomainException("Sale item not found.");
            item.Update(productName, quantity, unitPrice);
        }

        public void CancelItem(Guid itemId)
        {
            EnsureNotCancelled();

            var item = _items.FirstOrDefault(x => x.Id == itemId);

            if(item is null)
                throw new DomainException("Sale item not found.");

            item.Cancel();
        }

        public void Cancel()
        {
            if (IsCancelled)
                return;

            IsCancelled = true;
        }

        public void AddItem(Guid productId, string productName, int quantity, decimal unitPrice)
        {
            EnsureNotCancelled();

            if (_items.Any(item => item.ProductId == productId && !item.IsCancelled))
            {
                throw new DomainException("Product already exists in the sale.");
            }

            var item = new SaleItem(productId, productName, quantity, unitPrice);

            _items.Add(item);
        }

        private void EnsureNotCancelled()
        {
            if(IsCancelled)
                throw new DomainException("Cancelled sale can't be modified.");
        }
    }
}
