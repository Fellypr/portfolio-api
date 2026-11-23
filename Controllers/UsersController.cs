using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using portfolioApi.DTOs;
using portfolioApi.Services.Interfaces;

namespace portfolioApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly ILogger<UsersController> _logger;

        public UsersController(IUserService userService, ILogger<UsersController> logger)
        {
            _userService = userService;
            _logger = logger;
        }

        [HttpPost("register")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(AuthResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(AuthResponseDto), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status409Conflict)]
        [ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Register([FromBody] RegisterRequestDto request, CancellationToken cancellationToken)
        {
            if (request == null)
            {
                return BadRequest(new ApiResponse<string>
                {
                    Sucesso = false,
                    Mensagem = "Corpo da requisição não pode ser nulo."
                });
            }

            if (!ModelState.IsValid)
            {
                var erros = ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage)
                    .ToList();

                return BadRequest(new ApiResponse<string>
                {
                    Sucesso = false,
                    Mensagem = erros.FirstOrDefault() ?? "Dados de registro inválidos."
                });
            }

            try
            {
                var result = await _userService.RegisterAsync(request, cancellationToken);
                return CreatedAtAction(nameof(Login), new { username = result.Username }, result);
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogWarning("Tentativa de registro duplicado: {Message}", ex.Message);
                return Conflict(new ApiResponse<string>
                {
                    Sucesso = false,
                    Mensagem = ex.Message
                });
            }
            catch (ArgumentException ex)
            {
                _logger.LogWarning("Falha de validação no registro: {Message}", ex.Message);
                return BadRequest(new ApiResponse<string>
                {
                    Sucesso = false,
                    Mensagem = ex.Message
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro interno ao processar o registro do usuário.");
                return StatusCode(StatusCodes.Status500InternalServerError, new ApiResponse<string>
                {
                    Sucesso = false,
                    Mensagem = "Ocorreu um erro interno ao processar a solicitação."
                });
            }
        }

        [HttpPost("login")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(AuthResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Login([FromBody] LoginRequestDto request, CancellationToken cancellationToken)
        {
            if (request == null)
            {
                return BadRequest(new ApiResponse<string>
                {
                    Sucesso = false,
                    Mensagem = "Corpo da requisição não pode ser nulo."
                });
            }

            if (!ModelState.IsValid)
            {
                var erros = ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage)
                    .ToList();

                return BadRequest(new ApiResponse<string>
                {
                    Sucesso = false,
                    Mensagem = erros.FirstOrDefault() ?? "Dados de autenticação inválidos."
                });
            }

            try
            {
                var result = await _userService.LoginAsync(request, cancellationToken);
                return Ok(result);
            }
            catch (UnauthorizedAccessException ex)
            {
                _logger.LogWarning("Falha de autenticação para o usuário '{Username}': {Message}", request?.Username, ex.Message);
                return Unauthorized(new ApiResponse<string>
                {
                    Sucesso = false,
                    Mensagem = ex.Message
                });
            }
            catch (ArgumentException ex)
            {
                _logger.LogWarning("Requisição de login inválida: {Message}", ex.Message);
                return BadRequest(new ApiResponse<string>
                {
                    Sucesso = false,
                    Mensagem = ex.Message
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro interno ao processar login do usuário.");
                return StatusCode(StatusCodes.Status500InternalServerError, new ApiResponse<string>
                {
                    Sucesso = false,
                    Mensagem = "Ocorreu um erro interno ao processar a solicitação."
                });
            }
        }

        [HttpGet("me")]
        [Authorize]
        [ProducesResponseType(typeof(UserResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public IActionResult GetCurrentUser()
        {
            var username = User.Identity?.Name;
            if (string.IsNullOrEmpty(username))
            {
                return Unauthorized(new ApiResponse<string>
                {
                    Sucesso = false,
                    Mensagem = "Usuário não autenticado."
                });
            }

            var email = User.FindFirst(System.Security.Claims.ClaimTypes.Email)?.Value ?? string.Empty;

            return Ok(new UserResponseDto
            {
                Username = username,
                Email = email
            });
        }
    }
}
