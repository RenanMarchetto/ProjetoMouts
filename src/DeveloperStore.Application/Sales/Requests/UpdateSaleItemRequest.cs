namespace DeveloperStore.Application.Sales.Requests
{
    public sealed record UpdateSaleItemRequest(
        Guid ProductId,
        string ProductName,
        int Quantity,
        decimal UnitPrice
    );
}
