using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SgiConnection;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Mime;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using WebsupplyHHemo.Interface.Model;

namespace WebsupplyHHemo.Interface.Metodos
{
    public class UnidadesFiliaisMetodo
    {
        static int _intNumTransacao = 0;
        static int _intNumServico = 1;
        public string strMensagem = string.Empty;

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
            bool retorno = false;
            Class_Log_Hhemo objLog;

            try
            {
                // Cria o Cliente Http
                HttpClient cliente = new HttpClient();

                // Gera Log
                //objLog = new Class_Log_Hhemo("For" + Mod_Gerais.RetornaIdentificador(), intNumTransacao, _intNumServico,
                //                 0, 0, "", null, "Chamada a API Rest - Método " + Mod_Gerais.MethodName(),
                //                 "L", "", "", Mod_Gerais.MethodName());
                //objLog.GravaLog();
                //objLog = null;

                // Parametros para Controle de Paginação
                int totalRegistros = 0;
                int linhaInicial = 0;
                int limiteRegistrosPagina = 100;
                int totalRegistrosPagina = 0;

                // Cria um laço para percorrer todas as linhas
                do
                {
                    // Define a Estrutura da Request
                    object requestBody = new
                    {
                        tokenid = "HH@2021!%",
                        M0_CODIGO = "01",
                        ROWINI = linhaInicial,
                        ROWLINES = limiteRegistrosPagina
                    };

                    // Serializa o objeto para JSON
                    string jsonRequestBody = JsonConvert.SerializeObject(requestBody);

                    // Adiciona o JSON como conteúdo da requisição
                    var content = new StringContent(jsonRequestBody, Encoding.UTF8, "application/json");

                    // Define os parâmetros e cria a chamada
                    var request = new HttpRequestMessage
                    {
                        Method = HttpMethod.Get,
                        RequestUri = new Uri("http://h119347.protheus.cloudtotvs.com.br:4050/rest/HWEBM004"),
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
                        // Realiza a Chamada do Banco
                        //Conexao conn = new Conexao(Mod_Gerais.ConnectionString());

                        // Percorre Todos os Resultados
                        for (int i = 0; i < retornoAPI.Count; i++)
                        {
                            // Pega a Linha do Retorno
                            JObject linhaRetorno = JObject.Parse(retornoAPI[i].ToString());

                            // Sincroniza o Retorno da API com a Classe de Gerenciamento
                            UnidadeModel unidade = new UnidadeModel
                            {
                                CGC = CGC,
                                CodUnidade = linhaRetorno["M0_CODFIL"].ToString(),
                                NomeAbreviado = linhaRetorno["M0_NOMECOM"].ToString(),
                                Cidade = linhaRetorno["M0_CIDENT"].ToString(),
                                Pais = string.Empty,
                                IE = linhaRetorno["M0_INSC"].ToString(),
                                Bairro = linhaRetorno["M0_BAIRENT"].ToString(),
                                Endereco = linhaRetorno["M0_ENDENT"].ToString(),
                                Numero = string.Empty,
                                Complemento = linhaRetorno["M0_COMPENT"].ToString(),
                                CEP = linhaRetorno["M0_CEPENT"].ToString(),
                                Telefone = string.Empty,
                                Ramal = string.Empty,
                                CaixaPostal = string.Empty,
                                UF = linhaRetorno["M0_ESTENT"].ToString(),
                                CNPJ = linhaRetorno["M0_CGC"].ToString(),
                            };

                            // Cria o Parametro da query do banco

                            ArrayList arrParam = new ArrayList();

                            arrParam.Add(new Parametro("@cCGC", unidade.CGC == "" ? null : unidade.CGC.ToString(), SqlDbType.VarChar, 15, ParameterDirection.Input));
                            arrParam.Add(new Parametro("@vCodEmpresa", unidade.CodUnidade == "" ? null : unidade.CodUnidade.ToString(), SqlDbType.VarChar, 500, ParameterDirection.Input));
                            arrParam.Add(new Parametro("@vDescEmpresa", unidade.NomeAbreviado == "" ? null : unidade.NomeAbreviado.ToString(), SqlDbType.VarChar, 2000, ParameterDirection.Input));
                            arrParam.Add(new Parametro("@vCidade", unidade.Cidade == "" ? null : unidade.Cidade.ToString(), SqlDbType.VarChar, 100, ParameterDirection.Input));
                            arrParam.Add(new Parametro("@cPais", unidade.Pais == "" ? null : unidade.Pais.ToString(), SqlDbType.VarChar, 2, ParameterDirection.Input));
                            arrParam.Add(new Parametro("@cIE", unidade.IE == "" ? null : unidade.IE.ToString(), SqlDbType.VarChar, 2, ParameterDirection.Input));
                            arrParam.Add(new Parametro("@vBairro", unidade.Bairro == "" ? null : unidade.Bairro.ToString(), SqlDbType.VarChar, 100, ParameterDirection.Input));
                            arrParam.Add(new Parametro("@vEndereco", unidade.Endereco == "" ? null : unidade.Endereco.ToString(), SqlDbType.VarChar, 100, ParameterDirection.Input));
                            arrParam.Add(new Parametro("@vNumero", unidade.Numero == "" ? null : unidade.Numero.ToString(), SqlDbType.VarChar, 10, ParameterDirection.Input));
                            arrParam.Add(new Parametro("@vComplemento", unidade.Complemento == "" ? null : unidade.Complemento.ToString(), SqlDbType.VarChar, 100, ParameterDirection.Input));
                            arrParam.Add(new Parametro("@vCEP", unidade.CEP == "" ? null : unidade.CEP.ToString(), SqlDbType.VarChar, 10, ParameterDirection.Input));
                            arrParam.Add(new Parametro("@vTelefone", unidade.Telefone == "" ? null : unidade.Telefone.ToString(), SqlDbType.VarChar, 15, ParameterDirection.Input));
                            arrParam.Add(new Parametro("@vRamal", unidade.Ramal == "" ? null : unidade.Ramal.ToString(), SqlDbType.VarChar, 10, ParameterDirection.Input));
                            arrParam.Add(new Parametro("@vCaixaPostal", unidade.CaixaPostal == "" ? null : unidade.CaixaPostal.ToString(), SqlDbType.VarChar, 20, ParameterDirection.Input));
                            arrParam.Add(new Parametro("@cUF", unidade.UF == "" ? null : unidade.UF.ToString(), SqlDbType.VarChar, 3, ParameterDirection.Input));
                            arrParam.Add(new Parametro("@CNPJ", unidade.CNPJ == "" ? null : unidade.CNPJ.ToString(), SqlDbType.VarChar, 15, ParameterDirection.Input));

                            ArrayList arrOut = new ArrayList();

                            //conn.ExecuteStoredProcedure(new StoredProcedure("SP_HHEMO_EMPRESAS_INSUPD", arrParam), ref arrOut);
                        }

                        // Encerra a Conexão com Banco de Dados
                        //conn.Dispose();

                        // Registra o Total de Registros da Pagina
                        totalRegistrosPagina = retornoAPI.Count;

                        // Seta o Total de Registros
                        totalRegistros += totalRegistrosPagina;

                        // Atualiza a Paginação
                        linhaInicial = totalRegistros + 1;
                    }
                } while (totalRegistrosPagina == limiteRegistrosPagina);

                // Retorna a Mensagem de Sucesso
                if (totalRegistros > 0)
                {
                    strMensagem = $"{totalRegistros} Plano(s) de Conta(s) cadastrados/atualizados com sucesso";
                }
                else
                {
                    strMensagem = "Requisição concluída com sucesso sem dados retornados";
                }

                return true;
            }
            catch (Exception ex)
            {
                // Estrutura o Erro
                strMensagem = ex.Message;

                // Gera Log
                //objLog = new Class_Log_Hhemo("For" + Mod_Gerais.RetornaIdentificador(), intNumTransacao, 6,
                //                 1, -1, "", null, "Erro em " + Mod_Gerais.MethodName() + " :" + strMensagem,
                //                 "", "", "", Mod_Gerais.MethodName());
                //objLog.GravaLog();
                //objLog = null;

                // Retorna Falso
                return false;
            }
        }
    }
}
