using System.ComponentModel.DataAnnotations;

namespace portfolioApi.DTOs
{
    public class CreateUserDto
    {
        [Required(ErrorMessage = "O nome de usuario e obrigatorio.")]
        [MinLength(3, ErrorMessage = "O nome de usuario deve ter no minimo 3 caracteres.")]
        [MaxLength(100, ErrorMessage = "O nome de usuario deve ter no maximo 100 caracteres.")]
        public string Username { get; set; }

        [Required(ErrorMessage = "O email e obrigatorio.")]
        [EmailAddress(ErrorMessage = "Informe um email valido.")]
        [MaxLength(255, ErrorMessage = "O email deve ter no maximo 255 caracteres.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "A senha e obrigatoria.")]
        [MinLength(6, ErrorMessage = "A senha deve ter no minimo 6 caracteres.")]
        [MaxLength(100, ErrorMessage = "A senha deve ter no maximo 100 caracteres.")]
        public string Password { get; set; }
    }
}
