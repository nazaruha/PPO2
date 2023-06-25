using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PPO2.Core.DTOs.PlanDto;
using PPO2.Core.Interfaces;
using PPO2.Core.Validation.Plan;

namespace PPO2.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PlanController : ControllerBase
    {
        private readonly IPlanService _planService;

        public PlanController(IPlanService planService)
        {
            _planService = planService;
        }

        [HttpGet("index")]
        public async Task<IActionResult> Index()
        {
            var result = await _planService.GetAllAsync();
            return Ok(result);
        }

        [HttpPost("create")]
        public async Task<IActionResult> Create([FromBody] PlanCreateDto request)
        {
            var validation = new PlanCreateValidation();
            var validationResult = await validation.ValidateAsync(request);
            if (validationResult.IsValid)
            {
                var result = await _planService.CreateAsync(request);
                return Ok(result);
            }
            return BadRequest(validationResult.Errors);
        }

        [HttpPut("update/{id}")]
        public async Task<IActionResult> Upate([FromBody] PlanUpdateDto request)
        {
            var validation = new PlanUpdateValidation();
            var validationResult = await validation.ValidateAsync(request);

            if (validationResult.IsValid)
            {
                var result = await _planService.UpdateAsync(request);
                return Ok(result);
            }
            return BadRequest(validationResult.Errors);
        }

        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var result = await _planService.DeleteAsync(id);
                return Ok(result);  
            } catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
