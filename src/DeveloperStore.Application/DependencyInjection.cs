using DeveloperStore.Application.Common.Events;
using DeveloperStore.Application.Sales.Services;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace DeveloperStore.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddScoped<ISaleService, SaleService>();
            services.AddScoped<IDomainEventDispatcher, DomainEventDispatcher>();
            services.AddValidatorsFromAssembly(typeof(DependencyInjection).Assembly);
            return services;
        }
    }
}
