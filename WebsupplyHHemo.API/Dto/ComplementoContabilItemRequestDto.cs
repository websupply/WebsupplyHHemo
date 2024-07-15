using System.ComponentModel.DataAnnotations;

namespace WebsupplyHHemo.API.Dto
{
    public class ComplementoContabilItemRequestDto
    {
        [Required(ErrorMessage = "CodProtheus é obrigatório")]
        public string CodProtheus { get; set; }

        [Required(ErrorMessage = "CodWebsupply é obrigatório")]
        public string CodWebsupply { get; set; }

        [Required(ErrorMessage = "Conta Contábil é obrigatória")]
        public string ContaContabil { get; set; }
    }
}
