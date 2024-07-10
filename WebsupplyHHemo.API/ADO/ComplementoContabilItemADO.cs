using Microsoft.Data.SqlClient;
using WebsupplyHHemo.API.Dto;

namespace WebsupplyHHemo.API.ADO
{
    public class ComplementoContabilItemADO
    {
        public string strMensagem = string.Empty;

        public bool ATUALIZA_DADOS_CONTABEIS_ITEM(string Connection, ComplementoContabilItemRequestDto objRequest)
        {
            ConexaoSQLServer Conn = new ConexaoSQLServer(Connection);

            string NomeProcedure = "SP_HHemo_WS_Produtos_Precos_CCONTABIL_CLI_UPD";

            List<SqlParameter> parametros = new List<SqlParameter>();
            parametros.Add(new SqlParameter("@cCodProd", objRequest.CodWebsupply));
            parametros.Add(new SqlParameter("@cCodProdutoCompleto", objRequest.CodProtheus));
            parametros.Add(new SqlParameter("@cCCONTABIL_CLI", objRequest.ContaContabil));

            try
            {
                Conn.ExecutaComParametrosSemRetorno(NomeProcedure, parametros);

                Conn.Dispose();

                strMensagem = $"Atualização Contábil do Item [{objRequest.CodProtheus}] para a Conta Contábil [{objRequest.ContaContabil}] Concluída com Sucesso";

                return true;
            }
            catch (Exception ex)
            {
                Conn.Dispose();

                strMensagem = ex.Message;

                return false;
            }
        }
    }
}
