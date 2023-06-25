using AutoMapper;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client.Extensions.Msal;
using PPO2.Core.DTOs.CustomerDto;
using PPO2.Core.DTOs.OrderDto;
using PPO2.Core.DTOs.ProductDto;
using PPO2.Core.DTOs.ProjectDto;
using PPO2.Core.Entities;
using PPO2.Core.Interfaces;
using PPO2.Core.Validation.Order;
using PPO2.Infrastructure.Data;

namespace PPO2.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;
        private readonly DataContext _context;
        private readonly IMapper _mapper;
        private readonly IRepository<Order> _orderRepo;

        public OrderController(IOrderService orderService, DataContext context, IMapper mapper, IRepository<Order> orderRepo)
        {
            _orderService = orderService;
            _context = context;
            _mapper = mapper;
            _orderRepo = orderRepo;
        }

        [HttpGet("search")]
        public async Task<IActionResult> Search([FromQuery] OrderSearchDto search)
        {
            try
            {
                int page = search.Page;
                int pageSize = 8;
                var query = _context.Orders
                    .Include(o => o.Product)
                    .Include(o => o.Project)
                    .Include(o => o.Customer)
                    .Where(s => s.ProjectId == search.ProjectId)
                    .AsQueryable();

                if (!string.IsNullOrEmpty(search.CustomerFirstName))
                {
                    query = query.Where(x => x.Customer.FirstName.ToLower().Contains(search.CustomerFirstName.ToLower()));
                }
                if (!string.IsNullOrEmpty(search.CustomerSecondName))
                {
                    query = query.Where(x => x.Customer.SecondName.ToLower().Contains(search.CustomerSecondName.ToLower()));
                }
                if (!string.IsNullOrEmpty(search.ProductName))
                {
                    query = query.Where(x => x.Product.Name.ToLower().Contains(search.ProductName.ToLower()));
                }
                if (!string.IsNullOrEmpty(search.ManufacturerName))
                {
                    query = query.Where(x => x.Product.Manufacturer.Name.ToLower().Contains(search.ManufacturerName.ToLower()));
                }
                if (search.TotalPrice != 0)
                {
                    query = query.Where(x => x.TotalPrice == search.TotalPrice);
                }
                if (search.ProductQuantity != 0)
                {
                    query = query.Where(x => x.ProductQuantity == search.ProductQuantity);
                }
                var model = await query
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .Select(x => _mapper.Map<OrderDto>(x))
                    .ToListAsync();
                model.ForEach(m => m.Customer = _mapper.Map<CustomerDto>(_context.Customers.FirstOrDefault(c => c.Id == m.CustomerId)));
                model.ForEach(m => m.Product = _mapper.Map<ProductDto>(_context.Products.FirstOrDefault(p => p.Id == m.ProductId)));
                model.ForEach(m => m.Project = _mapper.Map<ProjectDto>(_context.Projects.FirstOrDefault(pr => pr.Id == m.ProjectId)));
                model.ForEach(m => m.Product.ManufacturerName = _context.Manufacturers.FirstOrDefault(man => man.Id == m.Product.ManufacturerId).Name);
                int total = query.Count();
                int pages = (int)Math.Ceiling(total / (double)pageSize);
                return Ok(new OrderSearchResultDto
                {
                    Orders = model,
                    Total = total,
                    CurrentPage = page,
                    Pages = pages
                });

            } catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("search-by-date")]
        public async Task<IActionResult> SearchByDate([FromQuery] OrderSearchDateDto search)
        {
            try
            {
                int page = search.Page;
                int pageSize = 8;
                var query = _context.Orders
                    .Include(o => o.Product)
                    .Include(o => o.Project)
                    .Include(o => o.Customer)
                    .Where(s => s.ProjectId == search.ProjectId)
                    .AsQueryable();

                if(!string.IsNullOrEmpty(search.Day))
                {
                    query = query.Where(x => x.SellDate.Date.Day.ToString().Contains(search.Day));
                }
                if (!string.IsNullOrEmpty(search.Month))
                {
                    query = query.Where(x => x.SellDate.Date.Month.ToString().Contains(search.Month));
                }
                if (!string.IsNullOrEmpty(search.Year))
                {
                    query = query.Where(x => x.SellDate.Date.Year.ToString().Contains(search.Year));
                }
                var model = await query
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .Select(x => _mapper.Map<OrderDto>(x))
                    .ToListAsync();
                model.ForEach(m => m.Customer = _mapper.Map<CustomerDto>(_context.Customers.FirstOrDefault(c => c.Id == m.CustomerId)));
                model.ForEach(m => m.Product = _mapper.Map<ProductDto>(_context.Products.FirstOrDefault(p => p.Id == m.ProductId)));
                model.ForEach(m => m.Project = _mapper.Map<ProjectDto>(_context.Projects.FirstOrDefault(pr => pr.Id == m.ProjectId)));
                model.ForEach(m => m.Product.ManufacturerName = _context.Manufacturers.FirstOrDefault(man => man.Id == m.Product.ManufacturerId).Name);
                int total = query.Count();
                int pages = (int)Math.Ceiling(total / (double)pageSize);
                return Ok(new OrderSearchResultDto
                {
                    Orders = model,
                    Total = total,
                    CurrentPage = page,
                    Pages = pages
                });

            } catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var order = await _context.Orders.Include(c => c.Customer).Include(p => p.Product).FirstOrDefaultAsync(x => x.Id == id);
                var mappedOrder = _mapper.Map<OrderDto>(order);
                mappedOrder.Product.ManufacturerName = _context.Manufacturers.FirstOrDefault(x => x.Id == mappedOrder.Product.ManufacturerId).Name;
                return Ok(mappedOrder);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("create")]
        public async Task<IActionResult> Create([FromBody] OrderCreateDto request)
        {
            var validation = new OrderCreateValidation();
            var validationResult = await validation.ValidateAsync(request);
            if (validationResult.IsValid)
            {
                var result = await _orderService.CreateAsync(request);
                if (result.Success)
                {
                    Order order = result.Payload as Order;
                    Core.Entities.Storage storage = _context.Storage.FirstOrDefault(s => s.ProductId == order.ProductId && s.ProjectId == order.ProjectId);

                    storage.Count -= order.ProductQuantity;

                    try
                    {
                        _context.Storage.Update(storage);
                        _context.Orders.Add(order);
                        await _context.SaveChangesAsync();

                        return Ok(); // cycling ???
                    } catch (Exception ex)
                    {
                        return BadRequest(ex.InnerException.ToString());
                    }
                }
                return BadRequest(result);
            }
            return BadRequest(validationResult.Errors);
        }
        [HttpPut("update")]
        public async Task<IActionResult> Update([FromBody] OrderUpdateDto request)
        {
            var validation = new OrderUpdateValidation();
            var validationResult = await validation.ValidateAsync(request);
            if (validationResult.IsValid)
            {
                try
                {
                    var oldOrder = await _context.Orders.FirstOrDefaultAsync(o => o.Id == request.Id && o.ProjectId == request.ProjectId);
                    var newProject = await _context.Projects.FirstOrDefaultAsync(p => p.Id == request.ProjectId);
                    if (oldOrder == null)
                    {
                        return NotFound("Не знайдено замовлення");
                    }
                    var newCustomer = await _context.Customers.FirstOrDefaultAsync(c => c.Id == request.CustomerId);
                    if (newCustomer == null)
                    {
                        return NotFound("Не знайдено користувача");
                    }
                    var newProduct = await _context.Products.FirstOrDefaultAsync(p => p.Id == request.ProductId);
                    var oldProduct = await _context.Products.FirstOrDefaultAsync(p => p.Id == oldOrder.ProductId);
                    if (newProduct == null || oldProduct == null)
                    {
                        return NotFound("Не знайдено продукт");
                    }
                    var newStorage = await _context.Storage.FirstOrDefaultAsync(s => s.Product == newProduct && s.ProjectId == request.ProjectId);
                    var oldStorage = await _context.Storage.FirstOrDefaultAsync(s => s.Product == oldProduct && s.ProjectId == request.ProjectId);
                    if (oldStorage == null || newStorage == null)
                    {
                        return NotFound("Не знайдено продукту в сховищі");
                    }

                    if (oldStorage != newStorage)
                    {
                        oldStorage.Count += oldOrder.ProductQuantity;
                        if (newStorage.Count - request.ProductQuantity < 0)
                        {
                            return BadRequest("Недостаня кількість продуктів в сховищі");
                        }
                        newStorage.Count -= request.ProductQuantity;
                        _context.Storage.Update(oldStorage);
                        await _context.SaveChangesAsync();
                    }
                    else
                    {
                        if (request.ProductQuantity > oldOrder.ProductQuantity)
                        {
                            int dif = request.ProductQuantity - oldOrder.ProductQuantity;
                            if (newStorage.Count - dif < 0)
                            {
                                return BadRequest("Недостатня кількість продуктів в сховищі");
                            }
                            newStorage.Count -= dif;
                        }
                        else if (request.ProductQuantity < oldOrder.ProductQuantity)
                        {
                            int dif = oldOrder.ProductQuantity - request.ProductQuantity;
                            newStorage.Count += dif;
                        }
                        _context.Storage.Update(newStorage);
                        await _context.SaveChangesAsync();
                    }
                    //var mappedOrder = _mapper.Map<Order>(request);
                    oldOrder.Id = request.Id;
                    oldOrder.ProductId = request.ProductId;
                    oldOrder.Product = newProduct;
                    oldOrder.ProjectId = request.ProjectId;
                    oldOrder.Project = newProject;
                    oldOrder.CustomerId = request.CustomerId;
                    oldOrder.Customer = newCustomer;
                    oldOrder.ProductQuantity = request.ProductQuantity;
                    oldOrder.TotalPrice = request.TotalPrice;
                    oldOrder.SellDate = request.SellDate;
                    _context.Update(oldOrder);
                    await _context.SaveChangesAsync();
                }
                catch (Exception ex)
                {
                    return BadRequest(ex.Message);
                }
            }
            return BadRequest(validationResult.Errors);
        }

        [HttpDelete("delete/orderId-{orderId}/projectId-{projectId}")]
        public async Task<IActionResult> Delete(int orderId, int projectId)
        {
            var order = await _context.Orders.Include(o => o.Product).FirstOrDefaultAsync(o => o.Id == orderId);
            if (order == null)
                return BadRequest("Id not found");
            Product product = order.Product;
            int productQuantity = order.ProductQuantity;
            var storage = _context.Storage.FirstOrDefault(s => s.ProductId == product.Id && s.ProjectId == projectId);
            storage.Count += productQuantity;
            _context.Orders.Remove(order);
            _context.Storage.Update(storage);
            await _context.SaveChangesAsync();
            return Ok();
        }
    }
}
