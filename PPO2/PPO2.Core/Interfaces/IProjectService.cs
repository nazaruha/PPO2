using PPO2.Core.DTOs.ProjectDto;
using PPO2.Core.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PPO2.Core.Interfaces
{
    public interface IProjectService
    {
        Task<ServiceResponse> GetAllAsync();
        Task<ServiceResponse> CreateAsync(ProjectCreateDto projectDto);
        Task<ServiceResponse> UpdateAsync(ProjectUpdateDto projectDto, int id);
        Task<ServiceResponse> DeleteAsync(int id);
        Task<ServiceResponse> GetById(int id);
    }
}
