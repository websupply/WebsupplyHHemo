using Newtonsoft.Json;
using SgiConnection;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using WebsupplyHHemo.InterfaceNew.Model;

namespace WebsupplyHHemo.InterfaceNew.Metodos
{
    public class UsuarioMetodo
    {
        static int _intNumTransacao = 0;
        private static int intNumTransacao
        {
            get
            {
                _intNumTransacao += 1;
                return _intNumTransacao;
            }
            set
            {
                _intNumTransacao = value;
            }
        }

        public async Task<bool> CadastraAtualiza(string CGC, string CCusto, string Requisit)
        {
            string strMensagem = string.Empty;
            bool retorno = false;
            Class_Log_Hhemo objLog;

            try
            {
                // Cria a Model para receber os dados da API
                UsuarioModel usuario = new UsuarioModel();

                // Cria o Cliente Http
                HttpClient cliente = new HttpClient();

                // Gera Log
                objLog = new Class_Log_Hhemo("For" + Mod_Gerais.RetornaIdentificador(), intNumTransacao, 6,
                                 0, 0, "", null, "Chamada a API Rest - Método " + Mod_Gerais.MethodName(),
                                 "L", "", "", Mod_Gerais.MethodName());
                objLog.GravaLog();
                objLog = null;

                // Define os Parametros e Cria a Chamada
                string URI = "";
                HttpResponseMessage response = await cliente.GetAsync(URI);
                response.EnsureSuccessStatusCode();

                // Recebe o retorno
                string responseBody = await response.Content.ReadAsStringAsync();

                // Trata o Retorno e aloca no objeto
                usuario = JsonConvert.DeserializeObject<UsuarioModel>(responseBody);

                // Realiza a Chamada do Banco
                Conexao conn = new Conexao(Mod_Gerais.ConnectionString());

                ArrayList arrParam = new ArrayList();

                arrParam.Add(new Parametro("@cCodigo", usuario.CodUsuario.ToString(), SqlDbType.Int, 4, ParameterDirection.Input));
                arrParam.Add(new Parametro("@cNOMEREQUISIT", usuario.Nome == "" ? null : usuario.Nome.ToString(), SqlDbType.Char, 30, ParameterDirection.Input));
                arrParam.Add(new Parametro("@cEMAIL", usuario.Email == "" ? null : usuario.Email.ToString(), SqlDbType.Char, 60, ParameterDirection.Input));
                arrParam.Add(new Parametro("@cCompraContrato", usuario.Comprador == "" ? "N" : usuario.Comprador.ToString(), SqlDbType.Char, 1, ParameterDirection.Input));
                arrParam.Add(new Parametro("@cCotacao", usuario.Solicitante == "" ? "N" : usuario.Solicitante.ToString(), SqlDbType.Char, 1, ParameterDirection.Input));
                arrParam.Add(new Parametro("@cINDEXCLUI", usuario.Status == "" ? "N" : usuario.Status.ToString(), SqlDbType.Char, 1, ParameterDirection.Input));
                arrParam.Add(new Parametro("@cCGC", CGC, SqlDbType.Char, 15, ParameterDirection.Input));
                arrParam.Add(new Parametro("@cCCUSTO", CCusto, SqlDbType.Char, 15, ParameterDirection.Input));
                arrParam.Add(new Parametro("@cREQUISIT", Requisit, SqlDbType.Char, 10, ParameterDirection.Input));

                ArrayList arrOut = new ArrayList();

                conn.ExecuteStoredProcedure(new StoredProcedure("SP_CENTROSCUSTO_INSUPD", arrParam), ref arrOut);

                // Caso de certo a gravação no banco de dados, retorna true
                return true;
            }
            catch (Exception ex)
            {
                // Estrutura o Erro
                strMensagem = ex.Message;

                // Gera Log
                objLog = new Class_Log_Hhemo("For" + Mod_Gerais.RetornaIdentificador(), intNumTransacao, 6,
                                 1, -1, "", null, "Erro em " + Mod_Gerais.MethodName() + " :" + strMensagem,
                                 "", "", "", Mod_Gerais.MethodName());
                objLog.GravaLog();
                objLog = null;

                // Retorna Falso
                return false;
            }
        }
    }
}
