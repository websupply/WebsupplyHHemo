using Microsoft.Data.SqlClient;
using WebsupplyHHemo.API.Dto;

namespace WebsupplyHHemo.API.ADO
{
    public class ComplementoContabilItemADO
    {
        public string strMensagem = string.Empty;

        public bool ATUALIZA_DADOS_CONTABEIS_ITEM(string Connection, ComplementoContabilItemRequestDto objRequest)
        {
            string Erro = string.Empty;

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
                        Erro = reader["Erro"].ToString().Trim();
                    }
                    reader.NextResult();
                }
            }

            Conn.Dispose();

            if (Erro == "0")
            {
                strMensagem = $"Atualização de Conta Contabil para o Produto com o CodWebsupply: [{objRequest.CodProtheus}] e CodProtheus: [{objRequest.CodProtheus}] com Sucesso";
                
                return true;
            }
            else
            {
                strMensagem = $"Nenhum Produto localizado com o CodWebsupply: [{objRequest.CodWebsupply}] e CodProtheus: [{objRequest.CodProtheus}].";
                
                return false;
            }
        }
    }
}
