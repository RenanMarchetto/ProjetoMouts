namespace DeveloperStore.Application.Sales.Requests
{
    public sealed record UpdateSaleRequest(
        DateTime SaleDate,      
        Guid CustomerId,
        string CustomerName,
        Guid BranchId,
        string BranchName,
        IReadOnlyCollection<UpdateSaleItemRequest> Items
    );
}
