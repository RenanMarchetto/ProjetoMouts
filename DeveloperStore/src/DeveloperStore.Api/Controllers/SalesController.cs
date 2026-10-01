using DeveloperStore.Application.Sales.DTOs;
using DeveloperStore.Application.Sales.Requests;
using DeveloperStore.Application.Sales.Services;
using Microsoft.AspNetCore.Mvc;

namespace DeveloperStore.Api.Controllers
{
    [ApiController]
    [Route("api/sales")]
    public sealed class SalesController : ControllerBase
    {
        private readonly ISaleService _saleService;

        public SalesController(ISaleService saleService)
        {
            _saleService = saleService;
        }

        [HttpPost]
        [ProducesResponseType(typeof(SaleDto), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<SaleDto>> CreateAsync([FromBody] CreateSaleRequest request, CancellationToken cancellationToken)
        {
            var sale = await _saleService.CreateAsync(request, cancellationToken);
            return CreatedAtAction(nameof(GetByIdAsync), new { id = sale.Id }, sale);
        }

        [HttpGet("{id:guid}")]
        [ProducesResponseType(typeof(SaleDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<SaleDto>> GetByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            var sale = await _saleService.GetByIdAsync(id, cancellationToken);
            if (sale is null)
            {
                return NotFound();
            }
            return Ok(sale);
        }

        [HttpGet]
        public async Task<ActionResult<IReadOnlyCollection<SaleDto>>> GetAllAsync(CancellationToken cancellationToken)
        {
            var sales = await _saleService.GetAllAsync(cancellationToken);
            return Ok(sales);
        }

        [HttpPut("{id:guid}")]
        [ProducesResponseType(typeof(SaleDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<SaleDto>> UpdateAsync(Guid id, [FromBody] UpdateSaleRequest request, CancellationToken cancellationToken)
        {
            var sale = await _saleService.UpdateAsync(id, request, cancellationToken);
            if (sale is null)
            {
                return NotFound();
            }
            return Ok(sale);
        }

        [HttpPatch("{id:guid}/cancel")]
        [ProducesResponseType(typeof(SaleDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<SaleDto>> CancelAsync(Guid id, CancellationToken cancellationToken)
        {
            var sale = await _saleService.CancelAsync(id, cancellationToken);
            if (sale is null)
            {
                return NotFound();
            }
            return Ok(sale);
        }

        [HttpPatch("{saleId:guid}/items/{itemId:guid}/cancel")]
        [ProducesResponseType(typeof(SaleDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<SaleDto>> CancelItemAsync(Guid saleId, Guid itemId, CancellationToken cancellationToken)
        {
            var sale = await _saleService.CancelItemAsync(saleId, itemId, cancellationToken);
            if (sale is null)
            {
                return NotFound();
            }
            return Ok(sale);
        }

        public async Task<IActionResult> DeleteAsync(Guid id, CancellationToken cancellationToken)
        {
            await _saleService.DeleteAsync(id, cancellationToken);
            return NoContent();
        }
    }
}
