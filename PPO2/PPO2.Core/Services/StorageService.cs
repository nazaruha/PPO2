using AutoMapper;
using PPO2.Core.DTOs.Storage;
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
    public class StorageService : IStorageService
    {
        private readonly IRepository<Project> _projectRepo;
        private readonly IRepository<Product> _productRepo;
        private readonly IProjectService _projectService;
        private readonly IProductService _productService;
        private readonly IMapper _mapper;

        public StorageService(IRepository<Project> projectRepo, IRepository<Product> productRepo, IProjectService projectService, IProductService productService, IMapper mapper)
        {
            _projectRepo = projectRepo;
            _productRepo = productRepo;
            _projectService = projectService;
            _productService = productService;
            _mapper = mapper;
        }

        public async Task<ServiceResponse> ValidatePrimaryKeysAsync(int productId, int projectId)
        {
            var project = await _projectRepo.GetItemBySpec(new ProjectSpecification.GetById(projectId));
            if (project == null)
            {
                return new ServiceResponse
                {
                    Success = false,
                    Message = "Project id not found"
                };
            }

            var product = await _productRepo.GetItemBySpec(new ProductSpecification.GetById(productId));
            if (product == null)
            {
                return new ServiceResponse
                {
                    Success = false,
                    Message = "Product id not found"
                };
            }

            Storage storage = project.Storage.FirstOrDefault(s => s.ProjectId == project.Id && s.ProductId == product.Id);
            return new ServiceResponse
            {
                Success = true,
                Message = "Product and Project ids exists",
                Payload = storage
            };
        }

        public async Task<ServiceResponse> CreateAsync(StorageCreateDto storageDto)
        {
            var res = await ValidatePrimaryKeysAsync(storageDto.ProductId, storageDto.ProjectId);
            if (!res.Success)
            {
                return new ServiceResponse
                {
                    Success = false,
                    Message = res.Message
                };
            }

            var project = await _projectRepo.GetItemBySpec(new ProjectSpecification.GetById(storageDto.ProjectId));
            if (project == null)
            {
                return new ServiceResponse
                {
                    Success = false,
                    Message = "Project id not found"
                };
            }

            var product = await _productRepo.GetItemBySpec(new ProductSpecification.GetById(storageDto.ProductId));
            if (product == null)
            {
                return new ServiceResponse
                {
                    Success = false,
                    Message = "Product id not found"
                };
            }

            var mappedStorage = _mapper.Map<Storage>(storageDto);
            mappedStorage.Product = product;
            mappedStorage.Project = project;
            return new ServiceResponse
            {
                Success = true,
                Message = "GOOD",
                Payload = mappedStorage
            };
        }
    }
}
