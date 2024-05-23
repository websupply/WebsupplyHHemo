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
    public class PlanoContaMetodo
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
                PlanoContaModel planoConta = new PlanoContaModel();

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
                planoConta = JsonConvert.DeserializeObject<PlanoContaModel>(responseBody);

                // Realiza a Chamada do Banco
                Conexao conn = new Conexao(Mod_Gerais.ConnectionString());

                ArrayList arrParam = new ArrayList();

                arrParam.Add(new Parametro("@cCGC", CGC.ToString(), SqlDbType.VarChar, 15, ParameterDirection.Input));
                arrParam.Add(new Parametro("@cCCONTABIL", planoConta.ContaContabil == "" ? null : planoConta.ContaContabil.ToString(), SqlDbType.Char, 20, ParameterDirection.Input));
                arrParam.Add(new Parametro("@vDescricao", planoConta.Descricao == "" ? null : planoConta.Descricao.ToString(), SqlDbType.Char, 20, ParameterDirection.Input));
                arrParam.Add(new Parametro("@vNatureza", planoConta.Natureza == "" ? null : planoConta.Natureza.ToString(), SqlDbType.VarChar, 50, ParameterDirection.Input));
                arrParam.Add(new Parametro("@vCondicao", planoConta.Condicao == "" ? null : planoConta.Condicao.ToString(), SqlDbType.VarChar, 50, ParameterDirection.Input));
                arrParam.Add(new Parametro("@cTransferencia", planoConta.Transferencia == "" ? null : planoConta.Transferencia.ToString(), SqlDbType.Char, 1, ParameterDirection.Input));
                arrParam.Add(new Parametro("@cSuperior", planoConta.Superior == "" ? null : planoConta.Superior.ToString(), SqlDbType.Char, 20, ParameterDirection.Input));
                arrParam.Add(new Parametro("@cIndAtivo", planoConta.Status == "" ? "S" : planoConta.Status.ToString(), SqlDbType.Char, 1, ParameterDirection.Input));
                arrParam.Add(new Parametro("@cGerente", planoConta.Gerente == "" ? "" : planoConta.Gerente.ToString(), SqlDbType.Char, 1, ParameterDirection.Input));
                arrParam.Add(new Parametro("@cInvestimento", planoConta.Investimento == "" ? "S" : planoConta.Investimento.ToString(), SqlDbType.Char, 1, ParameterDirection.Input));
                arrParam.Add(new Parametro("@iTipo", planoConta.Tipo.ToString(), SqlDbType.Int, 4, ParameterDirection.Input));
                arrParam.Add(new Parametro("@vAcao", "Incluir", SqlDbType.VarChar, 10, ParameterDirection.Input));

                ArrayList arrOut = new ArrayList();

                conn.ExecuteStoredProcedure(new StoredProcedure("SP_HHEMO_CONTACONTABIL_INSUPD", arrParam), ref arrOut);

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

        public async Task<bool> Exclui(string CGC)
        {
            string strMensagem = string.Empty;
            bool retorno = false;
            Class_Log_Hhemo objLog;

            try
            {
                // Cria a Model para receber os dados da API
                PlanoContaModel planoConta = new PlanoContaModel();

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
                planoConta = JsonConvert.DeserializeObject<PlanoContaModel>(responseBody);

                // Realiza a Chamada do Banco
                Conexao conn = new Conexao(Mod_Gerais.ConnectionString());

                ArrayList arrParam = new ArrayList();

                arrParam.Add(new Parametro("@cCGC", CGC.ToString(), SqlDbType.VarChar, 15, ParameterDirection.Input));
                arrParam.Add(new Parametro("@cCCONTABIL", planoConta.ContaContabil == "" ? null : planoConta.ContaContabil.ToString(), SqlDbType.Char, 20, ParameterDirection.Input));
                arrParam.Add(new Parametro("@vDescricao", planoConta.Descricao == "" ? null : planoConta.Descricao.ToString(), SqlDbType.Char, 20, ParameterDirection.Input));
                arrParam.Add(new Parametro("@vNatureza", planoConta.Natureza == "" ? null : planoConta.Natureza.ToString(), SqlDbType.VarChar, 50, ParameterDirection.Input));
                arrParam.Add(new Parametro("@vCondicao", planoConta.Condicao == "" ? null : planoConta.Condicao.ToString(), SqlDbType.VarChar, 50, ParameterDirection.Input));
                arrParam.Add(new Parametro("@cTransferencia", planoConta.Transferencia == "" ? null : planoConta.Transferencia.ToString(), SqlDbType.Char, 1, ParameterDirection.Input));
                arrParam.Add(new Parametro("@cSuperior", planoConta.Superior == "" ? null : planoConta.Superior.ToString(), SqlDbType.Char, 20, ParameterDirection.Input));
                arrParam.Add(new Parametro("@cIndAtivo", planoConta.Status == "" ? "S" : planoConta.Status.ToString(), SqlDbType.Char, 1, ParameterDirection.Input));
                arrParam.Add(new Parametro("@cGerente", planoConta.Gerente == "" ? "" : planoConta.Gerente.ToString(), SqlDbType.Char, 1, ParameterDirection.Input));
                arrParam.Add(new Parametro("@cInvestimento", planoConta.Investimento == "" ? "S" : planoConta.Investimento.ToString(), SqlDbType.Char, 1, ParameterDirection.Input));
                arrParam.Add(new Parametro("@iTipo", planoConta.Tipo.ToString(), SqlDbType.Int, 4, ParameterDirection.Input));
                arrParam.Add(new Parametro("@vAcao", "Excluir", SqlDbType.VarChar, 10, ParameterDirection.Input));

                ArrayList arrOut = new ArrayList();

                conn.ExecuteStoredProcedure(new StoredProcedure("SP_HHEMO_CONTACONTABIL_INSUPD", arrParam), ref arrOut);

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
