using System.ComponentModel.DataAnnotations;

namespace WebsupplyHHemo.API.Dto
{
    public class RecebimentoRequestDto
    {
        [Required(ErrorMessage = "NumPedido é obrigatório")]
        public string NumPedido { get; set; }

        [Required(ErrorMessage = "NumDoc é obrigatório")]
        public string NumDoc { get; set; }

        [Required(ErrorMessage = "Serie é obrigatório")]
        public string Serie { get; set; }

        [Required(ErrorMessage = "CodFornecedor é obrigatório")]
        public string CodFornecedor { get; set; }

        [Required(ErrorMessage = "CnpjFornecedor é obrigatório")]
        public string CnpjFornecedor { get; set; }

        [Required(ErrorMessage = "Status é obrigatório")]
        public string Status { get; set; }
    }
}
