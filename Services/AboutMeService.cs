using Microsoft.EntityFrameworkCore;
using portfolioApi.Data;
using portfolioApi.DTOs;
using portfolioApi.Models;
using portfolioApi.Services.Interfaces;

namespace portfolioApi.Services
{
    public class AboutMeService : IAboutMeService
    {
        private readonly AppDbContext _context;

        public AboutMeService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<ApiResponse<List<AboutMeResponseDto>>> GetAllAboutMeAsync()
        {
            var aboutMe = await _context.AboutMe
                .AsNoTracking()
                .OrderBy(item => item.Id)
                .Select(item => MapToResponse(item))
                .ToListAsync();

            return ApiResponse<List<AboutMeResponseDto>>.Ok(aboutMe, "Informacoes sobre mim listadas com sucesso.");
        }

        public async Task<ApiResponse<AboutMeResponseDto>> AddAboutMeAsync(CreateAboutMeDto createAboutMeDto)
        {
            var nameInPortfolio = createAboutMeDto.NameInPortfolio.Trim();

            var aboutMeAlreadyExists = await _context.AboutMe
                .AnyAsync(item => item.NameInPortfolio.ToLower() == nameInPortfolio.ToLower());

            if (aboutMeAlreadyExists)
            {
                return ApiResponse<AboutMeResponseDto>.Erro("Ja existe uma informacao sobre mim com esse nome.");
            }

            var aboutMe = new AboutMe
            {
                NameInPortfolio = nameInPortfolio,
                SubDescriptions = createAboutMeDto.SubDescriptions.Trim(),
                Profession = createAboutMeDto.Profession.Trim(),
                DescriptionAboutMe = createAboutMeDto.DescriptionAboutMe.Trim()
            };

            _context.AboutMe.Add(aboutMe);
            await _context.SaveChangesAsync();

            var response = MapToResponse(aboutMe);

            return ApiResponse<AboutMeResponseDto>.Ok(response, "Informacao sobre mim criada com sucesso.");
        }

        public async Task<ApiResponse<AboutMeResponseDto>> UpdateAboutMeAsync(int id, UpdateAboutMeDto updateAboutMeDto)
        {
            var aboutMe = await _context.AboutMe.FindAsync(id);

            if (aboutMe is null)
            {
                return ApiResponse<AboutMeResponseDto>.Erro("Informacao sobre mim nao encontrada.");
            }

            var nameInPortfolio = updateAboutMeDto.NameInPortfolio.Trim();

            var aboutMeAlreadyExists = await _context.AboutMe
                .AnyAsync(existingAboutMe =>
                    existingAboutMe.Id != id &&
                    existingAboutMe.NameInPortfolio.ToLower() == nameInPortfolio.ToLower());

            if (aboutMeAlreadyExists)
            {
                return ApiResponse<AboutMeResponseDto>.Erro("Ja existe uma informacao sobre mim com esse nome.");
            }

            aboutMe.NameInPortfolio = nameInPortfolio;
            aboutMe.SubDescriptions = updateAboutMeDto.SubDescriptions.Trim();
            aboutMe.Profession = updateAboutMeDto.Profession.Trim();
            aboutMe.DescriptionAboutMe = updateAboutMeDto.DescriptionAboutMe.Trim();

            await _context.SaveChangesAsync();

            var response = MapToResponse(aboutMe);

            return ApiResponse<AboutMeResponseDto>.Ok(response, "Informacao sobre mim atualizada com sucesso.");
        }

        public async Task<ApiResponse<bool>> DeleteAboutMeAsync(int id)
        {
            var aboutMe = await _context.AboutMe.FindAsync(id);

            if (aboutMe is null)
            {
                return ApiResponse<bool>.Erro("Informacao sobre mim nao encontrada.");
            }

            _context.AboutMe.Remove(aboutMe);
            await _context.SaveChangesAsync();

            return ApiResponse<bool>.Ok(true, "Informacao sobre mim excluida com sucesso.");
        }

        private static AboutMeResponseDto MapToResponse(AboutMe aboutMe)
        {
            return new AboutMeResponseDto
            {
                Id = aboutMe.Id,
                NameInPortfolio = aboutMe.NameInPortfolio,
                SubDescriptions = aboutMe.SubDescriptions,
                Profession = aboutMe.Profession,
                DescriptionAboutMe = aboutMe.DescriptionAboutMe
            };
        }
    }
}
