using System.Threading;
using System.Threading.Tasks;
using portfolioApi.DTOs;

namespace portfolioApi.Services.Interfaces
{
    public interface IUserService
    {
        Task<AuthResponseDto> RegisterAsync(RegisterRequestDto request, CancellationToken cancellationToken = default);
        Task<AuthResponseDto> LoginAsync(LoginRequestDto request, CancellationToken cancellationToken = default);
    }
}
