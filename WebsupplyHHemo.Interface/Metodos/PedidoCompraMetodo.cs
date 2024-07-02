using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using SgiConnection;
using System;
using System.Collections;
using System.Data;
using System.Text;
using WebsupplyHHemo.Interface.Funcoes;
using WebsupplyHHemo.Interface.Model;
using System.Net.Http;
using System.Collections.Generic;

namespace WebsupplyHHemo.Interface.Metodos
{
    public class PedidoCompraMetodo
    {
        static int _intNumTransacao = 0;
        static int _intNumServico = 12;
        string strIdentificador = "PedCom" + Mod_Gerais.RetornaIdentificador();

        public string strMensagem = string.Empty;

        // Paramêtros de Controle da Classe
        public int intCodPedComWebsupply = 0;
        public string strCodPedComProtheus = string.Empty;
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
                                 "L", intCodPedComWebsupply.ToString(), "", Mod_Gerais.MethodName());
                objLog.GravaLog();
                objLog = null;

                // Pega a URL do Serviço
                Class_Servico objServico = new Class_Servico();
                if (objServico.CarregaDados(_intNumServico, "", strIdentificador, intNumTransacao) == false)
                {
                    objLog = new Class_Log_Hhemo(strIdentificador, intNumTransacao, _intNumServico,
                                                       1, -1, "", null, "Erro ao recuperar dados do serviço",
                                                       "", intCodPedComWebsupply.ToString(), "", Mod_Gerais.MethodName());
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

                arrParam.Add(new Parametro("@iCL_CDG", intCodPedComWebsupply, SqlDbType.Int, 4, ParameterDirection.Input));

                ArrayList arrOut = new ArrayList();
                DataTable DadosPedidoCompra = conn.ExecuteStoredProcedure(new StoredProcedure("[procedure para consultar pedido]", arrParam), ref arrOut).Tables[0];

                // Encerra a Conexão com Banco de Dados
                conn.Dispose();

                if (DadosPedidoCompra.Rows.Count > 0)
                {
                    // Estrutura a Model
                    PedidoCompraModel pedidoCompra = new PedidoCompraModel
                    {
                        tokenid = DadosPedidoCompra.Rows[0]["tokenid"].ToString().Trim(),
                        M0_CODFIL = DadosPedidoCompra.Rows[0]["M0_CODFIL"].ToString().Trim(),
                        M0_CODIGO = DadosPedidoCompra.Rows[0]["M0_CODIGO"].ToString().Trim(),
                        C7_COND = DadosPedidoCompra.Rows[0]["C7_COND"].ToString().Trim(),
                        C7_DESPESA_TOTAL = (decimal)DadosPedidoCompra.Rows[0]["C7_DESPESA_TOTAL"],
                        C7_EMISSAO = DadosPedidoCompra.Rows[0]["C7_EMISSAO"].ToString().Trim(),
                        C7_FILENT = DadosPedidoCompra.Rows[0]["C7_FILENT"].ToString().Trim(),
                        C7_FORNECE = DadosPedidoCompra.Rows[0]["C7_FORNECE"].ToString().Trim(),
                        C7_LOJA = DadosPedidoCompra.Rows[0]["C7_LOJA"].ToString().Trim(),
                        C7_NUM = DadosPedidoCompra.Rows[0]["C7_NUM"].ToString().Trim(),
                        UUID_WEBSUPPLY = DadosPedidoCompra.Rows[0]["UUID_WEBSUPPLY"].ToString().Trim(),
                        C7_SEGURO_TOTAL = (decimal)DadosPedidoCompra.Rows[0]["C7_SEGURO_TOTAL"],
                        C7_VALFRE_TOTAL = (decimal)DadosPedidoCompra.Rows[0]["C7_VALFRE_TOTAL"],
                        C7_VLDESC_TOTAL = (decimal)DadosPedidoCompra.Rows[0]["C7_VLDESC_TOTAL"],
                        C7_MSBLQL = DadosPedidoCompra.Rows[0]["A2_MSBLQL"].ToString().Trim()
                    };

                    // Estrutura a Model de Itens
                    List<PedidoCompraModel.Item> itensPedidoCompra = new List<PedidoCompraModel.Item>();

                    // Realiza a Chamada do Banco
                    conn = new Conexao(Mod_Gerais.ConnectionString());

                    // Cria o Parametro da query do banco
                    arrParam = new ArrayList();

                    arrParam.Add(new Parametro("@iCL_CDG", intCodPedComWebsupply, SqlDbType.Int, 4, ParameterDirection.Input));

                    arrOut = new ArrayList();
                    DataTable DadosItens = conn.ExecuteStoredProcedure(new StoredProcedure("[procedure para consultar itens do pedido]", arrParam), ref arrOut).Tables[0];

                    // Encerra a Conexão com Banco de Dados
                    conn.Dispose();

                    // Verifica se Existe itens para o pedido e caso sim, traz
                    // os itens e caso não, retorna erro
                    if (DadosItens.Rows.Count > 0)
                    {
                        for(int i = 0; i < DadosItens.Rows.Count; i++)
                        {
                            // Pega a Linha do Registro
                            var registro = DadosItens.Rows[i];

                            // Carrega os Dados do item
                            PedidoCompraModel.Item item = new PedidoCompraModel.Item
                            {
                                C7_CC = registro["C7_CC"].ToString().Trim(),
                                C7_CONTA = registro["C7_CONTA"].ToString().Trim(),
                                C7_DATPRF = registro["C7_DATPRF"].ToString().Trim(),
                                C7_DESPESA = (decimal)registro["C7_DESPESA"],
                                C7_ITEM = registro["C7_ITEM"].ToString().Trim(),
                                C7_OBS = registro["C7_OBS"].ToString().Trim(),
                                C7_PRECO = (decimal)registro["C7_PRECO"],
                                C7_PRODUTO = registro["C7_PRODUTO"].ToString().Trim(),
                                C7_QUANT = (int)registro["C7_QUANT"],
                                C7_SEGURO = (decimal)registro["C7_SEGURO"],
                                C7_TOTAL = (decimal)registro["C7_TOTAL"],
                                C7_VALFRE = (decimal)registro["C7_VALFRE"],
                                C7_VALICM = (decimal)registro["C7_VALICM"],
                                C7_VALIPI = (decimal)registro["C7_VALIPI"],
                                C7_VLDESC = (decimal)registro["C7_VLDESC"],
                                C7_MSBLQL = registro["C7_MSBLQL"].ToString().Trim(),
                            };

                            // Adiciona a Array de Itens
                            itensPedidoCompra.Add(item);
                        }
                    }
                    else
                    {
                        // Define a mensagem de erro
                        strMensagem = $"Não foi possível realizar a operação, pois não foi retornando nenhum item associado ao Pedido Nº {intCodPedComWebsupply}";

                        // Gera Log
                        objLog = new Class_Log_Hhemo(strIdentificador, intNumTransacao, _intNumServico,
                                         0, 0, "", null, strMensagem,
                                         "L", intCodPedComWebsupply.ToString(), "", Mod_Gerais.MethodName());
                        objLog.GravaLog();
                        objLog = null;

                        return false;
                    }

                    // Serializa o objeto para JSON
                    string jsonRequestBody = JsonConvert.SerializeObject(pedidoCompra);

                    // Atualiza o Identificador
                    strIdentificador = (pedidoCompra.C7_NUM != String.Empty ? "Alt" : "Cad") + strIdentificador;

                    // Gera Log
                    objLog = new Class_Log_Hhemo(strIdentificador, intNumTransacao, _intNumServico,
                                     0, 0, jsonRequestBody, null, "Chamada a API Rest - Método " + Mod_Gerais.MethodName(),
                                     "L", intCodPedComWebsupply.ToString(), "", Mod_Gerais.MethodName());
                    objLog.GravaLog();
                    objLog = null;

                    // Adiciona o JSON como conteúdo da requisição
                    var content = new StringContent(jsonRequestBody, Encoding.UTF8, "application/json");

                    // Define os parâmetros e cria a chamada
                    var request = new HttpRequestMessage
                    {
                        Method = pedidoCompra.C7_NUM != String.Empty ? HttpMethod.Put : HttpMethod.Post,
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
                                     "L", intCodPedComWebsupply.ToString(), "", Mod_Gerais.MethodName());
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

                                // Instância a model de controle do retorno da API
                                RetornoAPIModel retornoAPIModel = new RetornoAPIModel
                                {
                                    C_STATUS = linhaRetorno["C_STATUS"].ToString().Trim(),
                                    N_STATUS = (int)linhaRetorno["N_STATUS"]
                                };

                                // Verifica se retornou erro do protheus
                                // N_STATUS": 1 = Sucesso / 0 = Erro
                                if (retornoAPIModel.N_STATUS != 1)
                                {
                                    strMensagem = retornoAPIModel.C_STATUS;

                                    // Gera Log com o retorno da API
                                    objLog = new Class_Log_Hhemo(strIdentificador, intNumTransacao, _intNumServico,
                                                     0, (int)response.StatusCode, retornoAPIModel, null, "Erro no Retorno da Chamada a API Rest - Método " + Mod_Gerais.MethodName(),
                                                     "L", intCodPedComWebsupply.ToString(), "", Mod_Gerais.MethodName());
                                    objLog.GravaLog();
                                    objLog = null;

                                    return false;
                                }

                                // Sincroniza o Retorno da API com os Parametros
                                strCodPedComProtheus = linhaRetorno["C7_COD"].ToString().Trim();
                                strCodLojaProtheus = linhaRetorno["C7_LOJA"].ToString().Trim();

                                // Valida se algum dos códigos retornou vázio
                                // caso sim, devolve erro
                                if (strCodPedComProtheus == String.Empty || strCodLojaProtheus == String.Empty)
                                {
                                    strMensagem = $"Ocorreu um erro na chamada da aplicação - [{linhaRetorno["C_STATUS"].ToString().Trim()}] - A2_LOJA [{strCodLojaProtheus}] - A2_COD [{strCodLojaProtheus}]";

                                    return false;
                                }

                                // Caso o Fornecedor não tenha código do protheus e loja, armazena essa informações
                                // no banco
                                if (pedidoCompra.C7_NUM == String.Empty)
                                {
                                    // Realiza a Chamada do Banco
                                    conn = new Conexao(Mod_Gerais.ConnectionString());

                                    // Cria o Parametro da query do banco
                                    ArrayList arrParam2 = new ArrayList();

                                    arrParam2.Add(new Parametro("@iID_Cadastro", intCodPedComWebsupply, SqlDbType.Int, 4, ParameterDirection.Input));
                                    arrParam2.Add(new Parametro("@cCod_for", strCodPedComProtheus, SqlDbType.Char, 15, ParameterDirection.Input));
                                    arrParam2.Add(new Parametro("@cLoja", strCodLojaProtheus, SqlDbType.Char, 10, ParameterDirection.Input));

                                    ArrayList arrOut2 = new ArrayList();

                                    conn.ExecuteStoredProcedure(new StoredProcedure("[procedure para gravar o codigo do totvs no pedido]", arrParam2), ref arrOut2);

                                    // Encerra a Conexão com Banco de Dados
                                    conn.Dispose();
                                }
                            }
                        }

