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
    public class FormaPagtoMetodo
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

        public async Task<bool> CadastraAtualiza(string CGC)
        {
            string strMensagem = string.Empty;
            bool retorno = false;
            Class_Log_Hhemo objLog;

            try
            {
                // Cria a Model para receber os dados da API
                FormaPagtoModel formaPagto = new FormaPagtoModel();

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
                formaPagto = JsonConvert.DeserializeObject<FormaPagtoModel>(responseBody);

                // Realiza a Chamada do Banco
                Conexao conn = new Conexao(Mod_Gerais.ConnectionString());

                ArrayList arrParam = new ArrayList();

                arrParam.Add(new Parametro("@Cod_FPagto", formaPagto.CodFormaPagto.ToString(), SqlDbType.Int, 4, ParameterDirection.Input));
                arrParam.Add(new Parametro("@descricao", formaPagto.Descricao == "" ? null : formaPagto.Descricao.ToString(), SqlDbType.VarChar, 15, ParameterDirection.Input));
                arrParam.Add(new Parametro("@CodTipo", formaPagto.CodTipo == "" ? null : formaPagto.CodTipo.ToString(), SqlDbType.VarChar, 15, ParameterDirection.Input));
                arrParam.Add(new Parametro("@dias", formaPagto.Dias.ToString(), SqlDbType.Int, 4, ParameterDirection.Input));
                arrParam.Add(new Parametro("@CGC", CGC, SqlDbType.VarChar, 15, ParameterDirection.Input));

                ArrayList arrOut = new ArrayList();

                conn.ExecuteStoredProcedure(new StoredProcedure("SP_HHEMO_FORMAS_PAGTO_INSUPD", arrParam), ref arrOut);

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
