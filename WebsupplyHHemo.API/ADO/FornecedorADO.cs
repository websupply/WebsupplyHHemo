using Microsoft.Data.SqlClient;
using WebsupplyHHemo.API.Dto;

namespace WebsupplyHHemo.API.ADO
{
    public class FornecedorADO
    {
        public string strMensagem = string.Empty;

        public bool ATUALIZA_STATUS_FORNECEDOR(string Connection, FornecedorRequestDto objRequest)
        {
            ConexaoSQLServer Conn = new ConexaoSQLServer(Connection);

            string NomeProcedure = "SP_HHemo_WS_Liberacao_Fornecedor_UPD";

            List<SqlParameter> parametros = new List<SqlParameter>();
            parametros.Add(new SqlParameter("@cA2_COD", objRequest.A2_COD));
            parametros.Add(new SqlParameter("@cA2_LOJA", objRequest.A2_LOJA));
            parametros.Add(new SqlParameter("@cA2_CGC", objRequest.A2_CGC));
            parametros.Add(new SqlParameter("@vA2_Justificativa", objRequest.A2_Justificativa));
            parametros.Add(new SqlParameter("@cA2_MSBLQL", objRequest.A2_MSBLQL));

            try
            {
                Conn.ExecutaComParametrosSemRetorno(NomeProcedure, parametros);

                Conn.Dispose();

                strMensagem = $"Atualização de Status do Fornecedor Realizada com Sucesso. Parametros Enviados - A2_COD: [{objRequest.A2_COD}], A2_LOJA: [{objRequest.A2_LOJA}], A2_CGC: [{objRequest.A2_CGC}], A2_Justificativa: [{objRequest.A2_Justificativa}], A2_MSBLQL: [{objRequest.A2_MSBLQL}]";

                return true;
            }
            catch (Exception ex)
            {
                Conn.Dispose();

                strMensagem = $"Erro durante a Atualização de Status do Fornecedor. Parametros Enviados - A2_COD: [{objRequest.A2_COD}], A2_LOJA: [{objRequest.A2_LOJA}], A2_CGC: [{objRequest.A2_CGC}], A2_Justificativa: [{objRequest.A2_Justificativa}], A2_MSBLQL: [{objRequest.A2_MSBLQL}]";

                return false;
            }
        }
    }
}
