using DeveloperStore.Application.Sales.DTOs;
using DeveloperStore.Application.Sales.Requests;


namespace DeveloperStore.Application.Sales.Services
{
    internal interface ISaleService
    {
        Task<SaleDto> CreateAsync(CreateSaleRequest request, CancellationToken cancellationToken = default);
        Task<SaleDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
        Task<IReadOnlyCollection<SaleDto>> GetAllAsync(CancellationToken cancellationToken = default);
        Task<SaleDto> UpdateAsync(Guid id, UpdateSaleRequest request, CancellationToken cancellationToken = default);
        Task DeleteAsync(Guid id, CancellationToken cancellationToken = default);
        Task CancelAsync(Guid saleId, CancellationToken cancellationToken = default);
        Task CancelItemAsync(Guid saleId, Guid itemId, CancellationToken cancellationToken = default);
    }
}
