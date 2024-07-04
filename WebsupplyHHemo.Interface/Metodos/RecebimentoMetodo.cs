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
using System.Net.Http.Headers;

namespace WebsupplyHHemo.Interface.Metodos
{
    public class RecebimentoMetodo
    {
        static int _intNumTransacao = 0;
        static int _intNumServico = 13;
        string strIdentificador = "Rec" + Mod_Gerais.RetornaIdentificador();

        public string strMensagem = string.Empty;

        // Paramêtros de Controle da Classe
        public int intCodRecComWebsupply = 0;
        public string strCodLojaProtheus = string.Empty;
        public string strFuncao = string.Empty; // Enviar - Cancelar

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
                // Verifica se a Funcao enviada está dentro do esperado
                if(strFuncao.ToString().Trim() != "Enviar" && strFuncao.ToString().Trim() != "Cancelar")
                {

                    // Define a mensagem de erro
                    strMensagem = $"Função não permitida ou não parametrizada, verifique e tente novamente. Função Solicitada [{strFuncao}]";

                    // Gera Log
                    objLog = new Class_Log_Hhemo(strIdentificador, intNumTransacao, _intNumServico,
                                     0, 0, "", null, strMensagem,
                                     "L", intCodRecComWebsupply.ToString(), "", Mod_Gerais.MethodName());
                    objLog.GravaLog();
                    objLog = null;

                    return false;
                }

                // Cria o Cliente Http
                HttpClient cliente = new HttpClient();

                // Gera Log
                objLog = new Class_Log_Hhemo(strIdentificador, intNumTransacao, _intNumServico,
                                 0, 0, "", null, "Inicio do Método " + Mod_Gerais.MethodName(),
                                 "L", intCodRecComWebsupply.ToString(), "", Mod_Gerais.MethodName());
                objLog.GravaLog();
                objLog = null;

