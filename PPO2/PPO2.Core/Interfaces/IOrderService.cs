using PPO2.Core.DTOs.OrderDto;
using PPO2.Core.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PPO2.Core.Interfaces
{
    public interface IOrderService
    {
        Task<ServiceResponse> CreateAsync(OrderCreateDto orderDto);
    }
}
