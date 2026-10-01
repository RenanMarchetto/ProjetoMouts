using DeveloperStore.Application.Sales.Services;
using Microsoft.Extensions.DependencyInjection;

namespace DeveloperStore.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddScoped<ISaleService, SaleService>();
            return services;
        }
    }
}
