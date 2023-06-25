using AutoMapper;
using PPO2.Core.DTOs.PlanDto;
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
    public class PlanService : IPlanService
    {
        private readonly IRepository<Plan> _planRepo;
        private readonly IMapper _mapper;

        public PlanService(IRepository<Plan> planRepo, IMapper mapper)
        {
            _planRepo = planRepo;
            _mapper = mapper;
        }

        public async Task<ServiceResponse> GetAllAsync()
        {
            try
            {
                var plans = await _planRepo.GetListBySpec(new PlanSpecification.GetAll());
                var mappedPlans = _mapper.Map<List<PlanDto>>(plans);

                return new ServiceResponse
                {
                    Success = true,
                    Message = "Get All Plans success",
                    Payload = mappedPlans
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

        public async Task<ServiceResponse> CreateAsync(PlanCreateDto planDto)
        {
            try
            {
                var mappedPlan = _mapper.Map<Plan>(planDto);
                await _planRepo.Insert(mappedPlan);
                await _planRepo.Save();
                return new ServiceResponse
                {
                    Success = true,
                    Message = "Plan has been created successfully",
                    Payload = mappedPlan
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

        public async Task<ServiceResponse> UpdateAsync(PlanUpdateDto planDto)
        {
            var plan = await _planRepo.GetItemBySpec(new PlanSpecification.GetById(planDto.Id));

            if (plan == null)
            {
                return new ServiceResponse { Success = false, Message = "Plan's id not found" };
            }

            plan.Text = planDto.Text;
            plan.Date = planDto.Date;

            try
            {
                await _planRepo.Update(plan);
                await _planRepo.Save();

                var plans = await _planRepo.GetListBySpec(new PlanSpecification.GetAll());
                var mappedPlans = _mapper.Map<List<PlanDto>>(plans);

                return new ServiceResponse
                {
                    Success = true,
                    Message = $"Plan #{plan.Id} update succeess",
                    Payload = mappedPlans
                };
            } catch (Exception ex)
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
            var plan = await _planRepo.GetItemBySpec(new PlanSpecification.GetById(id));

            if (plan == null)
            {
                return new ServiceResponse
                {
                    Success = false,
                    Message = "Plan's id not found"
                };
            }

            try
            {
                await _planRepo.Delete(plan);
                await _planRepo.Save();

                var plans = await _planRepo.GetListBySpec(new PlanSpecification.GetAll());
                var mappedPlans = _mapper.Map<List<PlanDto>>(plans);

                return new ServiceResponse
                {
                    Success = true,
                    Message = $"Category #{id} has been deleted",
                    Payload = mappedPlans
                };
            } catch (Exception ex)
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
