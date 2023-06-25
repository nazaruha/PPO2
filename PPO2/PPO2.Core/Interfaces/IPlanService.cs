using PPO2.Core.DTOs.PlanDto;
using PPO2.Core.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PPO2.Core.Interfaces
{
    public interface IPlanService
    {
        Task<ServiceResponse> GetAllAsync();
        Task<ServiceResponse> CreateAsync(PlanCreateDto planDto);
        Task<ServiceResponse> UpdateAsync(PlanUpdateDto planDto);
        Task<ServiceResponse> DeleteAsync(int id);
    }
}
