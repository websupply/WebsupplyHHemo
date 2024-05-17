using Microsoft.AspNetCore.Components.Forms;

namespace WebsupplyHHemo.API.Interface
{
    public interface UnidadesInterface
    {
        public int CodUnidade { get; set; }
        public string CGCMatriz { get; set; }
        public int CodUnidadeTipo { get; set; }

        public DadosEmpresa dadosEmpresa { get; set; }
        public DadosEmpresa? dadosFilial { get; set; }
        public DadosEntrega dadosEntrega { get; set; }
        public DadosFaturamento dadosFaturamento { get; set; }        
    }

    public class DadosEmpresa
    {
        public int Codigo { get; set; }
        public string RazaoSocial { get; set; }
        public string Descricao { get; set; }
        public string CGC { get; set; }
        public string InscricaoMunicipal { get; set; }
        public string InscricaoEstadual { get; set; }
        public DateTime DataInauguracao { get; set; }
        public int LimiteAprovacao { get; set; }
        public int PrevisaoEntrega { get; set; }
    }

    public class DadosEntrega
    {
        public string CGC { get; set; }
        public string Inscr_Est { get; set; }
        public string Inscr_Mun { get; set; }
        public string Endereco { get; set; }
        public string Numero { get; set; }
        public string Complemento { get; set; }
        public string Bairro { get; set; }
        public string Cidade { get; set; }
        public string UF { get; set; }
        public string CEP { get; set; }
        public string Tel_DDD { get; set; }
        public string Tel_NUM { get; set; }
        public string Tel_RAM { get; set; }
    }

    public class DadosFaturamento
    {
        public string CGC { get; set; }
        public string Inscr_Est { get; set; }
        public string Inscr_Mun { get; set; }
        public string Endereco { get; set; }
        public string Numero { get; set; }
        public string Complemento { get; set; }
        public string Bairro { get; set; }
        public string Cidade { get; set; }
        public string UF { get; set; }
        public string CEP { get; set; }
        public string Tel_DDD { get; set; }
        public string Tel_NUM { get; set; }
        public string Tel_RAM { get; set; }
    }
}
