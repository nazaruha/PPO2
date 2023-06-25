using AutoMapper;
using PPO2.Core.DTOs.CustomerDto;
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
    public class CustomerService : ICustomerService
    {
        private readonly IRepository<Customer> _customerRepo;
        private readonly IMapper _mapper;

        public CustomerService( IRepository<Customer> customerRepo, IMapper mapper)
        {
            _customerRepo = customerRepo;
            _mapper = mapper;
        }


        public async Task<ServiceResponse> GetAllAsync()
        {
            try
            {
                var customers = await _customerRepo.GetListBySpec(new CustomerSpecification.GetAll());
                var mappedCustomers = _mapper.Map<List<CustomerDto>>(customers);
                return new ServiceResponse
                {
                    Success = true,
                    Message = "Get All Customers",
                    Payload = mappedCustomers
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
        public async Task<ServiceResponse> CreateAsync(CustomerCreateDto customerDto)
        {
            var mappedCustomer = _mapper.Map<Customer>(customerDto);
            try
            {
                var customerByEmail = await _customerRepo.GetItemBySpec(new CustomerSpecification.GetByEmail(mappedCustomer.Email));
                if (customerByEmail != null)
                {
                    return new ServiceResponse
                    {
                        Success = false,
                        Message = "Пошта вже зайнята"
                    };
                }
                var customerByPhone = await _customerRepo.GetItemBySpec(new CustomerSpecification.GetByPhone(mappedCustomer.Phone));
                if (customerByPhone != null)
                {
                    return new ServiceResponse
                    {
                        Success = false,
                        Message = "Телефон вже зайнятий"
                    };
                }
                await _customerRepo.Insert(mappedCustomer);
                await _customerRepo.Save();
                return new ServiceResponse
                {
                    Success = true,
                    Message = "Customer has been created",
                    Payload = mappedCustomer
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

        public async Task<ServiceResponse> UpdateAsync(CustomerUpdateDto customerDto, int id)
        {
            var customer = await _customerRepo.GetItemBySpec(new CustomerSpecification.GetById(id));
            if (customer == null)
            {
                return new ServiceResponse
                {
                    Success = false,
                    Message = "Project id not found"
                };
            }
            var customerByEmail = await _customerRepo.GetItemBySpec(new CustomerSpecification.GetByEmail(customerDto.Email));
            if (customerByEmail != null && customerByEmail.Id != id)
            {
                return new ServiceResponse
                {
                    Success = false,
                    Message = "Пошта вже зайнята"
                };
            }
            var customerByPhone = await _customerRepo.GetItemBySpec(new CustomerSpecification.GetByPhone(customerDto.Phone));
            if (customerByPhone != null && customerByPhone.Id != id)
            {
                return new ServiceResponse
                {
                    Success = false,
                    Message = "Телефон вже зайнятий"
                };
            }
            customer.FirstName = customerDto.FirstName;
            customer.SecondName = customerDto.SecondName;
            customer.Phone = customerDto.Phone;
            customer.Email = customerDto.Email;
            customer.Address = customerDto.Address;
            // update orders and aybe projects
            try
            {
                await _customerRepo.Update(customer);
                await _customerRepo.Save();
                var allCustomers = await GetAllAsync();
                return new ServiceResponse
                {
                    Success = true,
                    Message = $"Customer #{id} has been updated successfully",
                    Payload = allCustomers
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
            var customer = await _customerRepo.GetByID(id);
            if (customer == null)
            {
                return new ServiceResponse
                {
                    Success = false,
                    Message = "Customer id not found."
                };
            }
            try
            {
                await _customerRepo.Delete(customer);
                await _customerRepo.Save();
                var allCustomers = await GetAllAsync();
                return new ServiceResponse
                {
                    Success = true,
                    Message = $"Customer #{customer.Id} has been deleted successfully",
                    Payload = allCustomers
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
            try
            {
                var customer = await _customerRepo.GetItemBySpec(new CustomerSpecification.GetById(id));
                var mappedCustomer = _mapper.Map<CustomerDto>(customer);
                return new ServiceResponse
                {
                    Success = true,
                    Message = "Customer get by id success",
                    Payload = customer
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

        public async Task<ServiceResponse> GetByProjectId(int id)
        {
            try
            {
                var customers = await _customerRepo.GetListBySpec(new CustomerSpecification.GetAll());
                List<Customer> customersByProject = new List<Customer>();
                foreach (var customer in customers)
                {
                    var projects = customer.Projects;
                    if (projects.FirstOrDefault(p => p.Id == id) != null)
                    {
                        customersByProject.Add(customer);
                    }
                }
                return new ServiceResponse
                {
                    Success = true,
                    Message = "Customer by Project Id has been found",
                    Payload = customersByProject
                };
            }
            catch(Exception ex)
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
