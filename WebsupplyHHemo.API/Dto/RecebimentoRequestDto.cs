namespace WebsupplyHHemo.API.Dto
{
    public class RecebimentoRequestDto
    {
        public string NumPedido { get; set; }
        public string NumDoc { get; set; }
        public string Serie { get; set; }
        public string CodFornecedor { get; set; }
        public string CnpjFornecedor { get; set; }
        public string Status { get; set; }
    }
}
