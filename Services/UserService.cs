using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using portfolioApi.Data;
using portfolioApi.DTOs;
using portfolioApi.Models;
using portfolioApi.Services.Interfaces;

namespace portfolioApi.Services
{
    public class UserService : IUserService
    {
        private static readonly SemaphoreSlim _registrationLock = new(1, 1);
        private const int WorkFactor = 11;

        private readonly AppDbContext _context;
        private readonly IConfiguration _configuration;

        public UserService(AppDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        public async Task<AuthResponseDto> RegisterAsync(RegisterRequestDto request, CancellationToken cancellationToken = default)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            if (string.IsNullOrWhiteSpace(request.Username))
            {
                throw new ArgumentException("O nome de usuário é obrigatório.");
            }

            if (string.IsNullOrWhiteSpace(request.Email))
            {
                throw new ArgumentException("O e-mail é obrigatório.");
            }

            if (string.IsNullOrWhiteSpace(request.Password))
            {
                throw new ArgumentException("A senha é obrigatória.");
            }

            var normalizedUsername = request.Username.Trim();
            var normalizedEmail = request.Email.Trim();

            await _registrationLock.WaitAsync(cancellationToken);
            try
            {
                var exists = await _context.Users
                    .AnyAsync(u => u.Username.ToLower() == normalizedUsername.ToLower(), cancellationToken);

                if (exists)
                {
                    throw new InvalidOperationException($"Nome de usuário já está em uso: '{normalizedUsername}'.");
                }

                var emailExists = await _context.Users
                    .AnyAsync(u => u.Email.ToLower() == normalizedEmail.ToLower(), cancellationToken);

                if (emailExists)
                {
                    throw new InvalidOperationException($"E-mail já está em uso: '{normalizedEmail}'.");
                }

                var passwordHash = HashPassword(request.Password);

                var user = new User
                {
                    Username = normalizedUsername,
                    Email = normalizedEmail,
                    PasswordHash = passwordHash
                };

                await _context.Users.AddAsync(user, cancellationToken);
                await _context.SaveChangesAsync(cancellationToken);

                var expiresAt = DateTime.UtcNow.AddHours(4);
                var token = GenerateJwtToken(user, expiresAt);

                return new AuthResponseDto
                {
                    Token = token,
                    Username = user.Username,
                    Email = user.Email,
                    ExpiresAt = expiresAt
                };
            }
            finally
            {
                _registrationLock.Release();
            }
        }

        public async Task<AuthResponseDto> LoginAsync(LoginRequestDto request, CancellationToken cancellationToken = default)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            if (string.IsNullOrWhiteSpace(request.Username) || string.IsNullOrWhiteSpace(request.Password))
            {
                throw new ArgumentException("Usuário e senha são obrigatórios.");
            }

            var normalizedUsername = request.Username.Trim();

            var user = await _context.Users
                .SingleOrDefaultAsync(u => u.Username.ToLower() == normalizedUsername.ToLower() || u.Email.ToLower() == normalizedUsername.ToLower(), cancellationToken);

            if (user == null || !VerifyPassword(request.Password, user.PasswordHash))
            {
                throw new UnauthorizedAccessException("Credenciais inválidas. Verifique o usuário e a senha.");
            }

            // Se o hash armazenado estiver no formato legado (salt:hash), atualiza para BCrypt
            if (!string.IsNullOrWhiteSpace(user.PasswordHash) && !user.PasswordHash.StartsWith("$2"))
            {
                user.PasswordHash = HashPassword(request.Password);
                await _context.SaveChangesAsync(cancellationToken);
            }

            var expiresAt = DateTime.UtcNow.AddHours(4);
            var token = GenerateJwtToken(user, expiresAt);

            return new AuthResponseDto
            {
                Token = token,
                Username = user.Username,
                Email = user.Email,
                ExpiresAt = expiresAt
            };
        }

        private static string HashPassword(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password, WorkFactor);
        }

        private static bool VerifyPassword(string password, string storedHash)
        {
            if (string.IsNullOrWhiteSpace(storedHash))
            {
                return false;
            }

            // Suporte para hash gerado por BCrypt (formato padrão $2a$, $2b$, etc.)
            if (storedHash.StartsWith("$2"))
            {
                try
                {
                    return BCrypt.Net.BCrypt.Verify(password, storedHash);
                }
                catch
                {
                    return false;
                }
            }

            // Suporte legado para hashes manuais no formato salt:hash
            var parts = storedHash.Split(':');
            if (parts.Length != 2)
            {
                return false;
            }

            try
            {
                byte[] salt = Convert.FromBase64String(parts[0]);
                byte[] expectedHash = Convert.FromBase64String(parts[1]);

                byte[] actualHash = Rfc2898DeriveBytes.Pbkdf2(
                    password,
                    salt,
                    100000,
                    HashAlgorithmName.SHA256,
                    expectedHash.Length);

                return CryptographicOperations.FixedTimeEquals(actualHash, expectedHash);
            }
            catch (FormatException)
            {
                return false;
            }
        }

        private string GenerateJwtToken(User user, DateTime expiresAt)
        {
            var jwtKey = _configuration["Jwt:Key"] ?? "PortfolioSuperSecretKey2025ChangeMeInProductionMinimum32Bytes!";
            var jwtIssuer = _configuration["Jwt:Issuer"] ?? "portfolio-api";
            var jwtAudience = _configuration["Jwt:Audience"] ?? "portfolio-client";

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Username),
                new Claim("unique_name", user.Username),
                new Claim("name", user.Username),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim("email", user.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var token = new JwtSecurityToken(
                issuer: jwtIssuer,
                audience: jwtAudience,
                claims: claims,
                notBefore: DateTime.UtcNow.AddMinutes(-5),
                expires: expiresAt,
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
