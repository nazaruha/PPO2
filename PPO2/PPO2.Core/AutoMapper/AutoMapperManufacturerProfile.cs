using AutoMapper;
using PPO2.Core.DTOs.ManufacturerDto;
using PPO2.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PPO2.Core.AutoMapper
{
    public class AutoMapperManufacturerProfile : Profile
    {
        public AutoMapperManufacturerProfile()
        {
            CreateMap<Manufacturer, ManufacturerCreateDto>().ReverseMap();
            CreateMap<Manufacturer, ManufacturerDto>().ReverseMap();
            CreateMap<ManufacturerSearchDto, ManufacturerDto>().ReverseMap();
            CreateMap<ManufacturerSearchDto, Manufacturer>().ReverseMap();
            CreateMap<Manufacturer, ManufacturerUpdateDto>().ReverseMap();
        }
    }
}