                // Pega a URL do Serviço
                Class_Servico objServico = new Class_Servico();
                if (objServico.CarregaDados(_intNumServico, "", strIdentificador, intNumTransacao) == false)
                {
                    objLog = new Class_Log_Hhemo(strIdentificador, intNumTransacao, _intNumServico,
                                                       1, -1, "", null, "Erro ao recuperar dados do serviço",
                                                       "", intCodRecComWebsupply.ToString(), "", Mod_Gerais.MethodName());
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

                arrParam.Add(new Parametro("@iCL_CDG", intCodRecComWebsupply, SqlDbType.Int, 4, ParameterDirection.Input));

                ArrayList arrOut = new ArrayList();
                DataTable DadosRecebimento = conn.ExecuteStoredProcedure(new StoredProcedure("[procedure para consultar recebimento]", arrParam), ref arrOut).Tables[0];

                // Encerra a Conexão com Banco de Dados
                conn.Dispose();

                if (DadosRecebimento.Rows.Count > 0)
                {
                    // Estrutura a Model
                    RecebimentoModel recebimento = new RecebimentoModel
                    {
                        tokenid = DadosRecebimento.Rows[0]["tokenid"].ToString().Trim(),
                        M0_CODFIL = DadosRecebimento.Rows[0]["M0_CODFIL"].ToString().Trim(),
                        M0_CODIGO = DadosRecebimento.Rows[0]["M0_CODIGO"].ToString().Trim(),
                        F1_CHVNFE = DadosRecebimento.Rows[0]["F1_CHVNFE"].ToString().Trim(),
                        F1_COND = DadosRecebimento.Rows[0]["F1_COND"].ToString().Trim(),
                        F1_DOC = DadosRecebimento.Rows[0]["F1_DOC"].ToString().Trim(),
                        F1_DTDIGIT = DadosRecebimento.Rows[0]["F1_DTDIGIT"].ToString().Trim(),
                        F1_EMISSAO = DadosRecebimento.Rows[0]["F1_EMISSAO"].ToString().Trim(),
                        F1_ESPECIE = DadosRecebimento.Rows[0]["F1_ESPECIE"].ToString().Trim(),
                        F1_FORMUL = DadosRecebimento.Rows[0]["F1_FORMUL"].ToString().Trim(),
                        F1_FORNECE = DadosRecebimento.Rows[0]["F1_FORNECE"].ToString().Trim(),
                        F1_LOJA = DadosRecebimento.Rows[0]["F1_LOJA"].ToString().Trim(),
                        F1_SERIE = DadosRecebimento.Rows[0]["F1_SERIE"].ToString().Trim(),
                        F1_TIPO = DadosRecebimento.Rows[0]["F1_TIPO"].ToString().Trim(),
                        F1_XML = DadosRecebimento.Rows[0]["F1_XML"].ToString().Trim(),
                        UUID_WEBSUPPLY = DadosRecebimento.Rows[0]["UUID_WEBSUPPLY"].ToString().Trim(),
                    };

                    // Realiza a Chamada do Banco
                    conn = new Conexao(Mod_Gerais.ConnectionString());

                    // Cria o Parametro da query do banco
                    arrParam = new ArrayList();

                    arrParam.Add(new Parametro("@iCL_CDG", intCodRecComWebsupply, SqlDbType.Int, 4, ParameterDirection.Input));

                    arrOut = new ArrayList();
                    DataTable DadosItens = conn.ExecuteStoredProcedure(new StoredProcedure("[procedure para consultar itens do pedido]", arrParam), ref arrOut).Tables[0];

                    // Encerra a Conexão com Banco de Dados
                    conn.Dispose();

                    // Verifica se Existe itens para o pedido e caso sim, traz
                    // os itens e caso não, retorna erro
                    if (DadosItens.Rows.Count > 0)
                    {
                        for (int i = 0; i < DadosItens.Rows.Count; i++)
                        {
                            // Pega a Linha do Registro
                            var registro = DadosItens.Rows[i];

                            // Carrega os Dados do item
                            RecebimentoModel.Item item = new RecebimentoModel.Item
                            {
                                D1_CC = registro["D1_CC"].ToString().Trim(),
                                D1_CONTA = registro["D1_CONTA"].ToString().Trim(),
                                D1_DATPRF = registro["D1_DATPRF"].ToString().Trim(),
                                D1_DESPESA = (decimal)registro["D1_DESPESA"],
                                D1_ITEM = registro["D1_ITEM"].ToString().Trim(),
                                D1_ITEMPC = registro["D1_ITEMPC"].ToString().Trim(),
                                D1_OBS = registro["D1_OBS"].ToString().Trim(),
                                D1_PRECO = (decimal)registro["D1_PRECO"],
                                D1_COD = registro["D1_COD"].ToString().Trim(),
                                D1_PEDIDO = registro["D1_PEDIDO"].ToString().Trim(),
                                D1_OPER = registro["D1_OPER"].ToString().Trim(),
                                D1_VALDESC = (decimal)registro["D1_VALDESC"],
                                D1_VUNIT = (decimal)registro["D1_VUNIT"],
                                D1_QUANT = (int)registro["D1_QUANT"],
                                D1_SEGURO = (decimal)registro["D1_SEGURO"],
                                D1_TOTAL = (decimal)registro["D1_TOTAL"],
                                D1_VALFRE = (decimal)registro["D1_VALFRE"],
                                D1_VALICM = (decimal)registro["D1_VALICM"],
                                D1_VALIPI = (decimal)registro["D1_VALIPI"],
                                D1_VLDESC = (decimal)registro["D1_VLDESC"],
                                D1_MSBLQL = registro["D1_MSBLQL"].ToString().Trim(),
                            };

                            // Adiciona a Array de Itens
                            recebimento.PRENOTA_ITENS.Add(item);
                        }
                    }
                    else
                    {
                        // Define a mensagem de erro
                        strMensagem = $"Não foi possível realizar a operação, pois não foi retornando nenhum item associado ao Pedido Nº {intCodRecComWebsupply}";

                        // Gera Log
                        objLog = new Class_Log_Hhemo(strIdentificador, intNumTransacao, _intNumServico,
                                         0, 0, "", null, strMensagem,
                                         "L", intCodRecComWebsupply.ToString(), "", Mod_Gerais.MethodName());
                        objLog.GravaLog();
                        objLog = null;

                        return false;
                    }

                    // Realiza a Chamada do Banco
                    conn = new Conexao(Mod_Gerais.ConnectionString());

                    // Cria o Parametro da query do banco
                    arrParam = new ArrayList();

                    arrParam.Add(new Parametro("@iCL_CDG", intCodRecComWebsupply, SqlDbType.Int, 4, ParameterDirection.Input));

                    arrOut = new ArrayList();
                    DataTable DadosAnexos = conn.ExecuteStoredProcedure(new StoredProcedure("[procedure para consultar anexos do pedido]", arrParam), ref arrOut).Tables[0];

                    // Encerra a Conexão com Banco de Dados
                    conn.Dispose();

                    // Verifica se Existe itens para o pedido e caso sim, traz
                    // os itens e caso não, retorna erro
                    if (DadosAnexos.Rows.Count > 0)
                    {
                        for (int i = 0; i < DadosAnexos.Rows.Count; i++)
                        {
                            // Pega a Linha do Registro
                            var registro = DadosAnexos.Rows[i];

                            // Carrega os Dados do Anexo
                            RecebimentoModel.Anexo anexo = new RecebimentoModel.Anexo
                            {
                                ID_DOC = registro["ID_DOC"].ToString().Trim(),
                                DOC = registro["DOC"].ToString().Trim(),
                                DOCX64 = registro["DOCX64"].ToString().Trim(),
                            };

                            // Adiciona a Array de Itens
                            recebimento.ANEXOS.Add(anexo);
                        }
                    }

                    // Serializa o objeto para JSON
                    string jsonRequestBody = JsonConvert.SerializeObject(recebimento);

                    // Atualiza o Identificador
                    strIdentificador = (strFuncao.ToString().Trim() == "Enviar" ? "Cad" : "Exc") + strIdentificador;

                    // Gera Log
                    objLog = new Class_Log_Hhemo(strIdentificador, intNumTransacao, _intNumServico,
                                     0, 0, jsonRequestBody, null, "Chamada a API Rest - Método " + Mod_Gerais.MethodName(),
                                     "L", intCodRecComWebsupply.ToString(), "", Mod_Gerais.MethodName());
                    objLog.GravaLog();
                    objLog = null;

                    // Adiciona o JSON como conteúdo da requisição
                    var content = new StringContent(jsonRequestBody, Encoding.UTF8, "application/json");

                    // Carrega os Dados de Autenticação
                    var base64EncodedAuth = Convert.ToBase64String(Encoding.ASCII.GetBytes($"{objServico.strUsuario}:{objServico.strSenha}"));

                    // Define os parâmetros e cria a chamada
                    var request = new HttpRequestMessage
                    {
                        Method = strFuncao.ToString().Trim() == "Enviar" ? HttpMethod.Post : HttpMethod.Put,
                        RequestUri = new Uri(objServico.strURL),
                        Content = content,
                        Headers =
                                {
                                    Authorization = new AuthenticationHeaderValue($"{objServico.strTipoAutenticao}", base64EncodedAuth)
                                }
                    };

                    // Envia a requisição
                    var response = cliente.SendAsync(request).ConfigureAwait(false).GetAwaiter().GetResult();

                    // Trata o Retorno da API
                    var responseBody = response.Content.ReadAsStringAsync().ConfigureAwait(false).GetAwaiter().GetResult();

                    // Gera Log com o retorno da API
                    objLog = new Class_Log_Hhemo(strIdentificador, intNumTransacao, _intNumServico,
                                     0, (int)response.StatusCode, responseBody, null, "Retorno da Chamada a API Rest - Método " + Mod_Gerais.MethodName(),
                                     "L", intCodRecComWebsupply.ToString(), "", Mod_Gerais.MethodName());
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
                                                     "L", intCodRecComWebsupply.ToString(), "", Mod_Gerais.MethodName());
                                    objLog.GravaLog();
                                    objLog = null;

                                    return false;
                                }
                            }
                        }

                        // Define a mensagem de sucesso
                        strMensagem = $"Recebimento Nº {intCodRecComWebsupply} da loja [{(strCodLojaProtheus != String.Empty ? strCodLojaProtheus : recebimento.F1_LOJA)}] {(strFuncao.ToString().Trim() == "Enviar" ? "cadastrado(a)" : "atualizado(a)")} com sucesso.";

                        // Gera Log
                        objLog = new Class_Log_Hhemo(strIdentificador, intNumTransacao, _intNumServico,
                                         0, 0, "", null, strMensagem,
                                         "L", intCodRecComWebsupply.ToString(), "", Mod_Gerais.MethodName());
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
                                         "L", intCodRecComWebsupply.ToString(), "", Mod_Gerais.MethodName());
                        objLog.GravaLog();
                        objLog = null;

                        return false;
                    }
                }
                else
                {
                    // Define a mensagem de erro
                    strMensagem = $"Não foi possível realizar a operação, pois não foi retornando nenhum dado referente ao Pedido Nº {intCodRecComWebsupply}";

                    // Gera Log
                    objLog = new Class_Log_Hhemo(strIdentificador, intNumTransacao, _intNumServico,
                                     0, 0, "", null, strMensagem,
                                     "L", intCodRecComWebsupply.ToString(), "", Mod_Gerais.MethodName());
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
                                 "L", intCodRecComWebsupply.ToString(), "", Mod_Gerais.MethodName());
                objLog.GravaLog();
                objLog = null;

                // Retorna Falso
                return false;
            }
        }
    }
}
