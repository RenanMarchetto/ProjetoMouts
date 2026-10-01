namespace DeveloperStore.Application.Sales.DTOs
{
    public sealed record SaleItemDto(
        Guid Id,
        Guid ProductId,
        string ProductName,
        int Quantity,
        decimal UnitPrice,
        decimal Discount,
        decimal TotalAmount,
        bool IsCancelled
    );

}
