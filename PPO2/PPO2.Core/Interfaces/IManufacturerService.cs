using PPO2.Core.DTOs.ManufacturerDto;
using PPO2.Core.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PPO2.Core.Interfaces
{
    public interface IManufacturerService
    {
        Task<ServiceResponse> GetAllAsync();
        Task<ServiceResponse> CreateAsync(ManufacturerCreateDto manufacturerDto);
        Task<ServiceResponse> UpdateAsync(ManufacturerUpdateDto manufacturerDto);
        Task<ServiceResponse> DeleteAsync(int id);
        Task<ServiceResponse> GetById(int id);
    }
}
