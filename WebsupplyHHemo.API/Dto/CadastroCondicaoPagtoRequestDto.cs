namespace WebsupplyHHemo.API.Dto
{
    public class CadastroCondicaoPagtoRequestDto
    {
        public int CodFormaPagto { get; set; }
        public string Descricao { get; set; }
        public string CodTipo { get; set; }
        public int Dias { get; set; }
        public string Status { get; set; }
    }
}
