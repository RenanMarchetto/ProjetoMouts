using DeveloperStore.Application.Sales.DTOs;
using DeveloperStore.Application.Sales.Requests;
using System.Net.Http.Json;

namespace DeveloperStore.IntegrationTests.Sales
{
    public sealed class SalesControllerTests : IClassFixture<CustomWebApplicationFactory>
    {
        private readonly HttpClient _client;

        public SalesControllerTests(CustomWebApplicationFactory factory)
        {
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task Create_ShouldReturnCreated()
        {
            var request = CreateValidRequest();

            var response = await _client.PostAsJsonAsync("/api/sales", request);

            Assert.Equal(System.Net.HttpStatusCode.Created, response.StatusCode);

            var sale = await response.Content.ReadFromJsonAsync<SaleDto>();
            Assert.NotNull(sale);
            Assert.Equal(request.SaleNumber, sale.SaleNumber);
            Assert.Equal(request.CustomerId, sale.CustomerId);
        }

        [Fact]
        public async Task Create_WithFourItems_ShouldApplyTenPercentDicount()
        {
            var request = CreateValidRequest(quantity: 4);

            var response = await _client.PostAsJsonAsync("/api/sales", request);

            Assert.Equal(System.Net.HttpStatusCode.Created, response.StatusCode);

            var sale = await response.Content.ReadFromJsonAsync<SaleDto>();
            Assert.NotNull(sale);

            var item = Assert.Single(sale.Items);
            Assert.Equal(0.10m, item.Discount);
        }

        [Fact]
        public async Task Create_WithMoreThanTwentyItems_ShouldReturnBadRequest()
        {
            var request = CreateValidRequest(quantity: 25);

            var response = await _client.PostAsJsonAsync("/api/sales", request);

            Assert.Equal(System.Net.HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task GetById_AfterCreatingSale_ShouldReturnSale()
        {
            var request = CreateValidRequest();

            var createResponse = await _client.PostAsJsonAsync("/api/sales", request);
            var createdSale = await createResponse.Content.ReadFromJsonAsync<SaleDto>();
            Assert.NotNull(createdSale);

            var response = await _client.GetAsync($"/api/sales/{createdSale.Id}");
            Assert.Equal(System.Net.HttpStatusCode.OK, response.StatusCode);

            var sale = await response.Content.ReadFromJsonAsync<SaleDto>();
            Assert.NotNull(sale);
            Assert.Equal(createdSale.Id, sale.Id);
            Assert.Equal(createdSale.SaleNumber, sale.SaleNumber);
        }

        private static CreateSaleRequest CreateValidRequest(int quantity = 2)
        {
            return new CreateSaleRequest
            (
                SaleNumber : $"SALE-{Guid.NewGuid()}",
                SaleDate : DateTime.UtcNow,
                CustomerId : Guid.NewGuid(),
                CustomerName : "Test Customer",
                BranchId : Guid.NewGuid(),
                BranchName : "Test Branch",
                Items : new List<CreateSaleItemRequest>
                {
                    new CreateSaleItemRequest
                    (
                        ProductId : Guid.NewGuid(),
                        ProductName : "Test Product",
                        Quantity : quantity,
                        UnitPrice : 100m
                    )
                }
            );
        }
    }
}
