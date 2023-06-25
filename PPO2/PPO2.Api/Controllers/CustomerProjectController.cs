using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PPO2.Core.Interfaces;
using PPO2.Core.Services;

namespace PPO2.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerProjectController : ControllerBase
    {
        private readonly ICustomerProjectService _customerProjectService;

        public CustomerProjectController(ICustomerProjectService customerProjectService)
        {
            _customerProjectService = customerProjectService;   
        }

        [HttpPost("create/customer-{customerId}/project-{projectId}")]
        public async Task<IActionResult> Create(int customerId, int projectId)
        {
            var result = await _customerProjectService.Create(customerId, projectId);
            return Ok(result);
        }
        [HttpDelete("delete/customer-{customerId}/project-{projectId}")]
        public async Task<IActionResult> DeleteCustomer(int customerId, int projectId)  // Видалення клієнта з проекту
        {
            var result = await _customerProjectService.DeleteCustomer(customerId, projectId);
            if (result.Success)
                return Ok(result);
            return BadRequest(result);
        }
    }
}
