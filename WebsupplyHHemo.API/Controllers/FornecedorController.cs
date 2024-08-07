using Microsoft.AspNetCore.Mvc;
using WebsupplyHHemo.API.Dto;
using WebsupplyHHemo.API.ADO;
using WebsupplyHHemo.API.Models;
using WebsupplyHHemo.API.Helpers;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using WebsupplyHHemo.Interface.Model;
using WebsupplyHHemo.Interface.Funcoes;
using WebsupplyHHemo.API.Attributes;
using Newtonsoft.Json;

namespace WebsupplyHHemo.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class FornecedorController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly string _ambiente;
        private static int _transacao;
        private static int _servico;
        private static string _identificador;

        public FornecedorController(IConfiguration configuration)
        {
            _configuration = configuration;
            _ambiente = _configuration.GetValue<string>("Parametros:Ambiente");
        }

        [HttpPost]
        [Route("atualiza-status")]
        [Servico(20)]
        public ObjectResult AtualizaStatus(FornecedorRequestDto objRequest)
        {
            // Instancia o obj do Log
            Class_Log_Hhemo objLog;

            // Pega a Claims
            ClaimsIdentity identity = (ClaimsIdentity)User.Identity;
            UserModel objUser = HelperClaims.CarregarUsuario(identity);

            // Limpa os espaços em branco
            objRequest.A2_COD = objRequest.A2_COD.Trim();
            objRequest.A2_LOJA = objRequest.A2_LOJA.Trim();
            objRequest.A2_CGC = objRequest.A2_CGC.Trim();
            objRequest.A2_Justificativa = objRequest.A2_Justificativa.Trim();
            objRequest.A2_MSBLQL = objRequest.A2_MSBLQL.Trim();

            try
            {
                // Pega o Atributo de Serviço
                var servicoAttribute = (ServicoAttribute)Attribute.GetCustomAttribute(
                    typeof(AutenticacaoController).GetMethod(nameof(AtualizaStatus)),
                    typeof(ServicoAttribute));

                // Seta os parametros inicias do Log
                _transacao = 0;
                _servico = servicoAttribute.IDServico;
                _identificador = "AtualizaStatus" + Mod_Gerais.RetornaIdentificador();

                // Gera Log
                objLog = new Class_Log_Hhemo(_identificador, _transacao, _servico,
                                 0, 0, JsonConvert.SerializeObject(objRequest), null, "Chamada a API Rest - Método " + Mod_Gerais.MethodName(),
                                 "L", "", "", Mod_Gerais.MethodName());
                objLog.GravaLog();
                objLog = null;

                // Instancia o ADO da Atualização de Status do Fornecedor
                FornecedorADO objADO = new FornecedorADO();

                if (!objADO.ATUALIZA_STATUS_FORNECEDOR(_configuration.GetValue<string>("ConnectionStrings:DefaultConnection"), objRequest))
                {
                    // Gera Log
                    objLog = new Class_Log_Hhemo(_identificador, _transacao, _servico,
                                     0, 0, JsonConvert.SerializeObject(objADO), null, "Erro na Chamada da API Rest - Método " + Mod_Gerais.MethodName(),
                                     "L", "", "", Mod_Gerais.MethodName());
                    objLog.GravaLog();
                    objLog = null;

                    return new ObjectResult(new
                    {
                        Mensagem = objADO.strMensagem
                    })
                    { StatusCode = 500 };
                }

                // Gera Log
                objLog = new Class_Log_Hhemo(_identificador, _transacao, _servico,
                                 0, 0, JsonConvert.SerializeObject(objADO), null, "Retorno da Chamada a API Rest - Método " + Mod_Gerais.MethodName(),
                                 "L", "", "", Mod_Gerais.MethodName());
                objLog.GravaLog();
                objLog = null;

                return new ObjectResult(new
                {
                    Mensagem = objADO.strMensagem
                })
                { StatusCode = 200 };
            }
            catch (Exception ex)
            {
                // Inicializa a Model de Excepetion
                ExcepetionModel excepetionEstruturada = new ExcepetionModel(ex, true);

                // Gera Log
                objLog = new Class_Log_Hhemo(_identificador, _transacao, _servico,
                                 1, -1, JsonConvert.SerializeObject(excepetionEstruturada), null, excepetionEstruturada.Mensagem,
                                 "L", "", "", Mod_Gerais.MethodName());
                objLog.GravaLog();
                objLog = null;

                // Devolve o Erro
                return new ObjectResult(new
                {
                    Mensagem = excepetionEstruturada.Mensagem
                })
                { StatusCode = 500 };
            }
        }
    }
}
