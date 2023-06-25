using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PPO2.Core.DTOs.ProjectDto;
using PPO2.Core.Interfaces;
using PPO2.Core.Services;
using PPO2.Core.Validation.Project;

namespace PPO2.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProjectController : ControllerBase
    {
        private readonly IProjectService _projectService;

        public ProjectController(IProjectService projectService)
        {
            _projectService = projectService;
        }
        [HttpGet("projects")]
        public async Task<IActionResult> Index()
        {
            var result = await _projectService.GetAllAsync();
            return Ok(result);
        }

        [HttpPost("create")]
        public async Task<IActionResult> Create([FromBody] ProjectCreateDto request)
        {
            var validation = new ProjectCreateValidation();
            var validationResult = await validation.ValidateAsync(request);

            if (validationResult.IsValid)
            {
                var result = await _projectService.CreateAsync(request);
                return Ok(result);
            }
            return BadRequest(validationResult.Errors);
        }

        [HttpPut("update/{id}")]
        public async Task<IActionResult> Update([FromBody] ProjectUpdateDto request, int id)
        {
            var validation = new ProjectUpdateValidation();
            var validationResult = await validation.ValidateAsync(request);
            if (validationResult.IsValid)
            {
                var result = await _projectService.UpdateAsync(request, id);
                return Ok(result);
            }
            return BadRequest(validationResult.Errors);
        }

        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            ServiceResponse result = new ServiceResponse();
            try
            {
                result = await _projectService.DeleteAsync(id);
                return Ok(result);
            }
            catch
            {
                return BadRequest(result);
            }
        }
        [HttpGet("project/{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            ServiceResponse result = new ServiceResponse();
            try
            {
                result = await _projectService.GetById(id);
                return Ok(result);
            }
            catch
            {
                return BadRequest(result);
            }
        }
    }
}
