using Microsoft.AspNetCore.Mvc;
using portfolioApi.DTOs;
using portfolioApi.Services.Interfaces;

namespace portfolioApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ContactsController : ControllerBase
    {
        private readonly IContactService _contactService;

        public ContactsController(IContactService contactService)
        {
            _contactService = contactService;
        }

        [HttpGet("GetAllContacts")]
        public async Task<ActionResult<ApiResponse<List<ContactResponseDto>>>> GetAllContacts()
        {
            var response = await _contactService.GetAllContactsAsync();
            return Ok(response);
        }

        [HttpPost("AddContact")]
        public async Task<ActionResult<ApiResponse<ContactResponseDto>>> AddContact(CreateContactDto createContactDto)
        {
            var response = await _contactService.AddContactAsync(createContactDto);
            if (!response.Sucesso)
            {
                return BadRequest(response);
            }

            return StatusCode(201, response);
        }

        [HttpPut("UpdateContact/{id}")]
        public async Task<ActionResult<ApiResponse<ContactResponseDto>>> UpdateContact(int id, UpdateContactDto updateContactDto)
        {
            var response = await _contactService.UpdateContactAsync(id, updateContactDto);
            if (!response.Sucesso)
            {
                return BadRequest(response);
            }

            return Ok(response);
        }

        [HttpDelete("DeleteContact/{id}")]
        public async Task<ActionResult<ApiResponse<bool>>> DeleteContact(int id)
        {
            var response = await _contactService.DeleteContactAsync(id);
            if (!response.Sucesso)
            {
                return BadRequest(response);
            }

            return Ok(response);
        }
    }
}
