using System.ComponentModel.DataAnnotations;

namespace WebsupplyHHemo.API.Dto
{
    public class FornecedorRequestDto
    {
        [Required(ErrorMessage = "A2_COD é obrigatório")]
        public string A2_COD { get; set; }

        [Required(ErrorMessage = "A2_LOJA é obrigatório")]
        public string A2_LOJA { get; set; }

        [Required(ErrorMessage = "A2_CGC é obrigatório")]
        public string A2_CGC { get; set; }

        [Required(ErrorMessage = "A2_Justificativa é obrigatório")]
        public string A2_Justificativa { get; set; }

        [Required(ErrorMessage = "A2_MSBLQL é obrigatório")]
        public string A2_MSBLQL { get; set; }
    }
}
