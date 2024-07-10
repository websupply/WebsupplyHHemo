using Microsoft.Data.SqlClient;
using WebsupplyHHemo.API.Models;

namespace WebsupplyHHemo.API.ADO
{
    public class TokenADO
    {
        public static UserModel TOKEN_SEL(string Connection, UserModel objUser)
        {
            ConexaoSQLServer Conn = new ConexaoSQLServer(Connection);

            string NomeProcedure = "SP_CentrosCusto_TOKEN_SEL";

            List<SqlParameter> parametros = new List<SqlParameter>();
            parametros.Add(new SqlParameter("@CGC", objUser.CGC));
            parametros.Add(new SqlParameter("@CCUSTO", objUser.CCUSTO));
            parametros.Add(new SqlParameter("@REQUISIT", objUser.REQUISIT));

            using (var reader = Conn.ExecutaComParametros(NomeProcedure, parametros))
            {
                while (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        objUser.RefreshToken = reader["RefreshToken"].ToString();
                        objUser.RefreshTokenExpiryTime = (DateTime?)reader["RefreshTokenExpiryTime"];
                    }
                    reader.NextResult();
                }
            }

            Conn.Dispose();

            return objUser;

        }

        public static void TOKEN_INSUPD(string Connection, UserModel objUser)
        {
            ConexaoSQLServer Conn = new ConexaoSQLServer(Connection);

            string NomeProcedure = "SP_CentrosCusto_TOKEN_INSUPD";

            List<SqlParameter> parametros = new List<SqlParameter>();
            parametros.Add(new SqlParameter("@CGC", objUser.CGC));
            parametros.Add(new SqlParameter("@CCUSTO", objUser.CCUSTO));
            parametros.Add(new SqlParameter("@REQUISIT", objUser.REQUISIT));
            parametros.Add(new SqlParameter("@RefreshToken", objUser.RefreshToken));
            parametros.Add(new SqlParameter("@RefreshTokenExpiryTime", objUser.RefreshTokenExpiryTime));

            SqlCommand cmd = Conn.ExecutaComParametrosSemRetorno(NomeProcedure, parametros);

            Conn.Dispose();

        }
    }
}
