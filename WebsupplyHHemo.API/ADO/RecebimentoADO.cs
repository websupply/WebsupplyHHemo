using Microsoft.Data.SqlClient;
using WebsupplyHHemo.API.Dto;

namespace WebsupplyHHemo.API.ADO
{
    public class RecebimentoADO
    {
        public string strMensagem = string.Empty;

        public bool ATUALIZA_RECEBIMENTO_FISCAL(string Connection, RecebimentoRequestDto objRequest)
        {
            // Seta os Parametros Iniciais de Retorno
            bool retorno = false;
            strMensagem = $"Erro durante a realização do Recebimento Fiscal do número [{objRequest.NumDoc}] referente ao pedido [{objRequest.NumPedido}]";

            ConexaoSQLServer Conn = new ConexaoSQLServer(Connection);

            string NomeProcedure = "SP_HHemo_WS_Produtos_Precos_CCONTABIL_CLI_UPD";

            List<SqlParameter> parametros = new List<SqlParameter>();
            //parametros.Add(new SqlParameter("@cCodProd", objRequest.CodWebsupply));
            //parametros.Add(new SqlParameter("@cCodProdutoCompleto", objRequest.CodProtheus));
            //parametros.Add(new SqlParameter("@cCCONTABIL_CLI", objRequest.ContaContabil));

            using (var reader = Conn.ExecutaComParametros(NomeProcedure, parametros))
            {
                while (reader.HasRows)
                {
                    strMensagem = $"Recebimento Fiscal do número [{objRequest.NumDoc}] referente ao pedido [{objRequest.NumPedido}] realizado com sucesso";
                    retorno = true;
                }
            }

            Conn.Dispose();

            return retorno;
        }
    }
}
