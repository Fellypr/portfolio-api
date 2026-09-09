using System.ComponentModel.DataAnnotations;

namespace portfolioApi.DTOs
{
    public class CreateContactDto
    {
        [Required(ErrorMessage = "O whatsapp e obrigatorio.")]
        [MaxLength(30, ErrorMessage = "O whatsapp deve ter no maximo 30 caracteres.")]
        public string Whatsapp { get; set; }

        [Required(ErrorMessage = "O email e obrigatorio.")]
        [EmailAddress(ErrorMessage = "Informe um email valido.")]
        [MaxLength(150, ErrorMessage = "O email deve ter no maximo 150 caracteres.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "O linkedin e obrigatorio.")]
        [Url(ErrorMessage = "Informe uma URL do linkedin valida.")]
        public string Linkedin { get; set; }
    }
}
