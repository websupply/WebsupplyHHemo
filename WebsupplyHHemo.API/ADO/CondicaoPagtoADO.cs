using Microsoft.Data.SqlClient;
using WebsupplyHHemo.API.Dto;
using WebsupplyHHemo.API.Models;

namespace WebsupplyHHemo.API.ADO
{
    public class CondicaoPagtoADO
    {
        public static CadastroCondicaoPagtoResponseDto CADASTRO_COND_PAGTO(string Connection, string CGCMatriz, UserModel userModel, CadastroCondicaoPagtoRequestDto requestDto)
        {
            CadastroCondicaoPagtoResponseDto resultDto = new CadastroCondicaoPagtoResponseDto();

            ConexaoSQLServer Conn = new ConexaoSQLServer(Connection);

            string NomeProcedure = "SP_HHEMO_FORMAS_PAGTO_INSUPD";

            List<SqlParameter> parametros = new List<SqlParameter>();
            parametros.Add(new SqlParameter("@Cod_FPagto", null));
            parametros.Add(new SqlParameter("@descricao", requestDto.Descricao));
            parametros.Add(new SqlParameter("@CodTipo", requestDto.CodTipo));
            parametros.Add(new SqlParameter("@dias", requestDto.Dias));
            parametros.Add(new SqlParameter("@CGC", CGCMatriz));

            using (var reader = Conn.ExecutaComParametros(NomeProcedure, parametros))
            {
                while (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        resultDto.Cod_FPagto = (int)reader["Cod_FPagto"];
                        resultDto.Erro = (int)reader["Erro"];
                    }
                    reader.NextResult();
                }
            }

            Conn.Dispose();

            return resultDto;
        }

        public static CadastroCondicaoPagtoResponseDto ATUALIZA_COND_PAGTO(string Connection, string CGCMatriz, UserModel userModel, CadastroCondicaoPagtoRequestDto requestDto)
        {
            CadastroCondicaoPagtoResponseDto resultDto = new CadastroCondicaoPagtoResponseDto();

            ConexaoSQLServer Conn = new ConexaoSQLServer(Connection);

            string NomeProcedure = "SP_HHEMO_FORMAS_PAGTO_INSUPD";

            List<SqlParameter> parametros = new List<SqlParameter>();
            parametros.Add(new SqlParameter("@Cod_FPagto", requestDto.CodFormaPagto));
            parametros.Add(new SqlParameter("@descricao", requestDto.Descricao));
            parametros.Add(new SqlParameter("@CodTipo", requestDto.CodTipo));
            parametros.Add(new SqlParameter("@dias", requestDto.Dias));
            parametros.Add(new SqlParameter("@CGC", CGCMatriz));

            using (var reader = Conn.ExecutaComParametros(NomeProcedure, parametros))
            {
                while (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        resultDto.Cod_FPagto = (int)reader["Cod_FPagto"];
                        resultDto.Erro = (int)reader["Erro"];
                    }
                    reader.NextResult();
                }
            }

            Conn.Dispose();

            return resultDto;
        }
    }
}
