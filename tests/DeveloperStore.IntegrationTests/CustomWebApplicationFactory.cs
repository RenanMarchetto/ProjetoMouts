using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using DeveloperStore.Infrastructure.Persistence;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace DeveloperStore.IntegrationTests
{
    public sealed class CustomWebApplicationFactory : WebApplicationFactory<Program>
    {
        private readonly string _databaseName = $"DeveloperStoreDb_{Guid.NewGuid()}";
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.UseEnvironment("IntegrationTests");
            builder.ConfigureServices(services =>
            {
                var descriptor = services.Where(d => d.ServiceType == typeof(DbContextOptions<DeveloperStoreDbContext>) || d.ServiceType == typeof(DbContextOptions)).ToList();

                foreach (var d in descriptor)
                {
                    services.Remove(d);
                }

                services.AddDbContext<DeveloperStoreDbContext>(options =>
                {
                    options.UseInMemoryDatabase(_databaseName);
                    options.ConfigureWarnings(warnings => warnings.Ignore(InMemoryEventId.TransactionIgnoredWarning));
                });
            });
        }
    }
}
