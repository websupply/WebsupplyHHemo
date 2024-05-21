using Newtonsoft.Json;
using SgiConnection;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using WebsupplyHHemo.Interface.Model;

namespace WebsupplyHHemo.Interface.Metodos
{
    public class UnidadesFiliaisMetodo
    {
        static int _intNumTransacao = 0;
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

        public async Task<bool> CadastraAtualiza()
        {
            string strMensagem = string.Empty;
            bool retorno = false;
            Class_Log_Hhemo objLog;

            try
            {
                // Cria a Model para receber os dados da API
                UnidadeModel unidade = new UnidadeModel();

                // Cria o Cliente Http
                HttpClient cliente = new HttpClient();

                // Gera Log
                objLog = new Class_Log_Hhemo("For" + Mod_Gerais.RetornaIdentificador(), intNumTransacao, 6,
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
                unidade = JsonConvert.DeserializeObject<UnidadeModel>(responseBody);

                // Realiza a Chamada do Banco
                Conexao conn = new Conexao(Mod_Gerais.ConnectionString());

                ArrayList arrParam = new ArrayList();

                arrParam.Add(new Parametro("@cCGC", unidade.CGC == "" ? null : unidade.CGC.ToString(), SqlDbType.VarChar, 15, ParameterDirection.Input));
                arrParam.Add(new Parametro("@vCodEmpresa", unidade.CodUnidade== "" ? null : unidade.CodUnidade.ToString(), SqlDbType.VarChar, 500, ParameterDirection.Input));
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

                conn.ExecuteStoredProcedure(new StoredProcedure("SP_HHEMO_EMPRESAS_INSUPD", arrParam), ref arrOut);

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
