using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PPO2.Core.DTOs.CustomerDto;
using PPO2.Core.Interfaces;
using PPO2.Core.Validation.Customer;
using PPO2.Infrastructure.Data;

namespace PPO2.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;
        private readonly ICustomerService _customerService;
        public CustomerController(ICustomerService customerService, DataContext context, IMapper mapper)
        {
            _customerService = customerService;
            _context = context;
            _mapper = mapper;
        }

        [HttpGet("customers")]
        public async Task<IActionResult> Index()
        {
            var result = await _customerService.GetAllAsync();
            return Ok(result);
        }

        [HttpGet("search")]
        public async Task<IActionResult> Search([FromQuery] CustomerSearchDto search)
        {
            try
            {
                int projectId = -1;
                string projectName = "";
                int page = search.Page;
                int pageSize = 8;
                var query = _context.Customers
                    .Include(p => p.Projects)
                    .Include(o => o.Orders)
                    .AsQueryable();
                if (search.Id != 0)
                {
                    query = query.Where(x => x.Id == search.Id);
                }
                if (!string.IsNullOrEmpty(search.FirstName))
                {
                    query = query.Where(x => x.FirstName.ToLower().Contains(search.FirstName.ToLower()));
                }
                if (!string.IsNullOrEmpty(search.SecondName))
                {
                    query = query.Where(x => x.SecondName.ToLower().Contains(search.SecondName.ToLower()));
                }
                if (!string.IsNullOrEmpty(search.Email))
                {
                    query = query.Where(x => x.Email.ToLower().Contains(search.Email.ToLower()));
                }
                if (!string.IsNullOrEmpty(search.Phone))
                {
                    query = query.Where(x => x.Phone.ToLower().Contains(search.Phone.ToLower()));
                }
                if (!string.IsNullOrEmpty(search.Address))
                {
                    query = query.Where(x => x.Address.ToLower().Contains(search.Address.ToLower()));
                }
                if (search.ProjectId != 0)
                {
                    var proj = await _context.Projects.SingleOrDefaultAsync(x => x.Id == search.ProjectId);
                    if (proj != null)
                    {
                        projectName = proj.Name;
                        projectId = proj.Id;
                        query = query.Where(x => x.Projects.SingleOrDefault(x => x.Id == proj.Id) != null);
                    }
                }
                var model = await query
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .Select(x => _mapper.Map<CustomerDto>(x))
                    .ToListAsync();
                int total = query.Count();
                int pages = (int)Math.Ceiling(total / (double)pageSize);
                return Ok(new CustomerSearchResultDto
                {
                    Customers = model,
                    Total = total,
                    CurrentPage = page,
                    Pages = pages,
                    ProjectId = projectId,
                    ProjectName = projectName
                });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("create")]
        public async Task<IActionResult> Create([FromBody] CustomerCreateDto request)
        {
            var validation = new CustomerCreateValidation();
            var validationResult = await validation.ValidateAsync(request);
            if (validationResult.IsValid)
            {
                var result = await _customerService.CreateAsync(request);
                if (result.Success)
                    return Ok(result);
                return BadRequest(result);
            }
            return BadRequest(validationResult.Errors);
        }

        [HttpPut("update/{id}")]
        public async Task<IActionResult> Update([FromBody] CustomerUpdateDto request, int id)
        {
            var validation = new CustomerUpdateValidation();
            var validationResult = await validation.ValidateAsync(request);
            if (validationResult.IsValid)
            {
                var result = await _customerService.UpdateAsync(request, id);
                if(result.Success)
                    return Ok(result);
                return BadRequest(result);
            }
            return BadRequest(validationResult.Errors);
        }

        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _customerService.DeleteAsync(id);
            return Ok(result);
        }

        [HttpGet("customer/{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _customerService.GetById(id);
            return Ok(result);
        }

        [HttpGet("ByProjectId/{id}")]
        public async Task<IActionResult> GetByProjectId(int id)
        {
            try
            {
                var result = await _customerService.GetByProjectId(id);
                if (result.Success)
                    return Ok(result);
                return BadRequest(result);
            } catch (Exception ex)
            {
                return BadRequest(ex.InnerException.ToString());
            }

        }
    }
}
