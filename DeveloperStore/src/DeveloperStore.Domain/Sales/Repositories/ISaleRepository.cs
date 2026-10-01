namespace DeveloperStore.Domain.Sales.Repositories
{
    public interface ISaleRepository
    {
        Task<Sale?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
        Task<Sale?> GetBySaleNumberAsync(string saleNumber, CancellationToken cancellationToken = default);
        Task<IReadOnlyCollection<Sale>> GetAllAsync(CancellationToken cancellationToken = default);

        void Update(Sale sale);
        void Delete(Sale sale);

        Task SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
