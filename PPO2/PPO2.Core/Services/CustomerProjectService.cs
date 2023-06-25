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
    public class CustomerProjectService : ICustomerProjectService
    {
        private readonly IRepository<Customer> _customerRepo;
        private readonly IRepository<Project> _projectRepo;
        private readonly IMapper _mapper;

        public CustomerProjectService(IRepository<Customer> customerRepos, IRepository<Project> projectRepo, IMapper mapper)
        {
            _customerRepo = customerRepos;
            _projectRepo = projectRepo;
            _mapper = mapper;
        }

        public async Task<ServiceResponse> Create(int customerId, int projectId)
        {
            var project = await _projectRepo.GetItemBySpec(new ProjectSpecification.GetById(projectId));
            var customer = await _customerRepo.GetItemBySpec(new CustomerSpecification.GetById(customerId));
            if (project == null || customer == null)
            {
                return new ServiceResponse
                {
                    Success = false,
                    Message = "Customer's or Project's id doesn't exist"
                };
            }

            try
            {
                project.Customers.Add(customer);
                await _projectRepo.Save();
                await _customerRepo.Save();
                var mappedProject = _mapper.Map<ProjectDto>(project);
                return new ServiceResponse
                {
                    Success = true,
                    Message = $"Customer#{customer.Id} has been added to the project#{project.Id}",
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

        public async Task<ServiceResponse> DeleteCustomer(int customerId, int projectId)
        {
            var project = await _projectRepo.GetItemBySpec(new ProjectSpecification.GetById(projectId));
            var customer = await _customerRepo.GetItemBySpec(new CustomerSpecification.GetById(customerId));
            if (project == null || customer == null)
            {
                return new ServiceResponse
                {
                    Success = false,
                    Message = "Customer's or Project's id doesn't exist"
                };
            }

            try
            {
                project.Customers.Remove(customer);
                await _projectRepo.Save();
                await _customerRepo.Save();
                return new ServiceResponse
                {
                    Success = true,
                    Message = $"Customer #{customerId} has been delete from the project \'{project.Name}\' #{projectId}"
                };
            } catch(Exception ex)
            {
                return new ServiceResponse
                {
                    Success = false,
                    Message = ex.InnerException.Message
                };
            }
        }
    }
}
