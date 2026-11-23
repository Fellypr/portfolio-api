using System.ComponentModel.DataAnnotations;

namespace portfolioApi.DTOs
{
    public class UpdateUserDto
    {
        [Required(ErrorMessage = "O nome de usuario e obrigatorio.")]
        [MinLength(3, ErrorMessage = "O nome de usuario deve ter no minimo 3 caracteres.")]
        [MaxLength(100, ErrorMessage = "O nome de usuario deve ter no maximo 100 caracteres.")]
        public string Username { get; set; }

        [Required(ErrorMessage = "O email e obrigatorio.")]
        [EmailAddress(ErrorMessage = "Informe um email valido.")]
        [MaxLength(255, ErrorMessage = "O email deve ter no maximo 255 caracteres.")]
        public string Email { get; set; }
    }
}
