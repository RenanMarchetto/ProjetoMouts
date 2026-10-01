using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeveloperStore.Application.Sales.Requests
{
    public sealed record CreateSaleRequest(
        string SaleNumber,
        DateTime SaleDate,
        Guid CustomerId,
        string CustomerName,
        Guid BranchId,
        string BranchName,
        IReadOnlyCollection<CreateSaleItemRequest> Items
    );
}
