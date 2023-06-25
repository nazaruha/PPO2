using AutoMapper;
using PPO2.Core.DTOs.OrderDto;
using PPO2.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PPO2.Core.AutoMapper
{
    public class AutoMapperOrderProfile : Profile
    {
        public AutoMapperOrderProfile()
        {
            CreateMap<Order, OrderCreateDto>().ReverseMap();
            CreateMap<Order, OrderDto>().ReverseMap();
            CreateMap<Order, OrderUpdateDto>().ReverseMap();
            CreateMap<Order, OrderSearchDto>().ReverseMap();
            CreateMap<Order, OrderSearchDateDto>().ReverseMap();
            CreateMap<OrderDto, OrderSearchDateDto>().ReverseMap();
            CreateMap<OrderDto, OrderSearchDto>().ReverseMap();
        }
    }
}
