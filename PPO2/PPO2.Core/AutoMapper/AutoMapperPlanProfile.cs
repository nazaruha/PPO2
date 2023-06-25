using AutoMapper;
using PPO2.Core.DTOs.PlanDto;
using PPO2.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PPO2.Core.AutoMapper
{
    public class AutoMapperPlanProfile : Profile
    {
        public AutoMapperPlanProfile()
        {
            CreateMap<Plan, PlanCreateDto>().ReverseMap();
            CreateMap<Plan, PlanDto>().ReverseMap();
            CreateMap<Plan, PlanUpdateDto>().ReverseMap();
        }
    }
}
