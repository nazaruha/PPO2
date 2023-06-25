using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PPO2.Core.DTOs.ProductDto;
using PPO2.Core.Interfaces;
using PPO2.Core.Services;
using PPO2.Infrastructure.Data;

namespace PPO2.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductManufacturerController : ControllerBase
    {
        private readonly IProductManufacturerService _productManufacturerService;
        private readonly DataContext _context;

        public ProductManufacturerController(IProductManufacturerService productManufacturerService, DataContext context)
        {
            _productManufacturerService = productManufacturerService;
            _context = context;
        }

        [HttpPost("create/manufacturerId-{manufacturerId}")]
        public async Task<IActionResult> Create(int manufacturerId, [FromBody] ProductCreateDto request)
        {
            var result = await _productManufacturerService.Create(manufacturerId, request);
            if (result.Success)
                return Ok(result);
            return BadRequest(result.Message);
        }

        [HttpGet("get/manufacturerId-{manufacturerId}")]
        public async Task<IActionResult> GetByManufaturerId(int manufacturerId)
        {
            var products = _context.Products.Where(p => p.ManufacturerId == manufacturerId).ToList();
            if (products == null)
                return Ok("Empty products");
            return Ok(products);
        }
    }
}
