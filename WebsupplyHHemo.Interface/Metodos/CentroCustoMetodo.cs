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
using WebsupplyHHemo.Interface.Model;

namespace WebsupplyHHemo.Interface.Metodos
{
    public class CentroCustoMetodo
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

        public async Task<bool> CadastraAtualiza(string CGCMatriz)
        {
            string strMensagem = string.Empty;
            bool retorno = false;
            Class_Log_Hhemo objLog;

            try
            {
                // Cria a Model para receber os dados da API
                CentroCustoModel centroCusto = new CentroCustoModel();

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
                centroCusto = JsonConvert.DeserializeObject<CentroCustoModel>(responseBody);

                // Realiza a Chamada do Banco
                Conexao conn = new Conexao(Mod_Gerais.ConnectionString());

                ArrayList arrParam = new ArrayList();

                arrParam.Add(new Parametro("@cCentroDeCusto", centroCusto.CentroCusto == "" ? null : centroCusto.CentroCusto.ToString(), SqlDbType.VarChar, 15, ParameterDirection.Input));
                arrParam.Add(new Parametro("@cDescricao", centroCusto.Descricao == "" ? null : centroCusto.Descricao.ToString(), SqlDbType.VarChar, 60, ParameterDirection.Input));
                arrParam.Add(new Parametro("@cCGCMatriz", CGCMatriz, SqlDbType.VarChar, 15, ParameterDirection.Input));
                arrParam.Add(new Parametro("@cTipo", centroCusto.Tipo.ToString(), SqlDbType.Int, 4, ParameterDirection.Input));
                arrParam.Add(new Parametro("@cCentroDeCusto_Sup", centroCusto.CentroDeCusto_Sup.ToString(), SqlDbType.Int, 4, ParameterDirection.Input));

                ArrayList arrOut = new ArrayList();

                conn.ExecuteStoredProcedure(new StoredProcedure("SP_HHEMO_CCUSTO_INC", arrParam), ref arrOut);

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
