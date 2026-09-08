using portfolioApi.DTOs;

namespace portfolioApi.Services.Interfaces
{
    public interface IProjectService
    {
        Task<ApiResponse<List<ProjectResponseDto>>> GetAllProjectsAsync();
        Task<ApiResponse<ProjectResponseDto>> AddProjectAsync(CreateProjectDto createProjectDto);
        Task<ApiResponse<ProjectResponseDto>> UpdateProjectAsync(int id, UpdateProjectDto updateProjectDto);
        Task<ApiResponse<bool>> DeleteProjectAsync(int id);
    }
}
