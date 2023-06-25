using AutoMapper;
using PPO2.Core.DTOs.ProjectDto;
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
    public class ProjectService : IProjectService
    {
        private readonly IRepository<Project> _projectRepo;
        private readonly IMapper _mapper;

        public ProjectService(IRepository<Project> projectRepo, IMapper mapper)
        {
            _projectRepo = projectRepo;
            _mapper = mapper;
        }

        public async Task<ServiceResponse> GetAllAsync()
        {
            try
            {
                var projects = await _projectRepo.GetListBySpec(new ProjectSpecification.GetAll());
                var mappedProjects = _mapper.Map<List<ProjectDto>>(projects);
                return new ServiceResponse
                {
                    Success = true,
                    Message = "Get All projects success",
                    Payload = mappedProjects
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
        public async Task<ServiceResponse> CreateAsync(ProjectCreateDto projectDto)
        {
            var mappedProject = _mapper.Map<Project>(projectDto);
            try
            {
                await _projectRepo.Insert(mappedProject);
                await _projectRepo.Save();
                return new ServiceResponse
                {
                    Success = true,
                    Message = "Project has been created successfully",
                    Payload = mappedProject
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

        public async Task<ServiceResponse> UpdateAsync(ProjectUpdateDto projectDto, int id)
        {
            var project = await _projectRepo.GetItemBySpec(new ProjectSpecification.GetById(id));
            if (project == null)
            {
                return new ServiceResponse
                {
                    Success = false,
                    Message = "Project id not found"
                };
            }
            project.Name = projectDto.Name;
            try
            {
                await _projectRepo.Update(project);
                await _projectRepo.Save();
                var allProjects = await GetAllAsync();
                return new ServiceResponse
                {
                    Success = true,
                    Message = "Project has been updated successfully",
                    Payload = allProjects.Payload
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
        public async Task<ServiceResponse> DeleteAsync(int id)
        {
            var project = await _projectRepo.GetByID(id);
            if (project == null)
            {
                return new ServiceResponse
                {
                    Success = false,
                    Message = "Project's id not found."
                };
            }
            try
            {
                await _projectRepo.Delete(project);
                await _projectRepo.Save();
                var allProjects = await GetAllAsync();
                return new ServiceResponse
                {
                    Success = true,
                    Message = "Project has been deleted.",
                    Payload = allProjects.Payload
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
            var project = await _projectRepo.GetItemBySpec(new ProjectSpecification.GetById(id));
            if (project == null)
            {
                return new ServiceResponse
                {
                    Success = false,
                    Message = $"Project by id {id} not found.",
                    Payload = null
                };
            }
            var mappedProject = _mapper.Map<ProjectDto>(project);
            return new ServiceResponse
            {
                Success = true,
                Message = $"Project has been found by id {id}",
                Payload = project
            };
        }
    }
}
