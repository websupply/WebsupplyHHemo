using Microsoft.Data.SqlClient;
using WebsupplyHHemo.API.Dto;

namespace WebsupplyHHemo.API.ADO
{
    public class FornecedorADO
    {
        public string strMensagem = string.Empty;

        public bool ATUALIZA_STATUS_FORNECEDOR(string Connection, FornecedorRequestDto objRequest)
        {
            // Seta os Parametros Iniciais de Retorno
            bool retorno = false;
            strMensagem = $"Erro durante a Atualização de Status do Fornecedor com A2_COD [{objRequest.A2_COD}] referente a A2_LOJA [{objRequest.A2_LOJA}] do A2_CGC [{objRequest.A2_CGC}] para o Status - A2_MSBLQL [{objRequest.A2_MSBLQL}]";

            ConexaoSQLServer Conn = new ConexaoSQLServer(Connection);

            string NomeProcedure = "SP_HHemo_WS_Liberacao_Fornecedor_UPD";

            List<SqlParameter> parametros = new List<SqlParameter>();
            parametros.Add(new SqlParameter("@cA2_COD", objRequest.A2_COD));
            parametros.Add(new SqlParameter("@cA2_LOJA", objRequest.A2_LOJA));
            parametros.Add(new SqlParameter("@cA2_CGC", objRequest.A2_CGC));
            parametros.Add(new SqlParameter("@vA2_Justificativa", objRequest.A2_Justificativa));
            parametros.Add(new SqlParameter("@cA2_MSBLQL", objRequest.A2_MSBLQL));

            using (var reader = Conn.ExecutaComParametros(NomeProcedure, parametros))
            {
                while (reader.HasRows)
                {
                    strMensagem = $"Atualização de Status do Fornecedor com A2_COD [{objRequest.A2_COD}] referente a A2_LOJA [{objRequest.A2_LOJA}] do A2_CGC [{objRequest.A2_CGC}] para o Status - A2_MSBLQL [{objRequest.A2_MSBLQL}] com sucesso";
                    retorno = true;
                }
            }

            Conn.Dispose();

            return retorno;
        }
    }
}
