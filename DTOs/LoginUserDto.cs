using System.ComponentModel.DataAnnotations;

namespace portfolioApi.DTOs
{
    public class LoginUserDto
    {
        [Required(ErrorMessage = "O nome de usuario e obrigatorio.")]
        public string Username { get; set; }

        [Required(ErrorMessage = "A senha e obrigatoria.")]
        public string Password { get; set; }
    }
}
