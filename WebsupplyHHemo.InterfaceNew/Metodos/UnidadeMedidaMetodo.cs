using Newtonsoft.Json;
using SgiConnection;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using WebsupplyHHemo.InterfaceNew.Model;

namespace WebsupplyHHemo.InterfaceNew.Metodos
{
    public class UnidadeMedidaMetodo
    {
        static int _intNumTransacao = 0;
        static int _intNumServico = 0;
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

        public async Task<object> CadastraAtualiza(string CGC, string CC, string REQ)
        {
            dynamic retorno = new
            {
                strMensagem = string.Empty,
                status = false
            };

            Class_Log_Hhemo objLog;

            try
            {
                // Cria a Model para receber os dados da API
                UnidadeMedidaModel unidadeMedida = new UnidadeMedidaModel();

                // Cria o Cliente Http
                HttpClient cliente = new HttpClient();

                // Gera Log
                objLog = new Class_Log_Hhemo("For" + Mod_Gerais.RetornaIdentificador(), intNumTransacao, _intNumServico,
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
                unidadeMedida = JsonConvert.DeserializeObject<UnidadeMedidaModel>(responseBody);

                // Realiza a Chamada do Banco
                Conexao conn = new Conexao(Mod_Gerais.ConnectionString());

                ArrayList arrParam = new ArrayList();

                arrParam.Add(new Parametro("@iCOD_UNIDADE_TIPO", unidadeMedida.CodUnidade.ToString(), SqlDbType.Int, 4, ParameterDirection.Input));
                arrParam.Add(new Parametro("@cDESCRICAO", unidadeMedida.Descricao == "" ? null : unidadeMedida.Descricao.ToString(), SqlDbType.VarChar, 50, ParameterDirection.Input));
                arrParam.Add(new Parametro("@cCGC", CGC, SqlDbType.VarChar, 15, ParameterDirection.Input));
                arrParam.Add(new Parametro("@cCC", CC, SqlDbType.VarChar, 15, ParameterDirection.Input));
                arrParam.Add(new Parametro("@cREQ", REQ, SqlDbType.VarChar, 15, ParameterDirection.Input));
                arrParam.Add(new Parametro("@cAcao", unidadeMedida.CodUnidade == null ? "INC" : "ALT", SqlDbType.VarChar, 15, ParameterDirection.Input));

                ArrayList arrOut = new ArrayList();

                conn.ExecuteStoredProcedure(new StoredProcedure("SP_HHEMO_EMPRESAS_INSUPD", arrParam), ref arrOut);

                // Caso de certo a gravação no banco de dados, retorna true
                retorno.status = true;

                return retorno;
            }
            catch (Exception ex)
            {
                // Estrutura o Erro
                retorno.strMensagem = ex.Message;
                retorno.status = false;

                // Gera Log
                objLog = new Class_Log_Hhemo("For" + Mod_Gerais.RetornaIdentificador(), intNumTransacao, 6,
                                 1, -1, "", null, "Erro em " + Mod_Gerais.MethodName() + " :" + retorno.strMensagem,
                                 "", "", "", Mod_Gerais.MethodName());
                objLog.GravaLog();
                objLog = null;

                // Retorna Falso
                return retorno;
            }
        }
    }
}
