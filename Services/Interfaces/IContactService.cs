using portfolioApi.DTOs;

namespace portfolioApi.Services.Interfaces
{
    public interface IContactService
    {
        Task<ApiResponse<List<ContactResponseDto>>> GetAllContactsAsync();
        Task<ApiResponse<ContactResponseDto>> AddContactAsync(CreateContactDto createContactDto);
        Task<ApiResponse<ContactResponseDto>> UpdateContactAsync(int id, UpdateContactDto updateContactDto);
        Task<ApiResponse<bool>> DeleteContactAsync(int id);
    }
}
