using Microsoft.Data.SqlClient;
using WebsupplyHHemo.API.Dto;
using WebsupplyHHemo.API.Models;

namespace WebsupplyHHemo.API.ADO
{
    public class CadastroUnidadesFiliaisADO
    {
        public static CadastroUnidadesFiliaisResponseDto CADASTRO_UNIDADES_FILIAIS(string Connection, string CGCMatriz, UserModel userModel, CadastroUnidadesFiliaisRequestDto requestDto)
        {
            CadastroUnidadesFiliaisResponseDto resultDto = new CadastroUnidadesFiliaisResponseDto();

            ConexaoSQLServer Conn = new ConexaoSQLServer(Connection);

            string NomeProcedure = "SP_Hhemo_UNIDADES_INC_ALT";

            List<SqlParameter> parametros = new List<SqlParameter>();
            parametros.Add(new SqlParameter("@CGCMatriz", CGCMatriz));
            parametros.Add(new SqlParameter("@iCODUnidade", requestDto.codigo));
            parametros.Add(new SqlParameter("@COD_UNIDADE_TIPO", requestDto.COD_UNIDADE_TIPO));
            parametros.Add(new SqlParameter("@cRazSoc", requestDto.RAZSOC));
            parametros.Add(new SqlParameter("@DESCRICAO", requestDto.descricao));
            parametros.Add(new SqlParameter("@CGC", requestDto.CGC));
            parametros.Add(new SqlParameter("@DTInauguracao", requestDto.DTInauguracao));
            parametros.Add(new SqlParameter("@Limite_Aprovacao", requestDto.Limite_Aprovacao));
            parametros.Add(new SqlParameter("@Previsao_Entrega", requestDto.Previsao_Entrega));
            parametros.Add(new SqlParameter("@ENT_CGC", requestDto.CGC));
            parametros.Add(new SqlParameter("@ENT_Inscr_Est", requestDto.INSCEST));
            parametros.Add(new SqlParameter("@ENT_Inscr_Mun", requestDto.INSCMUN));
            parametros.Add(new SqlParameter("@ENT_Endereco", requestDto.Endereco));
            parametros.Add(new SqlParameter("@ENT_Numero", requestDto.Numero));
            parametros.Add(new SqlParameter("@ENT_Complemento", requestDto.Complemento));
            parametros.Add(new SqlParameter("@ENT_Bairro", requestDto.Bairro));
            parametros.Add(new SqlParameter("@ENT_Cidade", requestDto.Cidade));
            parametros.Add(new SqlParameter("@ENT_UF", requestDto.UF));
            parametros.Add(new SqlParameter("@ENT_CEP", requestDto.CEP));
            parametros.Add(new SqlParameter("@ENT_Tel_DDD", requestDto.ENT_Tel_DDD));
            parametros.Add(new SqlParameter("@ENT_Tel_NUM", requestDto.ENT_Tel_NUM));
            parametros.Add(new SqlParameter("@ENT_Tel_RAM", requestDto.ENT_Tel_RAM));
            parametros.Add(new SqlParameter("@FAT_CGC", requestDto.CGC));
            parametros.Add(new SqlParameter("@FAT_Inscr_Est", requestDto.INSCEST));
            parametros.Add(new SqlParameter("@FAT_Inscr_Mun", requestDto.INSCMUN));
            parametros.Add(new SqlParameter("@FAT_Endereco", requestDto.Endereco));
            parametros.Add(new SqlParameter("@FAT_Numero", requestDto.Numero));
            parametros.Add(new SqlParameter("@FAT_Complemento", requestDto.Complemento));
            parametros.Add(new SqlParameter("@FAT_Bairro", requestDto.Bairro));
            parametros.Add(new SqlParameter("@FAT_Cidade", requestDto.Cidade));
            parametros.Add(new SqlParameter("@FAT_UF", requestDto.UF));
            parametros.Add(new SqlParameter("@FAT_CEP", requestDto.CEP));
            parametros.Add(new SqlParameter("@FAT_Tel_DDD", requestDto.FAT_Tel_DDD));
            parametros.Add(new SqlParameter("@FAT_Tel_NUM", requestDto.FAT_Tel_NUM));
            parametros.Add(new SqlParameter("@FAT_Tel_RAM", requestDto.FAT_Tel_RAM));
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
