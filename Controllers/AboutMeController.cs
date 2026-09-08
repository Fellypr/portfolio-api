using Microsoft.AspNetCore.Mvc;
using portfolioApi.DTOs;
using portfolioApi.Services.Interfaces;

namespace portfolioApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AboutMeController : ControllerBase
    {
        private readonly IAboutMeService _aboutMeService;

        public AboutMeController(IAboutMeService aboutMeService)
        {
            _aboutMeService = aboutMeService;
        }

        [HttpGet("GetAllAboutMe")]
        public async Task<ActionResult<ApiResponse<List<AboutMeResponseDto>>>> GetAllAboutMe()
        {
            var response = await _aboutMeService.GetAllAboutMeAsync();
            return Ok(response);
        }

        [HttpPost("AddAboutMe")]
        public async Task<ActionResult<ApiResponse<AboutMeResponseDto>>> AddAboutMe(CreateAboutMeDto createAboutMeDto)
        {
            var response = await _aboutMeService.AddAboutMeAsync(createAboutMeDto);
            if (!response.Sucesso)
            {
                return BadRequest(response);
            }

            return StatusCode(201, response);
        }

        [HttpPut("UpdateAboutMe/{id}")]
        public async Task<ActionResult<ApiResponse<AboutMeResponseDto>>> UpdateAboutMe(int id, UpdateAboutMeDto updateAboutMeDto)
        {
            var response = await _aboutMeService.UpdateAboutMeAsync(id, updateAboutMeDto);
            if (!response.Sucesso)
            {
                return BadRequest(response);
            }

            return Ok(response);
        }

        [HttpDelete("DeleteAboutMe/{id}")]
        public async Task<ActionResult<ApiResponse<bool>>> DeleteAboutMe(int id)
        {
            var response = await _aboutMeService.DeleteAboutMeAsync(id);
            if (!response.Sucesso)
            {
                return BadRequest(response);
            }

            return Ok(response);
        }
    }
}
