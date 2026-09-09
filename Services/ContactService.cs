using Microsoft.EntityFrameworkCore;
using portfolioApi.Data;
using portfolioApi.DTOs;
using portfolioApi.Models;
using portfolioApi.Services.Interfaces;

namespace portfolioApi.Services
{
    public class ContactService : IContactService
    {
        private readonly AppDbContext _context;

        public ContactService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<ApiResponse<List<ContactResponseDto>>> GetAllContactsAsync()
        {
            var contacts = await _context.Contacts
                .AsNoTracking()
                .OrderBy(contact => contact.Id)
                .Select(contact => MapToResponse(contact))
                .ToListAsync();

            return ApiResponse<List<ContactResponseDto>>.Ok(contacts, "Contatos listados com sucesso.");
        }

        public async Task<ApiResponse<ContactResponseDto>> AddContactAsync(CreateContactDto createContactDto)
        {
            var email = createContactDto.Email.Trim();

            var contactAlreadyExists = await _context.Contacts
                .AnyAsync(contact => contact.Email.ToLower() == email.ToLower());

            if (contactAlreadyExists)
            {
                return ApiResponse<ContactResponseDto>.Erro("Ja existe um contato com esse email.");
            }

            var contact = new Contacts
            {
                Whatsapp = createContactDto.Whatsapp.Trim(),
                Email = email,
                Linkedin = createContactDto.Linkedin.Trim()
            };

            _context.Contacts.Add(contact);
            await _context.SaveChangesAsync();

            var response = MapToResponse(contact);

            return ApiResponse<ContactResponseDto>.Ok(response, "Contato criado com sucesso.");
        }

        public async Task<ApiResponse<ContactResponseDto>> UpdateContactAsync(int id, UpdateContactDto updateContactDto)
        {
            var contact = await _context.Contacts.FindAsync(id);

            if (contact is null)
            {
                return ApiResponse<ContactResponseDto>.Erro("Contato nao encontrado.");
            }

            var email = updateContactDto.Email.Trim();

            var contactAlreadyExists = await _context.Contacts
                .AnyAsync(existingContact =>
                    existingContact.Id != id &&
                    existingContact.Email.ToLower() == email.ToLower());

            if (contactAlreadyExists)
            {
                return ApiResponse<ContactResponseDto>.Erro("Ja existe um contato com esse email.");
            }

            contact.Whatsapp = updateContactDto.Whatsapp.Trim();
            contact.Email = email;
            contact.Linkedin = updateContactDto.Linkedin.Trim();

            await _context.SaveChangesAsync();

            var response = MapToResponse(contact);

            return ApiResponse<ContactResponseDto>.Ok(response, "Contato atualizado com sucesso.");
        }

        public async Task<ApiResponse<bool>> DeleteContactAsync(int id)
        {
            var contact = await _context.Contacts.FindAsync(id);

            if (contact is null)
            {
                return ApiResponse<bool>.Erro("Contato nao encontrado.");
            }

            _context.Contacts.Remove(contact);
            await _context.SaveChangesAsync();

            return ApiResponse<bool>.Ok(true, "Contato excluido com sucesso.");
        }

        private static ContactResponseDto MapToResponse(Contacts contact)
        {
            return new ContactResponseDto
            {
                Id = contact.Id,
                Whatsapp = contact.Whatsapp,
                Email = contact.Email,
                Linkedin = contact.Linkedin
            };
        }
    }
}
