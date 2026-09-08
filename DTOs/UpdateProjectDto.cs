using System.ComponentModel.DataAnnotations;

namespace portfolioApi.DTOs
{
    public class UpdateProjectDto
    {
        [Required(ErrorMessage = "O titulo do projeto e obrigatorio.")]
        [MaxLength(150, ErrorMessage = "O titulo deve ter no maximo 150 caracteres.")]
        public string TitleProject { get; set; }

        [Required(ErrorMessage = "A descricao do projeto e obrigatoria.")]
        [MaxLength(1000, ErrorMessage = "A descricao deve ter no maximo 1000 caracteres.")]
        public string Description { get; set; }

        [Required(ErrorMessage = "A URL da imagem e obrigatoria.")]
        [Url(ErrorMessage = "Informe uma URL de imagem valida.")]
        public string UrlImage { get; set; }

        [Required(ErrorMessage = "O status do projeto e obrigatorio.")]
        [MaxLength(50, ErrorMessage = "O status deve ter no maximo 50 caracteres.")]
        public string Status { get; set; }

        [Required(ErrorMessage = "As tecnologias do projeto sao obrigatorias.")]
        [MaxLength(300, ErrorMessage = "As tecnologias devem ter no maximo 300 caracteres.")]
        public string Technologies { get; set; }
    }
}
