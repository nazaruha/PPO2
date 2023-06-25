using AutoMapper;
using PPO2.Core.DTOs.ProjectDto;
using PPO2.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PPO2.Core.AutoMapper
{
    public class AutoMapperProjectProfile : Profile
    {
        public AutoMapperProjectProfile()
        {
            CreateMap<Project, ProjectCreateDto>().ReverseMap();
            CreateMap<Project, ProjectUpdateDto>().ReverseMap();
            CreateMap<Project, ProjectDto>().ReverseMap();
        }
    }
}
