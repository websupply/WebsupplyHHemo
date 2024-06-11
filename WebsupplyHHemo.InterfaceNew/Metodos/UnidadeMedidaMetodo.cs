using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
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
        static int _intNumServico = 8;
        string strIdentificador = "UndMed" + Mod_Gerais.RetornaIdentificador();

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

        public async Task<object> CadastraAtualiza()
        {
            bool retorno = false;
            Class_Log_Hhemo objLog;

            try
            {
                // Cria o Cliente Http
                HttpClient cliente = new HttpClient();

                // Gera Log
                objLog = new Class_Log_Hhemo(strIdentificador, intNumTransacao, _intNumServico,
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
                ArrayList arrOut = new ArrayList();
                DataTable DadosUnidade = conn.ExecuteStoredProcedure(new StoredProcedure("SP_HHEMO_CONSULTA_EMPRESAS_INTERFACE_SEL", arrParam), ref arrOut).Tables[0];

                // Encerra a Conexão com Banco de Dados
                conn.Dispose();

                if (DadosUnidade.Rows.Count > 0)
                {
                    for (int i = 0; i < DadosUnidade.Rows.Count; i++)
                    {
                        // Parametros para Controle de Paginação
                        int totalRegistros = 0;
                        int linhaInicial = 0;
                        int limiteRegistrosPagina = 100;
                        int totalRegistrosPagina = 0;

                        string strMensagemInterna = String.Empty;

                        // Cria um laço para percorrer todas as linhas
                        do
                        {
                            // Define a Estrutura da Request
                            object requestBody = new
                            {
                                tokenid = "HH@2021!%",
                                M0_CODIGO = "01",
                                M0_CODFIL = DadosUnidade.Rows[i]["codigo"], // Incluir o CodFilial
                                AH_UNIMED = "*",
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
                                // Realiza a Chamada do Banco
                                conn = new Conexao(Mod_Gerais.ConnectionString());

                                // Percorre Todos os Resultados
                                for (int j = 0; j < retornoAPI.Count; j++)
                                {
                                    // Pega a Linha do Retorno
                                    JObject linhaRetorno = JObject.Parse(retornoAPI[j].ToString());

                                    // Sincroniza o Retorno da API com a Classe de Gerenciamento
                                    UnidadeMedidaModel unidadeMedida = new UnidadeMedidaModel
                                    {
                                        CodUnidade = DadosUnidade.Rows[i]["codigo"].ToString(),
                                        Descricao = linhaRetorno["AH_DESCPO"].ToString(),
                                        Sigla = linhaRetorno["AH_UNIMED"].ToString()
                                    };

                                    // Cria o Parametro da query do banco

                                    ArrayList arrParam2 = new ArrayList();

                                    arrParam2.Add(new Parametro("@vCodUnidade", unidadeMedida.CodUnidade.ToString(), SqlDbType.VarChar, 500, ParameterDirection.Input));
                                    arrParam2.Add(new Parametro("@cUnidade", unidadeMedida.Sigla.ToString(), SqlDbType.Char, 4, ParameterDirection.Input));
                                    arrParam2.Add(new Parametro("@vDescrUnidade", unidadeMedida.Descricao == "" ? null : unidadeMedida.Descricao.ToString(), SqlDbType.VarChar, 50, ParameterDirection.Input));

                                    ArrayList arrOut2 = new ArrayList();

                                    //conn.ExecuteStoredProcedure(new StoredProcedure("SP_HHEMO_CADASTRA_UNIDADE_MEDIDA_EMPRESA", arrParam2), ref arrOut2);
                                }

                                // Encerra a Conexão com Banco de Dados
                                conn.Dispose();

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
                            strMensagemInterna = $"{totalRegistros} Unidade(s) de Medida(s) cadastradas/atualizadas com sucesso para a Empresa {DadosUnidade.Rows[i]["descricao"]}";
                            strMensagem += strMensagemInterna + "\n";
                        }
                        else
                        {
                            strMensagemInterna = $"Requisição concluída com sucesso sem dados retornados para a Empresa {DadosUnidade.Rows[i]["descricao"]}";
                            strMensagem += strMensagemInterna + "\n";
                        }

                        // Gera Log
                        objLog = new Class_Log_Hhemo(strIdentificador, intNumTransacao, _intNumServico,
                                         0, 0, "", null, strMensagem,
                                         "L", "", "", Mod_Gerais.MethodName());
                        objLog.GravaLog();
                        objLog = null;
                    }
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