                        // Define a mensagem de sucesso
                        strMensagem = $"Pedido Nº {intCodPedComWebsupply} do codigo [{(strCodPedComProtheus != String.Empty ? strCodPedComProtheus : pedidoCompra.C7_NUM)}] da loja [{(strCodLojaProtheus != String.Empty ? strCodLojaProtheus : pedidoCompra.C7_LOJA)}] {(pedidoCompra.C7_NUM != String.Empty ? "atualizado(a)" : "cadastrado(a)")} com sucesso.";

                        // Gera Log
                        objLog = new Class_Log_Hhemo(strIdentificador, intNumTransacao, _intNumServico,
                                         0, 0, "", null, strMensagem,
                                         "L", intCodPedComWebsupply.ToString(), "", Mod_Gerais.MethodName());
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
                                         "L", intCodPedComWebsupply.ToString(), "", Mod_Gerais.MethodName());
                        objLog.GravaLog();
                        objLog = null;

                        return false;
                    }
                }
                else
                {
                    // Define a mensagem de erro
                    strMensagem = $"Não foi possível realizar a operação, pois não foi retornando nenhum dado referente ao Pedido Nº {intCodPedComWebsupply}";

                    // Gera Log
                    objLog = new Class_Log_Hhemo(strIdentificador, intNumTransacao, _intNumServico,
                                     0, 0, "", null, strMensagem,
                                     "L", intCodPedComWebsupply.ToString(), "", Mod_Gerais.MethodName());
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
                                 "L", intCodPedComWebsupply.ToString(), "", Mod_Gerais.MethodName());
                objLog.GravaLog();
                objLog = null;

                // Retorna Falso
                return false;
            }
        }
    }
}
