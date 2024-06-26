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
using System.Runtime.InteropServices;

namespace WebsupplyHHemo.Interface.Metodos
{
    [ComVisible(true)]
    public class FornecedorMetodo
    {
        static int _intNumTransacao = 0;
        static int _intNumServico = 11;
        string strIdentificador = "For" + Mod_Gerais.RetornaIdentificador();

        public string strMensagem = string.Empty;
        
        // Paramêtros de Controle da Classe
        public int intCodForWebsupply = 0;
        public string strCodForProtheus = string.Empty;
        public string strCodLojaProtheus = string.Empty;

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

        public bool ConsomeWS()
        {
            bool retorno = false;
            Class_Log_Hhemo objLog;

            try
            {
                // Cria o Cliente Http
                HttpClient cliente = new HttpClient();

                // Gera Log
                objLog = new Class_Log_Hhemo(strIdentificador, intNumTransacao, _intNumServico,
                                 0, 0, "", null, "Inicio do Método " + Mod_Gerais.MethodName(),
                                 "L", intCodForWebsupply.ToString(), "", Mod_Gerais.MethodName());
                objLog.GravaLog();
                objLog = null;

                // Pega a URL do Serviço
                Class_Servico objServico = new Class_Servico();
                if (objServico.CarregaDados(_intNumServico, "", strIdentificador, intNumTransacao) == false)
                {
                    objLog = new Class_Log_Hhemo(strIdentificador, intNumTransacao, _intNumServico,
                                                       1, -1, "", null, "Erro ao recuperar dados do serviço",
                                                       "", intCodForWebsupply.ToString(), "", Mod_Gerais.MethodName());
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
                        tokenid = DadosFornecedor.Rows[0]["tokenid"].ToString().Trim(),
                        M0_CODIGO = DadosFornecedor.Rows[0]["M0_CODIGO"].ToString().Trim(),
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
                        A2_COD_MUN = DadosFornecedor.Rows[0]["A2_COD_MUN"].ToString().Trim(),
                        A2_DDD = DadosFornecedor.Rows[0]["A2_DDD"].ToString().Trim(),
                        A2_TEL = DadosFornecedor.Rows[0]["A2_TEL"].ToString().Trim(),
                        A2_FORMPAG = DadosFornecedor.Rows[0]["A2_FORMPAG"].ToString().Trim(),
                        A2_TIPCTA = DadosFornecedor.Rows[0]["A2_TIPCTA"].ToString().Trim(),
                        A2_BANCO = DadosFornecedor.Rows[0]["A2_BANCO"].ToString().Trim(),
                        A2_AGENCIA = DadosFornecedor.Rows[0]["A2_AGENCIA"].ToString().Trim(),
                        A2_DVAGE = DadosFornecedor.Rows[0]["A2_DVAGE"].ToString().Trim(),
                        A2_NUMCON = DadosFornecedor.Rows[0]["A2_NUMCON"].ToString().Trim(),
                        A2_DVCTA = DadosFornecedor.Rows[0]["A2_DVCTA"].ToString().Trim(),
                        A2_XCGCDEP = DadosFornecedor.Rows[0]["A2_XCGCDEP"].ToString().Trim(),
                        A2_MSBLQL = DadosFornecedor.Rows[0]["A2_MSBLQL"].ToString().Trim()
                    };

                    // Serializa o objeto para JSON
                    string jsonRequestBody = JsonConvert.SerializeObject(fornecedor);

                    // Atualiza o Identificador
                    strIdentificador = (fornecedor.A2_COD != String.Empty ? "Alt" : "Cad") + strIdentificador;

                    // Gera Log
                    objLog = new Class_Log_Hhemo(strIdentificador, intNumTransacao, _intNumServico,
                                     0, 0, jsonRequestBody, null, "Chamada a API Rest - Método " + Mod_Gerais.MethodName(),
                                     "L", intCodForWebsupply.ToString(), "", Mod_Gerais.MethodName());
                    objLog.GravaLog();
                    objLog = null;

                    // Adiciona o JSON como conteúdo da requisição
                    var content = new StringContent(jsonRequestBody, Encoding.UTF8, "application/json");

                    // Define os parâmetros e cria a chamada
                    var request = new HttpRequestMessage
                    {
                        Method = fornecedor.A2_COD != String.Empty ? HttpMethod.Put : HttpMethod.Post,
                        RequestUri = new Uri(objServico.strURL),
                        Content = content
                    };

                    // Envia a requisição
                    var response = cliente.SendAsync(request).ConfigureAwait(false).GetAwaiter().GetResult();

                    // Trata o Retorno da API
                    var responseBody = response.Content.ReadAsStringAsync().ConfigureAwait(false).GetAwaiter().GetResult();

                    // Gera Log com o retorno da API
                    objLog = new Class_Log_Hhemo(strIdentificador, intNumTransacao, _intNumServico,
                                     0, (int)response.StatusCode, responseBody, null, "Retorno da Chamada a API Rest - Método " + Mod_Gerais.MethodName(),
                                     "L", intCodForWebsupply.ToString(), "", Mod_Gerais.MethodName());
                    objLog.GravaLog();
                    objLog = null;

                    if (response.IsSuccessStatusCode)
                    {
                        response.EnsureSuccessStatusCode();

                        // Trata o Retorno e aloca no objeto
                        JArray retornoAPI = JArray.Parse(responseBody);

                        // Verifica se tem retorno
                        if (retornoAPI.Count > 0)
                        {
                            // Percorre Todos os Resultados
                            for (int i = 0; i < retornoAPI.Count; i++)
                            {
                                // Pega a Linha do Retorno
                                JObject linhaRetorno = JObject.Parse(retornoAPI[i].ToString());

                                // Sincroniza o Retorno da API com os Parametros
                                strCodForProtheus = linhaRetorno["A2_COD"].ToString().Trim();
                                strCodLojaProtheus = linhaRetorno["A2_LOJA"].ToString().Trim();

                                // Valida se algum dos códigos retornou vázio
                                // caso sim, devolve erro
                                if (strCodForProtheus == String.Empty || strCodLojaProtheus == String.Empty)
                                {
                                    strMensagem = $"Ocorreu um erro na chamada da aplicação - [{linhaRetorno["C_STATUS"].ToString().Trim()}] - A2_LOJA [{strCodLojaProtheus}] - A2_COD [{strCodLojaProtheus}]";

                                    return false;
                                }

                                // Caso o Fornecedor não tenha código do protheus e loja, armazena essa informações
                                // no banco
                                if (fornecedor.A2_COD == String.Empty)
                                {
                                    // Realiza a Chamada do Banco
                                    conn = new Conexao(Mod_Gerais.ConnectionString());

                                    // Cria o Parametro da query do banco
                                    ArrayList arrParam2 = new ArrayList();

                                    arrParam2.Add(new Parametro("@iID_Cadastro", intCodForWebsupply, SqlDbType.Int, 4, ParameterDirection.Input));
                                    arrParam2.Add(new Parametro("@cCod_for", strCodForProtheus, SqlDbType.Char, 15, ParameterDirection.Input));
                                    arrParam2.Add(new Parametro("@cLoja", strCodLojaProtheus, SqlDbType.Char, 10, ParameterDirection.Input));

                                    ArrayList arrOut2 = new ArrayList();

                                    conn.ExecuteStoredProcedure(new StoredProcedure("SP_HHEMO_WS_FORNECEDORES_COD_FOR_UPD", arrParam2), ref arrOut2);

                                    // Encerra a Conexão com Banco de Dados
                                    conn.Dispose();
                                }
                            }
                        }

                        // Define a mensagem de sucesso
                        strMensagem = $"Fornecedor(a) {fornecedor.A2_NOME} do codigo [{(strCodForProtheus != String.Empty ? strCodForProtheus : fornecedor.A2_COD )}] da loja [{(strCodLojaProtheus != String.Empty ? strCodLojaProtheus : fornecedor.A2_LOJA)}] {(fornecedor.A2_COD != String.Empty ? "atualizado(a)" : "cadastrado(a)")} com sucesso.";

                        // Gera Log
                        objLog = new Class_Log_Hhemo(strIdentificador, intNumTransacao, _intNumServico,
                                         0, 0, "", null, strMensagem,
                                         "L", intCodForWebsupply.ToString(), "", Mod_Gerais.MethodName());
                        objLog.GravaLog();
                        objLog = null;

                        return true;
                    }
                    else
                    {
                        // Define a mensagem de erro
                        strMensagem = $"Ocorreu o erro [{response.StatusCode}] ao processar a solicitação, verifique nos logs e tente novamente.";


                        // Gera Log
                        objLog = new Class_Log_Hhemo(strIdentificador, intNumTransacao, _intNumServico,
                                         0, (int)response.StatusCode, "", null, strMensagem,
                                         "L", intCodForWebsupply.ToString(), "", Mod_Gerais.MethodName());
                        objLog.GravaLog();
                        objLog = null;

                        return false;
                    }
                }
                else
                {
                    // Define a mensagem de erro
                    strMensagem = $"Não foi possível realizar a operação, pois não foi retornando nenhum dado referente ao CodFornecedor {intCodForWebsupply}";

                    // Gera Log
                    objLog = new Class_Log_Hhemo(strIdentificador, intNumTransacao, _intNumServico,
                                     0, 0, "", null, strMensagem,
                                     "L", intCodForWebsupply.ToString(), "", Mod_Gerais.MethodName());
                    objLog.GravaLog();
                    objLog = null;

                    return false;
                }
            }
            catch (Exception ex)
            {
                // Estrutura o Erro
                strMensagem = ex.Message;

                // Gera Log
                objLog = new Class_Log_Hhemo(strIdentificador, intNumTransacao, _intNumServico,
                                 1, -1, "", null, strMensagem,
                                 "L", intCodForWebsupply.ToString(), "", Mod_Gerais.MethodName());
                objLog.GravaLog();
                objLog = null;

                // Retorna Falso
                return false;
            }
        }
    }
}
