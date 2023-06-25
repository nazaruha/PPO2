using AutoMapper;
using Microsoft.Extensions.Logging;
using PPO2.Core.DTOs.ManufacturerDto;
using PPO2.Core.DTOs.ProductDto;
using PPO2.Core.Entities;
using PPO2.Core.Entities.Specification;
using PPO2.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PPO2.Core.Services
{
    public class ProductService : IProductService
    {
        private readonly IRepository<Product> _productRepo;
        private readonly IRepository<Manufacturer> _manufacturerRepo;
        private readonly IMapper _mapper;

        public ProductService(IRepository<Product> productRepo, IRepository<Manufacturer> manufacturerRepo, IMapper mapper)
        {
            _productRepo = productRepo;
            _manufacturerRepo = manufacturerRepo;
            _mapper = mapper;
        }

        public async Task<ServiceResponse> GetAllAsync()
        {
            var products = await _productRepo.GetListBySpec(new ProductSpecification.GetAll());
            var manufacturers = await _manufacturerRepo.GetAll();
            var mappedProducts = _mapper.Map<List<ProductDto>>(products);
            mappedProducts.ForEach(p => p.Manufacturers.Add(_mapper.Map<ManufacturerDto>(manufacturers.FirstOrDefault(m => m.Id == p.ManufacturerId))));
            return new ServiceResponse
            {
                Success = true,
                Message = "Get All Products",
                Payload = mappedProducts
            };
        }

        public async Task<ServiceResponse> CreateAsync(ProductCreateDto productDto)
        {
            var manufacturer = await _manufacturerRepo.GetItemBySpec(new ManufacturerSpecification.GetById(productDto.ManufacturerId));
            if (manufacturer == null)
            {
                return new ServiceResponse
                {
                    Success = false,
                    Message = "Manufacturer's id doesn't exist"
                };
            }

            var product = await _productRepo.GetItemBySpec(new ProductSpecification.GetByManufacturerIdAndName(productDto.Name, productDto.ManufacturerId));
            if (product != null)
            {
                return new ServiceResponse
                {
                    Success = false,
                    Message = "Цей продукт вже зареєстрований в даного виробника"
                };
            }

            try
            {
                var mappedProduct = _mapper.Map<Product>(productDto); ;
                manufacturer.Products.Add(mappedProduct);
                await _manufacturerRepo.Save();

                return new ServiceResponse
                {
                    Success = true,
                    Message = "Product has been added",
                    Payload = product
                };
            }
            catch (Exception ex)
            {
                return new ServiceResponse
                {
                    Success = false,
                    Message = ex.InnerException.ToString()
                };
            }
        }

        public async Task<ServiceResponse> UpdateAsync(ProductUpdateDto productDto)
        {
            var product = await _productRepo.GetItemBySpec(new ProductSpecification.GetById(productDto.Id));
            if (product == null)
            {
                return new ServiceResponse
                {
                    Success = false,
                    Message = "Product's Id not found"
                };
            }
            if (product.Id == productDto.Id && product.Name == productDto.Name && product.ManufacturerId == productDto.ManufacturerId)
            {
                return new ServiceResponse
                {
                    Success = true,
                    Message = "Nothing to change"
                };
            }    
            //var oldManufacturer = await _manufacturerRepo.GetItemBySpec(new ManufacturerSpecification.GetById(product.ManufacturerId));
            var oldManufacturer = product.Manufacturer;
            var newManufacturer = await _manufacturerRepo.GetItemBySpec(new ManufacturerSpecification.GetById(productDto.ManufacturerId));
            if (newManufacturer == null)
            {
                return new ServiceResponse
                {
                    Success = false,
                    Message = "New Manufacturer's Id not found"
                };
            }
            var findProductByName = newManufacturer.Products.FirstOrDefault(p => p.Name == productDto.Name && p.Id != productDto.Id); // oldManufacturer
            if (findProductByName != null)
            {
                return new ServiceResponse
                {
                    Success = false,
                    Message = "Цей продукт вже існує в даному виробнику",
                };
            }

            product.Name = productDto.Name;
            if (product.ManufacturerId != productDto.ManufacturerId)
            {
                
                var sameProductName = newManufacturer.Products.FirstOrDefault(p => p.Name == productDto.Name);
                if (sameProductName == null)
                {
                    oldManufacturer.Products.Remove(product);
                    newManufacturer.Products.Add(product);
                    product.Manufacturer = newManufacturer;
                    await _manufacturerRepo.Save();
                }
                else
                {
                    return new ServiceResponse
                    {
                        Success = false,
                        Message = "Product's name is already occupied by this manufacturer",
                        Payload = newManufacturer
                    };
                }
            }
            await _productRepo.Save();
            return new ServiceResponse
            {
                Success = true,
                Message = $"Product {productDto.Id} has been updated"
            };
        }

        public async Task<ServiceResponse> DeleteAsync(int productId, int manufacturerId)
        {
            var product = await _productRepo.GetItemBySpec(new ProductSpecification.GetByIdAndManufacturerId(productId, manufacturerId));
            if (product == null)
            {
                return new ServiceResponse
                {
                    Success = false,
                    Message = "Incorrect Product's or Manufacturer's Id"
                };
            }

            try
            {
                await _productRepo.Delete(product);
                await _productRepo.Save();

                var productsResult = await GetAllAsync();

                return new ServiceResponse
                {
                    Success = true,
                    Message = $"Product #[{productId};{manufacturerId}] has been deleted",
                    Payload = productsResult.Payload
                };
            }
            catch (Exception ex)
            {
                return new ServiceResponse
                {
                    Success = false,
                    Message = ex.InnerException.ToString()
                };
            }
        }

        public async Task<ServiceResponse> GetById(int id)
        {
            var product = await _productRepo.GetItemBySpec(new ProductSpecification.GetById(id));
            if (product == null)
            {
                return new ServiceResponse
                {
                    Success = false,
                    Message = "Product's id not found"
                };
            }

            var mappedProduct = _mapper.Map<ProductDto>(product);
            return new ServiceResponse
            {
                Success = true,
                Message = "Get product by id success",
                Payload = product
            };
        }
    }
}
