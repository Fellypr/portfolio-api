using System.ComponentModel.DataAnnotations;

namespace portfolioApi.DTOs
{
    public class UpdateAboutMeDto
    {
        [Required(ErrorMessage = "O nome do portfolio e obrigatorio.")]
        [MaxLength(150, ErrorMessage = "O nome do portfolio deve ter no maximo 150 caracteres.")]
        public string NameInPortfolio { get; set; }

        [Required(ErrorMessage = "A subdescricao e obrigatoria.")]
        [MaxLength(300, ErrorMessage = "A subdescricao deve ter no maximo 300 caracteres.")]
        public string SubDescriptions { get; set; }

        [Required(ErrorMessage = "A profissao e obrigatoria.")]
        [MaxLength(150, ErrorMessage = "A profissao deve ter no maximo 150 caracteres.")]
        public string Profession { get; set; }

        [Required(ErrorMessage = "A descricao sobre mim e obrigatoria.")]
        [MaxLength(1500, ErrorMessage = "A descricao sobre mim deve ter no maximo 1500 caracteres.")]
        public string DescriptionAboutMe { get; set; }
    }
}
