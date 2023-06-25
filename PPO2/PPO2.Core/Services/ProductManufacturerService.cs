using AutoMapper;
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
    public class ProductManufacturerService
    {
        private readonly IRepository<Product> _productRepo;
        private readonly IRepository<Manufacturer> _manufacturerRepo;
        private readonly IMapper _mapper;

        public ProductManufacturerService(IRepository<Product> productRepo, IRepository<Manufacturer> manufacturerRepo, IMapper mapper)
        {
            _productRepo = productRepo;
            _manufacturerRepo = manufacturerRepo;
            _mapper = mapper;
        }
    }
}
