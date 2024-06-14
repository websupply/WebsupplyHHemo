using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using SgiConnection;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebsupplyHHemo.Interface.Funcoes;
using WebsupplyHHemo.Interface.Model;
using System.Net.Http;

namespace WebsupplyHHemo.Interface.Metodos
{
    public class FornecedorMetodo
    {
        static int _intNumTransacao = 0;
        static int _intNumServico = 11;
        string strIdentificador = "For" + Mod_Gerais.RetornaIdentificador();

        public string strMensagem = string.Empty;
        
        // Paramêtros de Controle da Classe
        public int intCodForWebsupply = 0;
        public string strCodForProtheus = string.Empty;

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

        public async Task<object> Cadastra()
        {
            bool retorno = false;
            Class_Log_Hhemo objLog;

            try
            {
                // Cria o Cliente Http
                HttpClient cliente = new HttpClient();

                // Gera Log
                objLog = new Class_Log_Hhemo("Cad" + strIdentificador, intNumTransacao, _intNumServico,
                                 0, 0, "", null, "Chamada a API Rest - Método " + Mod_Gerais.MethodName(),
                                 "L", "", "", Mod_Gerais.MethodName());
                objLog.GravaLog();
                objLog = null;

                // Pega a URL do Serviço
                Class_Servico objServico = new Class_Servico();
                if (objServico.CarregaDados(_intNumServico, "", strIdentificador, intNumTransacao) == false)
                {
                    objLog = new Class_Log_Hhemo(strIdentificador, intNumTransacao, _intNumServico,
                                                       1, -1, "", null, "Erro ao recuperar dados do serviço",
                                                       "", "", "", Mod_Gerais.MethodName());
                    objLog.GravaLog();
                    objLog = null;
                    strMensagem = "Erro ao recuperar dados do serviço";
                    return false;
                }
                else
                { _intNumTransacao -= 1; }

                // Realiza a Chamada do Banco
                Conexao conn = new Conexao(Mod_Gerais.ConnectionString());

                // Cria o Parametro da query do banco
                ArrayList arrParam = new ArrayList();
                
                arrParam.Add(new Parametro("@iID_Cadastro", intCodForWebsupply, SqlDbType.Int, 4, ParameterDirection.Input));

                ArrayList arrOut = new ArrayList();
                DataTable DadosFornecedor = conn.ExecuteStoredProcedure(new StoredProcedure("SP_HHEMO_FORNECEDORES_API_SEL", arrParam), ref arrOut).Tables[0];

                // Encerra a Conexão com Banco de Dados
                conn.Dispose();

                if (DadosFornecedor.Rows.Count > 0)
                {
                    // Estrutura a Model
                    FornecedorModel fornecedor = new FornecedorModel
                    {
                        tokenid = "HH@2021!%",
                        M0_CODIGO = "01",
                        M0_CODFIL = DadosFornecedor.Rows[0]["M0_CODFIL"].ToString().Trim(),
                        UUID_WEBSUPPLY = DadosFornecedor.Rows[0]["UUID_WEBSUPPLY"].ToString().Trim(),
                        A2_XTPFOR1 = DadosFornecedor.Rows[0]["A2_XTPFOR1"].ToString().Trim(),
                        A2_COD = DadosFornecedor.Rows[0]["A2_COD"].ToString().Trim(),
                        A2_LOJA = DadosFornecedor.Rows[0]["A2_LOJA"].ToString().Trim(),
                        A2_TIPO = DadosFornecedor.Rows[0]["A2_TIPO"].ToString().Trim(),
                        A2_NOME = DadosFornecedor.Rows[0]["A2_NOME"].ToString().Trim(),
                        A2_NREDUZ = DadosFornecedor.Rows[0]["A2_NREDUZ"].ToString().Trim(),
                        A2_CGC = DadosFornecedor.Rows[0]["A2_CGC"].ToString().Trim(),
                        A2_INSCR = DadosFornecedor.Rows[0]["A2_INSCR"].ToString().Trim(),
                        A2_END = DadosFornecedor.Rows[0]["A2_END"].ToString().Trim(),
                        A2_MUN = DadosFornecedor.Rows[0]["A2_MUN"].ToString().Trim(),
                        A2_EST = DadosFornecedor.Rows[0]["A2_EST"].ToString().Trim(),
                        A2_CEP = DadosFornecedor.Rows[0]["A2_CEP"].ToString().Trim(),
                        A2_BAIRRO = DadosFornecedor.Rows[0]["A2_BAIRRO"].ToString().Trim(),
                        A2_CODMUN = DadosFornecedor.Rows[0]["A2_CODMUN"].ToString().Trim(),
                        A2_DDD = DadosFornecedor.Rows[0]["A2_DDD"].ToString().Trim(),
                        A2_TEL = DadosFornecedor.Rows[0]["A2_TEL"].ToString().Trim(),
                        A2_FORMA = DadosFornecedor.Rows[0]["A2_FORMA"].ToString().Trim(),
                        A2_TPCONTA = DadosFornecedor.Rows[0]["A2_TPCONTA"].ToString().Trim(),
                        A2_BANCO = DadosFornecedor.Rows[0]["A2_BANCO"].ToString().Trim(),
                        A2_AGENCIA = DadosFornecedor.Rows[0]["A2_AGENCIA"].ToString().Trim(),
                        A2_DVAGE = DadosFornecedor.Rows[0]["A2_DVAGE"].ToString().Trim(),
                        A2_NUMCON = DadosFornecedor.Rows[0]["A2_NUMCON"].ToString().Trim(),
                        A2_DVCONTA = DadosFornecedor.Rows[0]["A2_DVCONTA"].ToString().Trim(),
                        A2_PIX = DadosFornecedor.Rows[0]["A2_PIX"].ToString().Trim(),
                        A2_MSBLQL = DadosFornecedor.Rows[0]["A2_MSBLQL"].ToString().Trim()
                    };

                    // Serializa o objeto para JSON
                    string jsonRequestBody = JsonConvert.SerializeObject(fornecedor);

                    // Adiciona o JSON como conteúdo da requisição
                    var content = new StringContent(jsonRequestBody, Encoding.UTF8, "application/json");

                    // Define os parâmetros e cria a chamada
                    var request = new HttpRequestMessage
                    {
                        Method = HttpMethod.Post,
                        RequestUri = new Uri(objServico.strURL),
                        Content = content
                    };

                    // Envia a requisição
                    var response = await cliente.SendAsync(request).ConfigureAwait(false);
                    response.EnsureSuccessStatusCode();

                    var responseBody = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

                    // Trata o Retorno e aloca no objeto
                    JArray retornoAPI = JArray.Parse(responseBody);

                    // Verifica se tem retorno
                    if (retornoAPI.Count > 0)
                    {
                        // Precisa definir o que vai trazer de retorno
                        // dependemos do Marcio Okada pra finalizar
                        // este passo

                        strCodForProtheus = "123456";
                    }

                    // Define a mensagem de sucesso
                    strMensagem = $"Fornecedor(a) {fornecedor.A2_NOME} cadastrado(a) com sucesso.";

                    // Gera Log
                    objLog = new Class_Log_Hhemo(strIdentificador, intNumTransacao, _intNumServico,
                                     0, 0, "", null, strMensagem,
                                     "L", "", "", Mod_Gerais.MethodName());
                    objLog.GravaLog();
                    objLog = null;
                }
                else
                {
                    // Define a mensagem de erro
                    strMensagem = $"Não foi possível realizar a operação, pois não foi retornando nenhum dado referente ao CodFornecedor {intCodForWebsupply}";

                    // Gera Log
                    objLog = new Class_Log_Hhemo(strIdentificador, intNumTransacao, _intNumServico,
                                     0, 0, "", null, strMensagem,
                                     "L", "", "", Mod_Gerais.MethodName());
                    objLog.GravaLog();
                    objLog = null;

                    return false;
                }

                return true;
            }
            catch (Exception ex)
            {
                // Estrutura o Erro
                strMensagem = ex.Message;

                // Gera Log
                objLog = new Class_Log_Hhemo(strIdentificador, intNumTransacao, _intNumServico,
                                 1, -1, "", null, strMensagem,
                                 "L", "", "", Mod_Gerais.MethodName());
                objLog.GravaLog();
                objLog = null;

                // Retorna Falso
                return false;
            }
        }

