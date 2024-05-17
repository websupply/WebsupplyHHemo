using Microsoft.Data.SqlClient;
using WebsupplyHHemo.API.Dto;
using WebsupplyHHemo.API.Models;

namespace WebsupplyHHemo.API.ADO
{
    public class UnidadesADO
    {
        public static CadastroUnidadesResponseDto CADASTRO_UNIDADES_FILIAIS(string Connection, string CGCMatriz, UserModel userModel, CadastroUnidadesRequestDto requestDto)
        {
            CadastroUnidadesResponseDto resultDto = new CadastroUnidadesResponseDto();

            ConexaoSQLServer Conn = new ConexaoSQLServer(Connection);

            string NomeProcedure = "SP_Hhemo_UNIDADES_INC_ALT";

            List<SqlParameter> parametros = new List<SqlParameter>();
            parametros.Add(new SqlParameter("@CGCMatriz", CGCMatriz));
            parametros.Add(new SqlParameter("@iCODUnidade", requestDto.CodUnidade));
            parametros.Add(new SqlParameter("@COD_UNIDADE_TIPO", requestDto.CodUnidadeTipo));
            parametros.Add(new SqlParameter("@cRazSoc", requestDto.dadosEmpresa.RazaoSocial));
            parametros.Add(new SqlParameter("@DESCRICAO", requestDto.dadosEmpresa.Descricao));
            parametros.Add(new SqlParameter("@CGC", requestDto.dadosEmpresa.CGC));
            parametros.Add(new SqlParameter("@DTInauguracao", requestDto.dadosEmpresa.DataInauguracao));
            parametros.Add(new SqlParameter("@Limite_Aprovacao", requestDto.dadosEmpresa.LimiteAprovacao));
            parametros.Add(new SqlParameter("@Previsao_Entrega", requestDto.dadosEmpresa.PrevisaoEntrega));
            parametros.Add(new SqlParameter("@ENT_CGC", requestDto.dadosEntrega.CGC));
            parametros.Add(new SqlParameter("@ENT_Inscr_Est", requestDto.dadosEntrega.Inscr_Est));
            parametros.Add(new SqlParameter("@ENT_Inscr_Mun", requestDto.dadosEntrega.Inscr_Mun));
            parametros.Add(new SqlParameter("@ENT_Endereco", requestDto.dadosEntrega.Endereco));
            parametros.Add(new SqlParameter("@ENT_Numero", requestDto.dadosEntrega.Numero));
            parametros.Add(new SqlParameter("@ENT_Complemento", requestDto.dadosEntrega.Complemento));
            parametros.Add(new SqlParameter("@ENT_Bairro", requestDto.dadosEntrega.Bairro));
            parametros.Add(new SqlParameter("@ENT_Cidade", requestDto.dadosEntrega.Cidade));
            parametros.Add(new SqlParameter("@ENT_UF", requestDto.dadosEntrega.UF));
            parametros.Add(new SqlParameter("@ENT_CEP", requestDto.dadosEntrega.CEP));
            parametros.Add(new SqlParameter("@ENT_Tel_DDD", requestDto.dadosEntrega.Tel_DDD));
            parametros.Add(new SqlParameter("@ENT_Tel_NUM", requestDto.dadosEntrega.Tel_NUM));
            parametros.Add(new SqlParameter("@ENT_Tel_RAM", requestDto.dadosEntrega.Tel_RAM));
            parametros.Add(new SqlParameter("@FAT_CGC", requestDto.dadosFaturamento.CGC));
            parametros.Add(new SqlParameter("@FAT_Inscr_Est", requestDto.dadosFaturamento.Inscr_Est));
            parametros.Add(new SqlParameter("@FAT_Inscr_Mun", requestDto.dadosFaturamento.Inscr_Mun));
            parametros.Add(new SqlParameter("@FAT_Endereco", requestDto.dadosFaturamento.Endereco));
            parametros.Add(new SqlParameter("@FAT_Numero", requestDto.dadosFaturamento.Numero));
            parametros.Add(new SqlParameter("@FAT_Complemento", requestDto.dadosFaturamento.Complemento));
            parametros.Add(new SqlParameter("@FAT_Bairro", requestDto.dadosFaturamento.Bairro));
            parametros.Add(new SqlParameter("@FAT_Cidade", requestDto.dadosFaturamento.Cidade));
            parametros.Add(new SqlParameter("@FAT_UF", requestDto.dadosFaturamento.UF));
            parametros.Add(new SqlParameter("@FAT_CEP", requestDto.dadosFaturamento.CEP));
            parametros.Add(new SqlParameter("@FAT_Tel_DDD", requestDto.dadosFaturamento.Tel_DDD));
            parametros.Add(new SqlParameter("@FAT_Tel_NUM", requestDto.dadosFaturamento.Tel_NUM));
            parametros.Add(new SqlParameter("@FAT_Tel_RAM", requestDto.dadosFaturamento.Tel_RAM));
            parametros.Add(new SqlParameter("@Acao", "INCLUIR"));
            parametros.Add(new SqlParameter("@cCGC", userModel.CGC));
            parametros.Add(new SqlParameter("@cCC", userModel.CCUSTO));
            parametros.Add(new SqlParameter("@cREQ", userModel.REQUISIT));

            using (var reader = Conn.ExecutaComParametros(NomeProcedure, parametros))
            {
                while (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        resultDto.CODUnidade = (int)reader["CODUnidade"];
                        resultDto.Erro = (int)reader["Erro"];
                        resultDto.CGC_ANTIGO = String.IsNullOrEmpty(reader["CGC_ANTIGO"].ToString()) ? "" : reader["CGC_ANTIGO"].ToString().Trim();
                    }
                    reader.NextResult();
                }
            }

            Conn.Dispose();

            return resultDto;
        }
    }
}
