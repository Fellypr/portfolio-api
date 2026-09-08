using Microsoft.AspNetCore.Mvc;
using portfolioApi.DTOs;
using portfolioApi.Services.Interfaces;

namespace portfolioApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProjectsController : ControllerBase
    {
        private readonly IProjectService _projectService;

        public ProjectsController(IProjectService projectService)
        {
            _projectService = projectService;
        }

        [HttpGet("GetAllProjects")]
        public async Task<ActionResult<ApiResponse<List<ProjectResponseDto>>>> GetAllProjects()
        {
            var response = await _projectService.GetAllProjectsAsync();
            return Ok(response);
        }

        [HttpPost("AddProject")]
        public async Task<ActionResult<ApiResponse<ProjectResponseDto>>> AddProject(CreateProjectDto createProjectDto)
        {
            var response = await _projectService.AddProjectAsync(createProjectDto);
            if (!response.Sucesso)
            {
                return BadRequest(response);
            }
            return StatusCode(201, response);
        }

        [HttpPut("UpdateProject/{id}")]
        public async Task<ActionResult<ApiResponse<ProjectResponseDto>>> UpdateProject(int id, UpdateProjectDto updateProjectDto)
        {
            var response = await _projectService.UpdateProjectAsync(id, updateProjectDto);
            if (!response.Sucesso)
            {
                return BadRequest(response);
            }

            return Ok(response);
        }

        [HttpDelete("DeleteProject/{id}")]
        public async Task<ActionResult<ApiResponse<bool>>> DeleteProject(int id)
        {
            var response = await _projectService.DeleteProjectAsync(id);
            if (!response.Sucesso)
            {
                return BadRequest(response);
            }

            return Ok(response);
        }
    }
}
