using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PPO2.Core.Interfaces;

namespace PPO2.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PlanProjectController : ControllerBase
    {
        private readonly IPlanProjectService _planProjectService;

        public PlanProjectController(IPlanProjectService planProjectService)
        {
            _planProjectService = planProjectService;
        }

        [HttpPost("create/plan-{planId}/project-{projectId}")]
        public async Task<IActionResult> Create(int planId, int projectId)
        {
            var result = await _planProjectService.CreateAsync(planId, projectId);
            return Ok (result);
        }
    }
}
