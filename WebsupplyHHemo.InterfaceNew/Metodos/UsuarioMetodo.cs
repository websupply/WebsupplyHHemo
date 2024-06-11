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
using WebsupplyHHemo.InterfaceNew.Funcoes;
using WebsupplyHHemo.InterfaceNew.Model;

namespace WebsupplyHHemo.InterfaceNew.Metodos
{
    public class UsuarioMetodo
    {

        static int _intNumTransacao = 0;
        static int _intNumServico = 9;
        string strIdentificador = "Usr" + Mod_Gerais.RetornaIdentificador();

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

        public async Task<bool> CadastraAtualiza(string CGC, string CCusto, string Requisit)
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

                // Parametros para Controle de Paginação
                int totalRegistros = 0;
                int linhaInicial = 0;
                int limiteRegistrosPagina = 100;
                int totalRegistrosPagina = 0;

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
                        //Conexao conn = new Conexao(Mod_Gerais.ConnectionString());

                        // Percorre Todos os Resultados
                        for (int i = 0; i < retornoAPI.Count; i++)
                        {
                            // Pega a Linha do Retorno
                            JObject linhaRetorno = JObject.Parse(retornoAPI[i].ToString());

                            // Sincroniza o Retorno da API com a Classe de Gerenciamento
                            UsuarioModel usuario = new UsuarioModel
                            {
                                CGC = CGC,
                                CCUSTO = CCusto,
                                REQUISIT = Requisit,
                                CodUsuario = linhaRetorno["USR_ID"].ToString().Trim(),
                                Nome = linhaRetorno["USR_NOME"].ToString().Trim(),
                                Usuario = linhaRetorno["USR_CODIGO"].ToString().Trim(),
                                Email = linhaRetorno["USR_EMAIL"].ToString().Trim()
                            };

                            // Cria o Parametro da query do banco
                            //Conexao conn = new Conexao(Mod_Gerais.ConnectionString());

                            //ArrayList arrParam = new ArrayList();

                            //arrParam.Add(new Parametro("@cCodigo", usuario.CodUsuario.ToString(), SqlDbType.Int, 4, ParameterDirection.Input));
                            //arrParam.Add(new Parametro("@cNOMEREQUISIT", usuario.Nome == "" ? null : usuario.Nome.ToString(), SqlDbType.Char, 30, ParameterDirection.Input));
                            //arrParam.Add(new Parametro("@cEMAIL", usuario.Email == "" ? null : usuario.Email.ToString(), SqlDbType.Char, 60, ParameterDirection.Input));
                            //arrParam.Add(new Parametro("@cCompraContrato", usuario.Comprador == "" ? "N" : usuario.Comprador.ToString(), SqlDbType.Char, 1, ParameterDirection.Input));
                            //arrParam.Add(new Parametro("@cCotacao", usuario.Solicitante == "" ? "N" : usuario.Solicitante.ToString(), SqlDbType.Char, 1, ParameterDirection.Input));
                            //arrParam.Add(new Parametro("@cINDEXCLUI", usuario.Status == "" ? "N" : usuario.Status.ToString(), SqlDbType.Char, 1, ParameterDirection.Input));
                            //arrParam.Add(new Parametro("@cCGC", CGC, SqlDbType.Char, 15, ParameterDirection.Input));
                            //arrParam.Add(new Parametro("@cCCUSTO", CCusto, SqlDbType.Char, 15, ParameterDirection.Input));
                            //arrParam.Add(new Parametro("@cREQUISIT", Requisit, SqlDbType.Char, 10, ParameterDirection.Input));

                            //ArrayList arrOut = new ArrayList();

                            //conn.ExecuteStoredProcedure(new StoredProcedure("SP_CENTROSCUSTO_INSUPD", arrParam), ref arrOut);
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
                    strMensagem = $"{totalRegistros} Usuario(s) cadastrado(s)/atualizado(s) com sucesso";
                }
                else
                {
                    strMensagem = "Requisição concluída com sucesso sem dados retornados";
                }

                // Gera Log
                objLog = new Class_Log_Hhemo(strIdentificador, intNumTransacao, _intNumServico,
                                 0, 0, "", null, strMensagem,
                                 "L", "", "", Mod_Gerais.MethodName());
                objLog.GravaLog();
                objLog = null;

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
