using portfolioApi.DTOs;

namespace portfolioApi.Services.Interfaces
{
    public interface IAboutMeService
    {
        Task<ApiResponse<List<AboutMeResponseDto>>> GetAllAboutMeAsync();
        Task<ApiResponse<AboutMeResponseDto>> AddAboutMeAsync(CreateAboutMeDto createAboutMeDto);
        Task<ApiResponse<AboutMeResponseDto>> UpdateAboutMeAsync(int id, UpdateAboutMeDto updateAboutMeDto);
        Task<ApiResponse<bool>> DeleteAboutMeAsync(int id);
    }
}
