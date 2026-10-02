using DeveloperStore.Application.Common.Events;
using DeveloperStore.Application.Common.Exceptions;
using DeveloperStore.Application.Sales.DTOs;
using DeveloperStore.Application.Sales.Requests;
using DeveloperStore.Domain.Exceptions;
using DeveloperStore.Domain.Sales;
using DeveloperStore.Domain.Sales.Repositories;
using System.Data;

namespace DeveloperStore.Application.Sales.Services
{
    public class SaleService : ISaleService
    {
        private readonly ISaleRepository _repository;
        private readonly IDomainEventDispatcher _eventDispatcher;

        public SaleService(ISaleRepository repository, IDomainEventDispatcher eventDispatcher)
        {
            _repository = repository;
            _eventDispatcher = eventDispatcher;
        }

        public async Task<SaleDto> CreateAsync(CreateSaleRequest request, CancellationToken cancellationToken = default)
        {
            var existingSale = await _repository.GetBySaleNumberAsync(request.SaleNumber, cancellationToken);

            if(existingSale != null)
            {
                throw new DomainException($"A sale with the number '{request.SaleNumber}' already exists.");
            }

            var sale = new Sale
            (
                request.SaleNumber,
                request.SaleDate,
                request.CustomerId,
                request.CustomerName,
                request.BranchId,
                request.BranchName
            );

            foreach (var item in request.Items)
            {
                sale.AddItem(item.ProductId, item.ProductName, item.Quantity, item.UnitPrice);
            }

            await _repository.AddAsync(sale, cancellationToken);
            await _repository.SaveChangesAsync(cancellationToken);

            await DispatchDomainEventsAsync(sale, cancellationToken);

            return Map(sale);
        }

        public async Task<SaleDto> UpdateAsync(Guid id, UpdateSaleRequest request, CancellationToken cancellationToken = default)
        {
            var sale = await _repository.GetByIdAsync(id, cancellationToken);

            if(sale is null)
            {
                throw new NotFoundException($"Sale with ID '{id}' not found.");
            }

            sale.Update(request.SaleDate, request.CustomerId, request.CustomerName, request.BranchId, request.BranchName);

            foreach (var item in request.Items)
            {
                sale.UpdateItem(item.ProductId, item.ProductName, item.Quantity, item.UnitPrice);
            }

            await _repository.SaveChangesAsync(cancellationToken);

            await DispatchDomainEventsAsync(sale, cancellationToken);

            return Map(sale);
        }

        public async Task<SaleDto> CancelAsync(Guid id, CancellationToken cancellationToken = default)
        {
            var sale = _repository.GetByIdAsync(id, cancellationToken).Result;

            if(sale is null)
            {
                throw new NotFoundException($"Sale with ID '{id}' not found.");
            }
            
            sale.Cancel();

            await _repository.SaveChangesAsync(cancellationToken);

            await DispatchDomainEventsAsync(sale, cancellationToken);

            return Map(sale);
        }

        public async Task<SaleDto> CancelItemAsync(Guid saleId, Guid itemId, CancellationToken cancellationToken = default)
        {
            var sale = _repository.GetByIdAsync(saleId, cancellationToken).Result;

            if(sale is null)
            {
                throw new NotFoundException($"Sale with ID '{saleId}' not found.");
            }

            sale.CancelItem(itemId);

            await _repository.SaveChangesAsync(cancellationToken);

            await DispatchDomainEventsAsync(sale, cancellationToken);

            return Map(sale);
        }

        public async Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
        {
            var sale = _repository.GetByIdAsync(id, cancellationToken).Result;

            if(sale is null)
            {
                throw new NotFoundException($"Sale with ID '{id}' not found.");
            }
            
            _repository.Delete(sale);

            await _repository.SaveChangesAsync(cancellationToken);
        }

        public async Task<SaleDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            var sale = await _repository.GetByIdAsync(id, cancellationToken);

            return sale is null ? null : Map(sale);
        }

        public async Task<IReadOnlyCollection<SaleDto>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            var sales = await _repository.GetAllAsync(cancellationToken);

            return sales.Select(Map).ToList();
        }
        
        private async Task DispatchDomainEventsAsync(Sale sale, CancellationToken cancellationToken)
        {
            await _eventDispatcher.DispatchAsync(sale.DomainEvents, cancellationToken);

            sale.ClearDomainEvents();
        }

        private static SaleDto Map(Sale sale)
        {
            var items = sale.Items.Select(item => new SaleItemDto
            (                
                item.Id,
                item.ProductId,
                item.ProductName,
                item.Quantity,
                item.UnitPrice,
                item.Discount,
                item.TotalAmount,
                item.IsCancelled
            )).ToList();

            return new SaleDto
            (
                sale.Id,
                sale.SaleNumber,
                sale.SaleDate,
                sale.CustomerId,
                sale.CustomerName,
                sale.BranchId,
                sale.BranchName,
                sale.TotalAmount,
                sale.IsCancelled,
                items
            );
        }
    }
}