        public async Task<object> Atualiza()
        {
            bool retorno = false;
            Class_Log_Hhemo objLog;

            try
            {
                // Cria o Cliente Http
                HttpClient cliente = new HttpClient();

                // Gera Log
                objLog = new Class_Log_Hhemo("Alt" + strIdentificador, intNumTransacao, _intNumServico,
                                 0, 0, "", null, "Chamada a API Rest - Método " + Mod_Gerais.MethodName(),
                                 "L", "", "", Mod_Gerais.MethodName());
                objLog.GravaLog();
                objLog = null;

                // Pega a URL do Serviço
                Class_Servico objServico = new Class_Servico();
                if (objServico.CarregaDados(_intNumServico, "", strIdentificador, intNumTransacao) == false)
                {
                    objLog = new Class_Log_Hhemo(strIdentificador, intNumTransacao, _intNumServico,
                                                       1, -1, "", null, "Erro ao recuperar dados do serviço",
                                                       "", "", "", Mod_Gerais.MethodName());
                    objLog.GravaLog();
                    objLog = null;
                    strMensagem = "Erro ao recuperar dados do serviço";
                    return false;
                }
                else
                { _intNumTransacao -= 1; }

                // Realiza a Chamada do Banco
                Conexao conn = new Conexao(Mod_Gerais.ConnectionString());

                // Cria o Parametro da query do banco
                ArrayList arrParam = new ArrayList();

                arrParam.Add(new Parametro("@iID_Cadastro", intCodForWebsupply, SqlDbType.Int, 4, ParameterDirection.Input));

                ArrayList arrOut = new ArrayList();
                DataTable DadosFornecedor = conn.ExecuteStoredProcedure(new StoredProcedure("SP_HHEMO_FORNECEDORES_API_SEL", arrParam), ref arrOut).Tables[0];

                // Encerra a Conexão com Banco de Dados
                conn.Dispose();

                if (DadosFornecedor.Rows.Count > 0)
                {
                    // Estrutura a Model
                    FornecedorModel fornecedor = new FornecedorModel
                    {
                        tokenid = "HH@2021!%",
                        M0_CODIGO = "01",
                        M0_CODFIL = DadosFornecedor.Rows[0]["M0_CODFIL"].ToString().Trim(),
                        UUID_WEBSUPPLY = DadosFornecedor.Rows[0]["UUID_WEBSUPPLY"].ToString().Trim(),
                        A2_XTPFOR1 = DadosFornecedor.Rows[0]["A2_XTPFOR1"].ToString().Trim(),
                        A2_COD = DadosFornecedor.Rows[0]["A2_COD"].ToString().Trim(),
                        A2_LOJA = DadosFornecedor.Rows[0]["A2_LOJA"].ToString().Trim(),
                        A2_TIPO = DadosFornecedor.Rows[0]["A2_TIPO"].ToString().Trim(),
                        A2_NOME = DadosFornecedor.Rows[0]["A2_NOME"].ToString().Trim(),
                        A2_NREDUZ = DadosFornecedor.Rows[0]["A2_NREDUZ"].ToString().Trim(),
                        A2_CGC = DadosFornecedor.Rows[0]["A2_CGC"].ToString().Trim(),
                        A2_INSCR = DadosFornecedor.Rows[0]["A2_INSCR"].ToString().Trim(),
                        A2_END = DadosFornecedor.Rows[0]["A2_END"].ToString().Trim(),
                        A2_MUN = DadosFornecedor.Rows[0]["A2_MUN"].ToString().Trim(),
                        A2_EST = DadosFornecedor.Rows[0]["A2_EST"].ToString().Trim(),
                        A2_CEP = DadosFornecedor.Rows[0]["A2_CEP"].ToString().Trim(),
                        A2_BAIRRO = DadosFornecedor.Rows[0]["A2_BAIRRO"].ToString().Trim(),
                        A2_CODMUN = DadosFornecedor.Rows[0]["A2_CODMUN"].ToString().Trim(),
                        A2_DDD = DadosFornecedor.Rows[0]["A2_DDD"].ToString().Trim(),
                        A2_TEL = DadosFornecedor.Rows[0]["A2_TEL"].ToString().Trim(),
                        A2_FORMA = DadosFornecedor.Rows[0]["A2_FORMA"].ToString().Trim(),
                        A2_TPCONTA = DadosFornecedor.Rows[0]["A2_TPCONTA"].ToString().Trim(),
                        A2_BANCO = DadosFornecedor.Rows[0]["A2_BANCO"].ToString().Trim(),
                        A2_AGENCIA = DadosFornecedor.Rows[0]["A2_AGENCIA"].ToString().Trim(),
                        A2_DVAGE = DadosFornecedor.Rows[0]["A2_DVAGE"].ToString().Trim(),
                        A2_NUMCON = DadosFornecedor.Rows[0]["A2_NUMCON"].ToString().Trim(),
                        A2_DVCONTA = DadosFornecedor.Rows[0]["A2_DVCONTA"].ToString().Trim(),
                        A2_PIX = DadosFornecedor.Rows[0]["A2_PIX"].ToString().Trim(),
                        A2_MSBLQL = DadosFornecedor.Rows[0]["A2_MSBLQL"].ToString().Trim()
                    };

                    // Serializa o objeto para JSON
                    string jsonRequestBody = JsonConvert.SerializeObject(fornecedor);

                    // Adiciona o JSON como conteúdo da requisição
                    var content = new StringContent(jsonRequestBody, Encoding.UTF8, "application/json");

                    // Define os parâmetros e cria a chamada
                    var request = new HttpRequestMessage
                    {
                        Method = HttpMethod.Get,
                        RequestUri = new Uri(objServico.strURL),
                        Content = content
                    };

                    // Envia a requisição
                    var response = await cliente.SendAsync(request).ConfigureAwait(false);
                    response.EnsureSuccessStatusCode();

                    var responseBody = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

                    // Trata o Retorno e aloca no objeto
                    JArray retornoAPI = JArray.Parse(responseBody);

                    // Verifica se tem retorno
                    if (retornoAPI.Count > 0)
                    {
                        // Precisa definir o que vai trazer de retorno
                        // dependemos do Marcio Okada pra finalizar
                        // este passo
                    }

                    // Define a mensagem de sucesso
                    strMensagem = $"Fornecedor(a) {fornecedor.A2_NOME} Atualizado(a) com sucesso.";

                    // Gera Log
                    objLog = new Class_Log_Hhemo(strIdentificador, intNumTransacao, _intNumServico,
                                     0, 0, "", null, strMensagem,
                                     "L", "", "", Mod_Gerais.MethodName());
                    objLog.GravaLog();
                    objLog = null;
                }

                return true;
            }
            catch (Exception ex)
            {
                // Estrutura o Erro
                strMensagem = ex.Message;

                // Gera Log
                objLog = new Class_Log_Hhemo(strIdentificador, intNumTransacao, _intNumServico,
                                 1, -1, "", null, strMensagem,
                                 "L", "", "", Mod_Gerais.MethodName());
                objLog.GravaLog();
                objLog = null;

                // Retorna Falso
                return false;
            }
        }
    }
}
