using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PPO2.Core.DTOs.ManufacturerDto;
using PPO2.Core.Interfaces;
using PPO2.Core.Validation.Manufacturer;
using PPO2.Infrastructure.Data;

namespace PPO2.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ManufacturerController : ControllerBase
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;
        private readonly IManufacturerService _manufacturerService;

        public ManufacturerController(IManufacturerService manufacturerService, DataContext context, IMapper mapper)
        {
            _manufacturerService = manufacturerService;
            _context = context;
            _mapper = mapper;
        }

        [HttpGet("index")]
        public async Task<IActionResult> Index()
        {
            var result = await _manufacturerService.GetAllAsync();
            return Ok(result);
        }

        [HttpGet("search")]
        public async Task<IActionResult> Search([FromQuery] ManufacturerSearchDto search)
        {
            int page = search.Page;
            int pageSize = 8;
            var query = _context.Manufacturers.Include(m => m.Products).AsQueryable();

            //if (search.Id != 0)
            //{
            //    query = query.Where(x => x.Id == search.Id);
            //}
            if (!string.IsNullOrEmpty(search.Name))
            {
                query = query.Where(x => x.Name.ToLower().Contains(search.Name.ToLower()));
            }
            var model = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(x => _mapper.Map<ManufacturerDto>(x))
                .ToListAsync();
            int total = query.Count();
            int pages = (int)Math.Ceiling(total / (double)pageSize);
            return Ok(new ManufacturerSearchResultDto
            {
                Manufacturers = model,
                Total = total,
                CurrentPage = page,
                Pages = pages
            });
        }

        [HttpPost("create")]
        public async Task<IActionResult> Create([FromBody] ManufacturerCreateDto request)
        {
            var validation = new ManufacturerCreateValidation();
            var validationResult = await validation.ValidateAsync(request);

            if (validationResult.IsValid)
            {
                var result = await _manufacturerService.CreateAsync(request);
                if (result.Success)
                    return Ok(result);
                return BadRequest(result);
            }
            return BadRequest(validationResult.Errors);
        }

        [HttpPut("update/{id}")]
        public async Task<IActionResult> Update([FromBody] ManufacturerUpdateDto request)
        {
            var validation = new ManufacturerUpdateValidation();
            var validationResult = await validation.ValidateAsync(request);

            if (validationResult.IsValid)
            {
                var result = await _manufacturerService.UpdateAsync(request);
                if (result.Success)
                    return Ok(result);
                return BadRequest(result);
            }
            return BadRequest(validationResult.Errors);
        }

        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _manufacturerService.DeleteAsync(id);
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _manufacturerService.GetById(id);
            return Ok(result);
        }
    }
}
