using AutoMapper;
using PPO2.Core.DTOs.CustomerDto;
using PPO2.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PPO2.Core.AutoMapper
{
    public class AutoMapperCustomerProfile : Profile
    {
        public AutoMapperCustomerProfile() 
        {
            CreateMap<Customer, CustomerCreateDto>().ReverseMap();
            CreateMap<Customer, CustomerUpdateDto>().ReverseMap();
            CreateMap<Customer, CustomerDto>().ReverseMap();
            CreateMap<Customer, CustomerSearchDto>().ReverseMap();
            CreateMap<CustomerDto, CustomerSearchDto>().ReverseMap();
        }
    }
}
