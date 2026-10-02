using DeveloperStore.Domain.Sales.Repositories;
using DeveloperStore.Infrastructure.Persistence;
using DeveloperStore.Infrastructure.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DeveloperStore.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

            services.AddDbContext<DeveloperStoreDbContext>(options => options.UseNpgsql(connectionString));

            services.AddScoped<ISaleRepository, SaleRepository>();
            return services;
        }
    }
}
