using Ardalis.Specification.EntityFrameworkCore;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client.Extensions.Msal;
using PPO2.Core.DTOs.ProductDto;
using PPO2.Core.DTOs.ProjectDto;
using PPO2.Core.DTOs.Storage;
using PPO2.Core.Entities;
using PPO2.Core.Interfaces;
using PPO2.Core.Services;
using PPO2.Core.Validation.Storage;
using PPO2.Infrastructure.Data;

namespace PPO2.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StorageController : ControllerBase
    {
        private readonly IStorageService _storageService;
        private readonly IMapper _mapper;
        private readonly DataContext _context;
        private ServiceResponse _serviceResponse;

        public StorageController(IStorageService storageService, IMapper mapper, DataContext context)
        {
            _storageService = storageService;
            _mapper = mapper;
            _context = context;
        }
        [HttpGet("index")]
        public async Task<IActionResult> Index()
        {
            try
            {
                var storage = await _context.Storage.ToListAsync();
                storage.ForEach(s => s.Product = _context.Products.FirstOrDefault(p => p.Id == s.ProductId));
                storage.ForEach(s => s.Project = _context.Projects.FirstOrDefault(p => p.Id == s.ProjectId));
                var mappedStorage = _mapper.Map<List<StorageDto>>(storage);
                _serviceResponse = new ServiceResponse
                {
                    Success = true,
                    Message = "Get the whole Storage",
                    Payload = storage
                };
                return Ok(_serviceResponse);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.InnerException.ToString());  
            }
        }

        [HttpGet("search/{projectId}")]
        public async Task<IActionResult> Search(int projectId, [FromQuery] StorageSearchDto search)
        {
            try
            {
                int page = search.Page;
                int pageSize = 8;
                var query = _context.Storage
                    .Include(p => p.Project)
                    .Include(p => p.Product)
                    .Where(s => s.ProjectId == projectId)
                    .AsQueryable();

                if (!string.IsNullOrEmpty(search.ProductName))
                {
                    query = query.Where(x => x.Product.Name.ToLower().Contains(search.ProductName.ToLower()));
                }
                if (!string.IsNullOrEmpty(search.ManufacturerName))
                {
                    query = query.Where(x => x.Product.Manufacturer.Name.ToLower().Contains(search.ManufacturerName.ToLower()));
                }
                if (!string.IsNullOrEmpty(search.Description))
                {
                    query = query.Where(x => x.Description.ToLower().Contains(search.Description.ToLower()));
                }
                if (search.Price != 0)
                {
                    query = query.Where(x => x.Price == search.Price);
                }
                if (search.Count != 0)
                {
                    query = query.Where(x => x.Count == search.Count);
                }
                var model = await query
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .Select(x => _mapper.Map<StorageDto>(x))
                    .ToListAsync();
                model.ForEach(m => m.Product = _mapper.Map<ProductDto>(_context.Products.FirstOrDefault(p => p.Id == m.ProductId)));
                model.ForEach(m => m.Product.ManufacturerName = _context.Manufacturers.FirstOrDefault(x => x.Id == m.Product.ManufacturerId).Name);
                model.ForEach(m => m.Project = _mapper.Map<ProjectDto>(_context.Projects.FirstOrDefault(p => p.Id == m.ProjectId)));
                int total = query.Count();
                int pages = (int)Math.Ceiling(total / (double)pageSize);
                return Ok(new StorageSearchResultDto
                {
                    Storage = model,
                    Total = total,
                    CurrentPage = page,
                    Pages = pages
                });
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("create")]
        public async Task<IActionResult> Create([FromBody] StorageCreateDto request)
        {
            var validation = new StorageCreateValidation();
            var validationResult = await validation.ValidateAsync(request);
            if (validationResult.IsValid)
            {
                var result = await _storageService.CreateAsync(request);
                if (result.Success)
                {
                    if (result.Payload == null)
                        return BadRequest("Payload == null");

                    if(_context.Storage.FirstOrDefault(s => s.ProductId == request.ProductId && s.ProjectId == request.ProjectId) != null)
                    {
                        return BadRequest("Цей продукт з виробником вже існують в сховищі даного проекту");
                    }

                    Core.Entities.Storage storage = result.Payload as Core.Entities.Storage;
                    await _context.Storage.AddAsync(storage);
                    await _context.SaveChangesAsync();
                    return Ok("Storage has been added");
                }
                return BadRequest(result);
            }
            return BadRequest(validationResult.Errors);
        }

        [HttpPut("update")]
        public async Task<IActionResult> Update([FromBody] StorageUpdateDto request)
        {
            var validation = new StorageUpdateValidation();
            var validationResult = await validation.ValidateAsync(request);
            if (!validationResult.IsValid)
                return BadRequest(validationResult.Errors);

            var result = await _storageService.ValidatePrimaryKeysAsync(request.ProductId, request.ProjectId);
            if (result.Success)
            {
                try
                {
                    var storage = await GetByPKsAsync(request.ProductId, request.ProjectId);
                    if (storage == null)
                    {
                        _serviceResponse = new ServiceResponse
                        {
                            Success = false,
                            Message = "Storage element not found"
                        };
                        return NotFound(_serviceResponse);
                    }

                    storage.Price = request.Price;
                    storage.Count = request.Count;
                    storage.Description = request.Description;
                    storage.ExpireDate = request.ExpireDate;

                    _context.Storage.Update(storage);
                    await _context.SaveChangesAsync();
                    return Ok("Storage has been updated");
                }
                catch (Exception ex)
                {
                    _serviceResponse = new ServiceResponse
                    {
                        Success = false,
                        Message = ex.InnerException.ToString()
                    };
                    return BadRequest(_serviceResponse);
                }
            }
            return BadRequest(result);
        }

        [HttpDelete("delete/product-{productId}/project-{projectId}")]
        public async Task<IActionResult> Delete(int productId, int projectId)
        {
            var result = await _storageService.ValidatePrimaryKeysAsync(productId, projectId);
            if (result.Success)
            {
                try
                {
                    var storage = await _context.Storage.FirstOrDefaultAsync(x => x.ProductId == productId && x.ProjectId == projectId);
                    if (storage == null)
                    {
                        return NotFound("Storage element not found");
                    }
                    _context.Storage.Remove(storage);
                    await _context.SaveChangesAsync();
                    return Ok("Storage element has been deleted");
                } catch (Exception ex)
                {
                    _serviceResponse = new ServiceResponse
                    {
                        Success = false,
                        Message = ex.InnerException.ToString()
                    };
                    return BadRequest(_serviceResponse);
                }
            }
            return BadRequest(result);
        }

        [HttpGet("manufacturer/{projectId}")]
        public async Task<IActionResult> GetProjectManufacturers(int projectId)
        {
            try
            {
                List<Manufacturer> manufacturers = await _context.Manufacturers.ToListAsync();
                var storage = _context.Storage.Include(p => p.Product).Where(x => x.ProjectId == projectId);

                List<Manufacturer> projectManufacturers = new List<Manufacturer>();
                foreach (var manufacturer in manufacturers)
                {
                    if (storage.FirstOrDefault(s => s.Product.ManufacturerId == manufacturer.Id) != null)
                    {
                        projectManufacturers.Add(manufacturer);
                    }
                }
                return Ok(projectManufacturers);
            } catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("product/{manufacturerId}/{projectId}")]
        public async Task<IActionResult> GetProjectProductByManufacturer(int manufacturerId, int projectId)
        {
            try
            {
                var storage = _context.Storage.Include(p => p.Product).Where(x => x.ProjectId == projectId && x.Count > 0);

                List<Product> products = new List<Product>();
                foreach (var storageItem in storage)
                {
                    Product product = storageItem.Product;
                    if (product.ManufacturerId == manufacturerId)
                    {
                        products.Add(product);
                    }
                }
                return Ok(products);
            } catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("product-edit/{manufacturerId}/{productId}/{projectId}")]
        public async Task<IActionResult> GetProjectProductEditByManufacturer(int manufacturerId, int productId, int projectId)
        {
            try
            {
                var allStorage = _context.Storage.Include(p => p.Product).Where(x => x.ProjectId == projectId);
                List<Core.Entities.Storage> storage = new List<Core.Entities.Storage>();
                foreach (var item in allStorage)
                {
                    if (item.Count <= 0 && item.ProductId != productId)
                        continue;
                    storage.Add(item);
                }

                List<Product> products = new List<Product>();
                foreach (var storageItem in storage)
                {
                    Product product = storageItem.Product;
                    if (product.ManufacturerId == manufacturerId)
                    {
                        products.Add(product);
                    }
                }
                return Ok(products);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("product-{productId}/project-{projectId}")]
        public async Task<IActionResult> GetByProductProjectIds (int productId, int projectId)
        {
            var storage = await GetByPKsAsync(productId, projectId);
            if (storage != null)
            {
                var mappedStorage = _mapper.Map<StorageDto>(storage);
                //_serviceResponse = new ServiceResponse
                //{
                //    Success = true,
                //    Message = "Get Sotrage element by PKs",
                //    Payload = mappedStorage
                //};
                return Ok(storage);
            }
            _serviceResponse = new ServiceResponse
            {
                Success = false,
                Message = "Storage element not found"
            };
            return BadRequest(_serviceResponse);
        }

        private async Task<Core.Entities.Storage> GetByPKsAsync (int productId, int projectId)
        {
            try
            {
                //Core.Entities.Storage storage = await _context.Storage.FirstOrDefaultAsync(s => s.ProductId == productId && s.ProjectId == projectId);

                Core.Entities.Storage storage2 = (Core.Entities.Storage)_context.Storage.Include(x => x.Product).Include(x => x.Project).FirstOrDefault(s => s.ProductId == productId && s.ProjectId == projectId);
                return storage2;
            } catch
            {
                return null;
            }
        }
    }
}
