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
