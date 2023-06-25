using PPO2.Core.DTOs.ProductDto;
using PPO2.Core.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PPO2.Core.Interfaces
{
    public interface IProductManufacturerService
    {
        Task<ServiceResponse> Create(int manufacturerId, ProductCreateDto productDto);
        Task<ServiceResponse> GetByManufacturerId(int manufacturerId);
    }
}
