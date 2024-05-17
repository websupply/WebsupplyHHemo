using WebsupplyHHemo.API.Interface;

namespace WebsupplyHHemo.API.Dto
{
    public class CadastroUnidadesRequestDto : UnidadesInterface
    {
        public int CodUnidade { get; set; }
        public string CGCMatriz { get; set; }
        public int CodUnidadeTipo { get; set; }
        public DadosEmpresa dadosEmpresa { get; set; }
        public DadosEmpresa? dadosFilial { get; set; }
        public DadosEntrega dadosEntrega { get; set; }
        public DadosFaturamento dadosFaturamento { get; set; }

        public CadastroUnidadesRequestDto()
        {
            dadosEmpresa = new DadosEmpresa();
            dadosFilial = new DadosEmpresa();
            dadosEntrega = new DadosEntrega();
            dadosFaturamento = new DadosFaturamento();
        }
    }
}
