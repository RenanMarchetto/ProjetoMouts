using DeveloperStore.Domain.Sales;
using DeveloperStore.Domain.Sales.Repositories;
using Microsoft.EntityFrameworkCore;

namespace DeveloperStore.Infrastructure.Persistence.Repositories
{
    public class SaleRepository : ISaleRepository
    {
        private readonly DeveloperStoreDbContext _context;

        public SaleRepository(DeveloperStoreDbContext context)
        {
            _context = context;
        }

        public async Task<Sale?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return await _context.Sales.Include(sale => sale.Items).FirstOrDefaultAsync(s => s.Id == id, cancellationToken);
        }

        public async Task<Sale?> GetBySaleNumberAsync(string saleNumber, CancellationToken cancellationToken = default)
        {
            return await _context.Sales.Include(sale => sale.Items).FirstOrDefaultAsync(s => s.SaleNumber == saleNumber, cancellationToken);
        }

        public async Task<IReadOnlyCollection<Sale>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            return await _context.Sales.Include(sale => sale.Items).OrderByDescending(sale => sale.SaleDate).ToListAsync(cancellationToken);
        }

        public async Task AddAsync(Sale sale, CancellationToken cancellationToken = default)
        {
            await _context.Sales.AddAsync(sale, cancellationToken);
        }

        public void Update(Sale sale)
        {
            _context.Sales.Update(sale);
        }

        public void Delete(Sale sale)
        {
            _context.Sales.Remove(sale);
        }

        public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
