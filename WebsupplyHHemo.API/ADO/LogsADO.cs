using Microsoft.Data.SqlClient;
using WebsupplyHHemo.API.Models;

namespace WebsupplyHHemo.API.ADO
{
    public class LogsADO
    {
        public static void GERA_LOGDEOPERACAO(string Connection, UserModel objUser, LogDeOperacaoModel objLog)
        {
            ConexaoSQLServer Conn = new ConexaoSQLServer(Connection);

            string NomeProcedure = "SP_HHEMO_LOGDEOPERACAO_INS";

            List<SqlParameter> parametros = new List<SqlParameter>();
            parametros.Add(new SqlParameter("@cCGC", objUser.CGC));
            parametros.Add(new SqlParameter("@cCCUSTO", objUser.CCUSTO));
            parametros.Add(new SqlParameter("@cREQUISIT", objUser.REQUISIT));
            parametros.Add(new SqlParameter("@nCod_Operacao", objLog.nCod_Operacao));
            parametros.Add(new SqlParameter("@cDetalhe", objLog.cDetalhe));
            parametros.Add(new SqlParameter("@cIP", objLog.cIP));

            Conn.ExecutaComParametrosSemRetorno(NomeProcedure, parametros);

            Conn.Dispose();
        }
    }
}
