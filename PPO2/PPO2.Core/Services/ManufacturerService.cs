using AutoMapper;
using PPO2.Core.DTOs.ManufacturerDto;
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
    public class ManufacturerService : IManufacturerService
    {
        private readonly IRepository<Manufacturer> _manufacturerRepo;
        private readonly IMapper _mapper;

        public ManufacturerService(IRepository<Manufacturer> manufacturerRepo, IMapper mapper)
        {
            _manufacturerRepo = manufacturerRepo;
            _mapper = mapper;
        }

        public async Task<ServiceResponse> GetAllAsync()
        {
            try
            {
                var manufacturers = await _manufacturerRepo.GetListBySpec(new ManufacturerSpecification.GetAll());
                var mappedManufacturers = _mapper.Map<List<ManufacturerDto>>(manufacturers);

                return new ServiceResponse
                {
                    Success = true,
                    Message = "Get All Manufacturers",
                    Payload = mappedManufacturers
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

        public async Task<ServiceResponse> CreateAsync(ManufacturerCreateDto manufacturerDto)
        {
            var manufacturer = await _manufacturerRepo.GetItemBySpec(new ManufacturerSpecification.GetByName(manufacturerDto.Name));
            if (manufacturer != null)
            {
                return new ServiceResponse
                {
                    Success = false,
                    Message = "Назва продукту вже зайнята"
                };
            }

            try
            {
                var mappedManufacturer = _mapper.Map<Manufacturer>(manufacturerDto);
                await _manufacturerRepo.Insert(mappedManufacturer);
                await _manufacturerRepo.Save();

                return new ServiceResponse
                {
                    Success = true,
                    Message = "Manufacturer has been created",
                    Payload = mappedManufacturer
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

        public async Task<ServiceResponse> UpdateAsync(ManufacturerUpdateDto manufacturerDto)
        {
            var manufacturer = await _manufacturerRepo.GetItemBySpec(new ManufacturerSpecification.GetByName(manufacturerDto.Name));
            if (manufacturer != null && manufacturer.Id != manufacturerDto.Id)
            {
                return new ServiceResponse
                {
                    Success = false,
                    Message = "Такий виробник вже існує"
                };
            }

            manufacturer = await _manufacturerRepo.GetByID(manufacturerDto.Id);
            if (manufacturer == null)
            {
                return new ServiceResponse
                {
                    Success = false,
                    Message = "Manufacturer's id doesn't exist"
                };
            }

            manufacturer.Name = manufacturerDto.Name;

            try
            {
                await _manufacturerRepo.Update(manufacturer);
                await _manufacturerRepo.Save();

                return new ServiceResponse
                {
                    Success = true,
                    Message = $"Manufacturer #{manufacturer.Id} has been updated",
                    Payload = manufacturer
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
            var manufacturer = await _manufacturerRepo.GetByID(id);
            if (manufacturer == null)
            {
                return new ServiceResponse
                {
                    Success = false,
                    Message = "Manufacturer's id not found"
                };
            }

            try
            {
                await _manufacturerRepo.Delete(manufacturer);
                await _manufacturerRepo.Save();

                var manufacturers = await _manufacturerRepo.GetListBySpec(new ManufacturerSpecification.GetAll());
                var mappedManufacturers = _mapper.Map<List<ManufacturerDto>>(manufacturers);

                return new ServiceResponse
                {
                    Success = true,
                    Message = $"Manufacturer {id} has been deleted",
                    Payload = mappedManufacturers
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
            var manufacturer = await _manufacturerRepo.GetItemBySpec(new ManufacturerSpecification.GetById(id));
            if (manufacturer == null)
            {
                return new ServiceResponse
                {
                    Success = false,
                    Message = "Manufacturer's id doesn't exist"
                };
            }

            var mappedManufacturer = _mapper.Map<ManufacturerDto>(manufacturer);
            return new ServiceResponse
            {
                Success = true,
                Message = $"Manufacturer #{id} has been found",
                Payload = mappedManufacturer
            };
        }
    }
}
