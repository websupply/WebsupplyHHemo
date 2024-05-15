using Microsoft.Data.SqlClient;
using WebsupplyHHemo.API.Models;

namespace WebsupplyHHemo.API.ADO
{
    public class UserADO
    {
        public static UserModel VALIDA_LOGIN_SEL(string Connection,string Login,string Senha,string CGCMATRIZ)
        {
            UserModel objUser = new UserModel();

            ConexaoSQLServer Conn = new ConexaoSQLServer(Connection);

            string NomeProcedure = "SP_MDS_LOGINS_SEL";

            List<SqlParameter> parametros = new List<SqlParameter>();
            parametros.Add(new SqlParameter("@cLogin", Login));
            parametros.Add(new SqlParameter("@cSenha", Senha));
            parametros.Add(new SqlParameter("@cCGCMATRIZ", CGCMATRIZ));


            using (var reader = Conn.ExecutaComParametros(NomeProcedure, parametros))
            {
                while (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        objUser.CGC = String.IsNullOrEmpty(reader["CGC"].ToString()) ? "" : reader["CGC"].ToString().Trim();
                        objUser.CCUSTO = String.IsNullOrEmpty(reader["CCUSTO"].ToString()) ? "" : reader["CCUSTO"].ToString().Trim();
                        objUser.REQUISIT = String.IsNullOrEmpty(reader["REQUISIT"].ToString()) ? "" : reader["REQUISIT"].ToString().Trim();
                        objUser.NOME = String.IsNullOrEmpty(reader["NOME"].ToString()) ? "" : reader["NOME"].ToString().Trim();
                        objUser.EMAIL = String.IsNullOrEmpty(reader["EMAIL"].ToString()) ? "" : reader["EMAIL"].ToString().Trim();
                        objUser.NUMTENTATIVAS = (Int16)reader["NUMTENTATIVAS"];
                    }
                    reader.NextResult();
                }
            }

            Conn.Dispose();

            return objUser;

        }

        public static int ATUALIZA_TENTATIVA_LOGIN(string Connection, string Login, string CGCMATRIZ)
        {
            ConexaoSQLServer Conn = new ConexaoSQLServer(Connection);

            string NomeProcedure = "SP_MDS_LOGINS_TENTATIVAS_UPD";

            List<SqlParameter> parametros = new List<SqlParameter>();
            parametros.Add(new SqlParameter("@cLogin", Login));
            parametros.Add(new SqlParameter("@cCGCMatriz", CGCMATRIZ));

            int numTentativas = 0;

            using (var reader = Conn.ExecutaComParametros(NomeProcedure, parametros))
            {
                while (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        numTentativas = (Int16)reader["NUMTENTATIVAS"];
                    }
                    reader.NextResult();
                }
            }

            Conn.Dispose();

            return numTentativas;
        }

        public static void DESTRAVA_LOGIN(string Connection, string Login, string CGCMATRIZ)
        {
            ConexaoSQLServer Conn = new ConexaoSQLServer(Connection);

            string NomeProcedure = "SP_MDS_LOGINS_DESTRAVA";

            List<SqlParameter> parametros = new List<SqlParameter>();
            parametros.Add(new SqlParameter("@cLogin", Login));
            parametros.Add(new SqlParameter("@cCGCMatriz", CGCMATRIZ));


            var reader = Conn.ExecutaComParametros(NomeProcedure, parametros);

            Conn.Dispose();
        }

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
                        objUser.RefreshToken = String.IsNullOrEmpty(reader["RefreshToken"].ToString()) ? "" : reader["RefreshToken"].ToString().Trim();
                        objUser.RefreshTokenExpiryTime = (DateTime?)reader["RefreshTokenExpiryTime"];
                    }
                    reader.NextResult();
                }
            }

            Conn.Dispose();

            return objUser;

        }
    }
}
