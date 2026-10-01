using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeveloperStore.Application.Sales.Requests
{
    public sealed record CreateSaleItemRequest(
        Guid ProductId,
        string ProductName,        
        int Quantity,
        decimal Price,
        decimal UnitPrice
    );
}
