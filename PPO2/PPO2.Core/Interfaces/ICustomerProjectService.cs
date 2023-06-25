using PPO2.Core.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PPO2.Core.Interfaces
{
    public interface ICustomerProjectService
    {
        Task<ServiceResponse> Create(int customerId, int projectId);
        Task<ServiceResponse> DeleteCustomer(int customerId, int projectId);
    }
}
