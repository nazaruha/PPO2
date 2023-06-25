using PPO2.Core.DTOs.CustomerDto;
using PPO2.Core.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PPO2.Core.Interfaces
{
    public interface ICustomerService
    {
        Task<ServiceResponse> GetAllAsync();
        Task<ServiceResponse> CreateAsync(CustomerCreateDto customerDto);
        Task<ServiceResponse> UpdateAsync(CustomerUpdateDto customerDto, int id);
        Task<ServiceResponse> DeleteAsync(int id);
        Task<ServiceResponse> GetById(int id);
        Task<ServiceResponse> GetByProjectId(int id);
    }
}
