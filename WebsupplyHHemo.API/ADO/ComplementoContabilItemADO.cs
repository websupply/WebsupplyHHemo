using Microsoft.Data.SqlClient;
using WebsupplyHHemo.API.Dto;

namespace WebsupplyHHemo.API.ADO
{
    public class ComplementoContabilItemADO
    {
        public string strMensagem = string.Empty;

        public bool ATUALIZA_DADOS_CONTABEIS_ITEM(string Connection, ComplementoContabilItemRequestDto objRequest)
        {
            // Seta os Parametros Iniciais de Retorno
            bool retorno = false;
            strMensagem = $"Erro durante a Atualização de Conta Contabil para o Produto com CodWebsupply [{objRequest.CodWebsupply}] atribuido ao CodProtheus [{objRequest.CodProtheus}] atualizando para a Conta Contabil [{objRequest.ContaContabil}]";

            ConexaoSQLServer Conn = new ConexaoSQLServer(Connection);

            string NomeProcedure = "SP_HHemo_WS_Produtos_Precos_CCONTABIL_CLI_UPD";

            List<SqlParameter> parametros = new List<SqlParameter>();
            parametros.Add(new SqlParameter("@cCodProd", objRequest.CodWebsupply));
            parametros.Add(new SqlParameter("@cCodProdutoCompleto", objRequest.CodProtheus));
            parametros.Add(new SqlParameter("@cCCONTABIL_CLI", objRequest.ContaContabil));

            using (var reader = Conn.ExecutaComParametros(NomeProcedure, parametros))
            {
                while (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        strMensagem = $"Atualização de Conta Contabil para o Produto com CodWebsupply [{objRequest.CodWebsupply}] atribuido ao CodProtheus [{objRequest.CodProtheus}] atualizando para a Conta Contabil [{objRequest.ContaContabil}] com sucesso";
                        retorno = true;
                    }
                    reader.NextResult();
                }
            }
            
            Conn.Dispose();

            return retorno;
        }
    }
}
