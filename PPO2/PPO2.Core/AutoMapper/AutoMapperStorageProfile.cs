using AutoMapper;
using PPO2.Core.DTOs.Storage;
using PPO2.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PPO2.Core.AutoMapper
{
    internal class AutoMapperStorageProfile : Profile
    {
        public AutoMapperStorageProfile()
        {
            CreateMap<Storage, StorageCreateDto>().ReverseMap();
            CreateMap<Storage, StorageDto>().ReverseMap();
            CreateMap<Storage, StorageSearchDto>().ReverseMap();
            CreateMap<StorageDto, StorageUpdateDto>().ReverseMap();
            CreateMap<StorageDto, StorageSearchDto>().ReverseMap();
            CreateMap<Storage, StorageUpdateDto>().ReverseMap();
        }
    }
}
