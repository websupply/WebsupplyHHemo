using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
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
    public class NaturezaMetodo
    {
        static int _intNumTransacao = 0;
        static int _intNumServico = 2;
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

        public async Task<bool> CadastraAtualizaExclui()
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
                        ED_CODIGO = "*",
                        M0_CODFIL = "01010001",
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
                        RequestUri = new Uri("http://h119347.protheus.cloudtotvs.com.br:4050/rest/HWEBM014"),
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
                        Conexao conn = new Conexao(Mod_Gerais.ConnectionString());

                        // Percorre Todos os Resultados
                        for (int i = 0; i < retornoAPI.Count; i++)
                        {
                            // Pega a Linha do Retorno
                            JObject linhaRetorno = JObject.Parse(retornoAPI[i].ToString());

                            // Sincroniza o Retorno da API com a Classe de Gerenciamento
                            NaturezaModel natureza = new NaturezaModel
                            {
                                CodNatureza = linhaRetorno["ED_CODIGO"].ToString(),
                                Descricao = linhaRetorno["ED_DESCRIC"].ToString(),
                                Status = linhaRetorno["N_STATUS"].ToString() == "1" ? "N" : "S",
                            };

                            // Cria o Parametro da query do banco

                            ArrayList arrParam = new ArrayList();

                            arrParam.Add(new Parametro("@vCodigo", natureza.CodNatureza.ToString(), SqlDbType.Int, 4, ParameterDirection.Input));
                            arrParam.Add(new Parametro("@vDescricao", natureza.Descricao == "" ? null : natureza.Descricao.ToString(), SqlDbType.VarChar, 50, ParameterDirection.Input));
                            arrParam.Add(new Parametro("@cStatus", natureza.Status == "" ? "N" : natureza.Status.ToString(), SqlDbType.Char, 1, ParameterDirection.Input));

                            ArrayList arrOut = new ArrayList();

                            conn.ExecuteStoredProcedure(new StoredProcedure("SP_HHEMO_Natureza_INSUPDEXC", arrParam), ref arrOut);
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
                    strMensagem = $"{totalRegistros} Natureza(s) cadastradas/atualizadas com sucesso";
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
