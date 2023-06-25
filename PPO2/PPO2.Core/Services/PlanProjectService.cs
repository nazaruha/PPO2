using AutoMapper;
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
    public class PlanProjectService : IPlanProjectService
    {
        private readonly IRepository<Plan> _planRepo;
        private readonly IRepository<Project> _projectRepo;
        private readonly IMapper _mapper;

        public PlanProjectService(IRepository<Plan> planRepo, IRepository<Project> projectRepo, IMapper mapper)
        {
            _planRepo = planRepo;
            _projectRepo = projectRepo;
            _mapper = mapper;
        }

        public async Task<ServiceResponse> CreateAsync(int planId, int projectId)
        {
            var plan = await _planRepo.GetItemBySpec(new PlanSpecification.GetById(planId));
            var project = await _projectRepo.GetItemBySpec(new ProjectSpecification.GetById(projectId));

            if (plan == null || project == null)
            {
                return new ServiceResponse
                {
                    Success = false,
                    Message = "Plan's or Project's id doesn't exist"
                };
            }

            try
            {
                project.Plans.Add(plan);

                await _projectRepo.Save();
                await _planRepo.Save();

                return new ServiceResponse
                {
                    Success = true,
                    Message = $"PlanProject [#{planId};#{projectId}] relation has been created"
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
    }
}
