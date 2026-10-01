namespace DeveloperStore.Application.Sales.DTOs
{
    public sealed record SaleDto(
        Guid Id,
        string SaleNumber,
        DateTime SaleDate,
        Guid CustomerId,
        string CustomerName,
        Guid BranchId,
        string BranchName,
        decimal TotalAmount,
        bool IsCancelled,
        IReadOnlyCollection<SaleItemDto> Items
    );
}
