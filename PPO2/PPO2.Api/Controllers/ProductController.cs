using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PPO2.Core.DTOs.ProductDto;
using PPO2.Core.Entities;
using PPO2.Core.Interfaces;
using PPO2.Core.Validation.Product;
using PPO2.Infrastructure.Data;

namespace PPO2.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;
        private readonly IMapper _mapper;
        private readonly DataContext _context;

        public ProductController(IProductService productService, IMapper mapper, DataContext context)
        {
            _productService = productService;
            _mapper = mapper;
            _context = context;
        }

        [HttpGet("index")]
        public async Task<IActionResult> Index()
        {
            try
            {
                var result = await _productService.GetAllAsync();
                return Ok(result);
            }
            catch
            {
                return BadRequest();
            }
        }

        [HttpGet("search")]
        public async Task<IActionResult> Search([FromQuery] ProductSearchDto search)
        {
            int page = search.Page;
            int pageSize = 8;
            var query = _context.Products.Include(p => p.Manufacturer).Include(p => p.Orders).Include(p => p.Storage).AsQueryable();

            if (!string.IsNullOrEmpty(search.Name))
            {
                query = query.Where(x => x.Name.ToLower().Contains(search.Name.ToLower()));
            }
            if (!string.IsNullOrEmpty(search.ManufacturerName))
            {
                query = query.Where(x => x.Manufacturer.Name.ToLower().Contains(search.ManufacturerName.ToLower()));
            }
            var model = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(x => _mapper.Map<ProductDto>(x))
                .ToListAsync();
            int total = query.Count();
            int pages = (int)Math.Ceiling(total / (double)pageSize);
            return Ok(new ProductSearchResultDto
            {
                Products = model,
                Total = total,
                CurrentPage = page,
                Pages = pages
            });
        }

        [HttpPost("create")]
        public async Task<IActionResult> Create([FromBody] ProductCreateDto request)
        {
            var validation = new ProductCreateValidation();
            var validationResult = await validation.ValidateAsync(request);

            if (validationResult.IsValid)
            {
                var result = await _productService.CreateAsync(request);
                if (result.Success)
                    return Ok(result);
                return BadRequest(result.Message);
            }
            return BadRequest(validationResult.Errors);
        }

        [HttpPut("update")]
        public async Task<IActionResult> Update([FromBody] ProductUpdateDto request)
        {
            var validation = new ProductUpdateValidation();
            var validationResult = await validation.ValidateAsync(request);
            if (validationResult.IsValid)
            {
                var result = await _productService.UpdateAsync(request);
                if (result.Success)
                    return Ok(result);
                return BadRequest(result.Message);
            }
            return BadRequest(validationResult.Errors);
        }

        [HttpDelete("delete/product-{productId}/manufacturer-{manufacturerId}")]
        public async Task<IActionResult> Delete(int productId, int manufacturerId)
        {
            var result = await _productService.DeleteAsync(productId, manufacturerId);
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _productService.GetById(id);
            if (result.Success)
                return Ok(result);
            return BadRequest(result.Message);
        }

        [HttpGet("manufacturerId-{id}")]
        public async Task<IActionResult> GetByManufacturerId(int id)
        {
            var products = _context.Products.Where(p => p.ManufacturerId == id).ToList();
            if (products == null)
                return Ok("Empty products");
            return Ok(products);
        }
    }
}
