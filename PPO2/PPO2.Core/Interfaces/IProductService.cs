using PPO2.Core.DTOs.ProductDto;
using PPO2.Core.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PPO2.Core.Interfaces
{
    public interface IProductService
    {
        Task<ServiceResponse> GetAllAsync();
        Task<ServiceResponse> CreateAsync(ProductCreateDto productDto);
        Task<ServiceResponse> UpdateAsync(ProductUpdateDto productDto);
        Task<ServiceResponse> DeleteAsync(int productId, int manufacturerId);
        Task<ServiceResponse> GetById(int id);
    }
}
