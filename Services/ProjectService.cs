using Microsoft.EntityFrameworkCore;
using portfolioApi.Data;
using portfolioApi.DTOs;
using portfolioApi.Models;
using portfolioApi.Services.Interfaces;

namespace portfolioApi.Services
{
    public class ProjectService : IProjectService
    {
        private readonly AppDbContext _context;

        public ProjectService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<ApiResponse<List<ProjectResponseDto>>> GetAllProjectsAsync()
        {
            var projects = await _context.Projects
                .AsNoTracking()
                .OrderBy(project => project.Id)
                .Select(project => MapToResponse(project))
                .ToListAsync();

            return ApiResponse<List<ProjectResponseDto>>.Ok(projects, "Projetos listados com sucesso.");
        }

        public async Task<ApiResponse<ProjectResponseDto>> AddProjectAsync(CreateProjectDto createProjectDto)
        {
            var title = createProjectDto.TitleProject.Trim();

            var projectAlreadyExists = await _context.Projects
                .AnyAsync(project => project.TitleProject.ToLower() == title.ToLower());

            if (projectAlreadyExists)
            {
                return ApiResponse<ProjectResponseDto>.Erro("Ja existe um projeto com esse titulo.");
            }

            var project = new Projects
            {
                TitleProject = title,
                Description = createProjectDto.Description.Trim(),
                UrlImage = createProjectDto.UrlImage.Trim(),
                Status = createProjectDto.Status.Trim(),
                Technologies = createProjectDto.Technologies.Trim()
            };

            _context.Projects.Add(project);
            await _context.SaveChangesAsync();

            var response = MapToResponse(project);

            return ApiResponse<ProjectResponseDto>.Ok(response, $"Projeto criado com sucesso, atualmente ele estar {response.Status}");
        }

        public async Task<ApiResponse<ProjectResponseDto>> UpdateProjectAsync(int id, UpdateProjectDto updateProjectDto)
        {
            var project = await _context.Projects.FindAsync(id);

            if (project is null)
            {
                return ApiResponse<ProjectResponseDto>.Erro("Projeto nao encontrado.");
            }

            var title = updateProjectDto.TitleProject.Trim();

            var projectAlreadyExists = await _context.Projects
                .AnyAsync(existingProject =>
                    existingProject.Id != id &&
                    existingProject.TitleProject.ToLower() == title.ToLower());

            if (projectAlreadyExists)
            {
                return ApiResponse<ProjectResponseDto>.Erro("Ja existe um projeto com esse titulo.");
            }

            project.TitleProject = title;
            project.Description = updateProjectDto.Description.Trim();
            project.UrlImage = updateProjectDto.UrlImage.Trim();
            project.Status = updateProjectDto.Status.Trim();
            project.Technologies = updateProjectDto.Technologies.Trim();

            await _context.SaveChangesAsync();

            var response = MapToResponse(project);

            return ApiResponse<ProjectResponseDto>.Ok(response, "Projeto atualizado com sucesso.");
        }

        public async Task<ApiResponse<bool>> DeleteProjectAsync(int id)
        {
            var project = await _context.Projects.FindAsync(id);

            if (project is null)
            {
                return ApiResponse<bool>.Erro("Projeto nao encontrado.");
            }

            _context.Projects.Remove(project);
            await _context.SaveChangesAsync();

            return ApiResponse<bool>.Ok(true, "Projeto excluido com sucesso.");
        }

        private static ProjectResponseDto MapToResponse(Projects project)
        {
            return new ProjectResponseDto
            {
                Id = project.Id,
                TitleProject = project.TitleProject,
                Description = project.Description,
                UrlImage = project.UrlImage,
                Status = project.Status,
                Technologies = project.Technologies
            };
        }
    }
}
