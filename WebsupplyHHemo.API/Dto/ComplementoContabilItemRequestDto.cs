using System.ComponentModel.DataAnnotations;

namespace WebsupplyHHemo.API.Dto
{
    public class ComplementoContabilItemRequestDto
    {
        [Required(ErrorMessage = "CodProtheus é obrigatório")]
        public string CodProtheus { get; set; }

        public string? CodWebsupply { get; set; }

        [Required(ErrorMessage = "Conta Contábil é obrigatória")]
        public string ContaContabil { get; set; }
    }
}
