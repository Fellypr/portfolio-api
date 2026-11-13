using System.ComponentModel.DataAnnotations;

namespace portfolioApi.DTOs
{
    public class LoginUserDto
    {
        [Required(ErrorMessage = "O email e obrigatorio.")]
        [EmailAddress(ErrorMessage = "Informe um email valido.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "A senha e obrigatoria.")]
        public string Password { get; set; }
    }
}
