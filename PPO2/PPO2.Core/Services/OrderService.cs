using AutoMapper;
using PPO2.Core.DTOs.OrderDto;
using PPO2.Core.Entities;
using PPO2.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PPO2.Core.Services
{
    public class OrderService : IOrderService
    {
        private readonly IRepository<Order> _orderRepo;
        private readonly ICustomerService _customerService;
        private readonly IProjectService _projectService;
        private readonly IProductService _productService;
        private readonly IStorageService _storageService;
        private readonly IMapper _mapper;

        public OrderService(IRepository<Order> orderRepo, ICustomerService customerService, IProjectService projectService, IProductService productService, IStorageService storageService, IMapper mapper)
        {
            _orderRepo = orderRepo;
            _customerService = customerService;
            _projectService = projectService;
            _productService = productService;
            _storageService = storageService;
            _mapper = mapper;
        }
        public async Task<ServiceResponse> CreateAsync(OrderCreateDto orderDto)
        {
            var customerResp = await _customerService.GetById(orderDto.CustomerId);
            Customer customer = _mapper.Map<Customer>(customerResp.Payload);
            if (customer == null)
            {
                return new ServiceResponse
                {
                    Success = false,
                    Message = "Customer's id not found"
                };
            }

            var projectResp = await _projectService.GetById(orderDto.ProjectId);
            Project project = _mapper.Map<Project>(projectResp.Payload);
            if (project == null)
            {
                return new ServiceResponse
                {
                    Success = false,
                    Message = "Project's id not found"
                };
            }

            if (project.Customers.FirstOrDefault(c => c.Id == customer.Id) == null)
            {
                return new ServiceResponse
                {
                    Success = false,
                    Message = "Customer doesn't belong to this project"
                };
            }

            var productResp = await _productService.GetById(orderDto.ProductId);
            Product product = _mapper.Map<Product>(productResp.Payload);
            if (product == null)
            {
                return new ServiceResponse
                {
                    Success = false,
                    Message = "Product's id not found"
                };
            }

            var storageResp = await _storageService.ValidatePrimaryKeysAsync(orderDto.ProductId, orderDto.ProjectId);
            Storage storage = _mapper.Map<Storage>(storageResp.Payload);
            if (storage == null)
            {
                return new ServiceResponse
                {
                    Success = false,
                    Message = "This product doesn't belong to this project"
                };
            }

            if (orderDto.ProductQuantity > storage.Count)
            {
                return new ServiceResponse
                {
                    Success = false,
                    Message = $"Недостатня кількість продуктів у сховищі (макс. {storage.Count})"
                };
            }

            orderDto.TotalPrice = orderDto.ProductQuantity * storage.Price;

            var mappedOrder = _mapper.Map<Order>(orderDto);
            mappedOrder.Customer = customer;
            mappedOrder.Project = project;
            mappedOrder.Product = product;

            return new ServiceResponse
            {
                Success = true,
                Message = "Order data is correct",
                Payload = mappedOrder
            };
        }
    }
}
