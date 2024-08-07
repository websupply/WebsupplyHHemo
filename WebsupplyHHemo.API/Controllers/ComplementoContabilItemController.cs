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
    public class ComplementoContabilItemController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly string _ambiente;
        private static int _transacao;
        private static int _servico;
        private static string _identificador;

        public ComplementoContabilItemController(IConfiguration configuration)
        {
            _configuration = configuration;
            _ambiente = _configuration.GetValue<string>("Parametros:Ambiente");
        }

        [HttpPost]
        [Route("complemento-item")]
        [Servico(18)]
        public ObjectResult ComplementoItem(ComplementoContabilItemRequestDto objRequest)
        {
            // Instancia o obj do Log
            Class_Log_Hhemo objLog;

            // Instancia a Model de Log
            LogWebService logWebService;

            // Pega a Claims
            ClaimsIdentity identity = (ClaimsIdentity)User.Identity;
            UserModel objUser = HelperClaims.CarregarUsuario(identity);

            // Limpa os espaços em branco
            objRequest.CodWebsupply = objRequest.CodWebsupply.Trim();
            objRequest.CodProtheus = objRequest.CodProtheus.Trim();
            objRequest.ContaContabil = objRequest.ContaContabil.Trim();

            try
            {
                // Pega o Atributo de Serviço
                var servicoAttribute = (ServicoAttribute)Attribute.GetCustomAttribute(
                    typeof(ComplementoContabilItemController).GetMethod(nameof(ComplementoItem)),
                    typeof(ServicoAttribute));

                // Seta os parametros inicias do Log
                _transacao = 0;
                _servico = servicoAttribute.IDServico;
                _identificador = "ComplementoItem" + Mod_Gerais.RetornaIdentificador();

                // Gera Log
                objLog = new Class_Log_Hhemo(_identificador, _transacao, _servico,
                                 0, 0, JsonConvert.SerializeObject(objRequest), null, "Chamada a API Rest - Método " + Mod_Gerais.MethodName(),
                                 "L", "", "", Mod_Gerais.MethodName());
                objLog.GravaLog();
                objLog = null;

                // Instancia o ADO do Complemento Contabil do Item
                ComplementoContabilItemADO objADO = new ComplementoContabilItemADO();

                if (!objADO.ATUALIZA_DADOS_CONTABEIS_ITEM(_configuration.GetValue<string>("ConnectionStrings:DefaultConnection"), objRequest))
                {
                    // Instancia a model do Log
                    logWebService = new LogWebService()
                    {
                        Mensagem = objADO.strMensagem,
                        Retorno = objRequest
                    };

                    // Gera Log
                    objLog = new Class_Log_Hhemo(_identificador, _transacao, _servico,
                                     0, 0, JsonConvert.SerializeObject(logWebService), null, "Erro na Chamada da API Rest - Método " + Mod_Gerais.MethodName(),
                                     "L", "", "", Mod_Gerais.MethodName());
                    objLog.GravaLog();
                    objLog = null;

                    return new ObjectResult(new
                    {
                        Mensagem = logWebService.Mensagem
                    })
                    { StatusCode = 500 };
                }

                // Instancia a model do Log
                logWebService = new LogWebService()
                {
                    Mensagem = objADO.strMensagem,
                    Retorno = objRequest
                };

                // Gera Log
                objLog = new Class_Log_Hhemo(_identificador, _transacao, _servico,
                                 0, 0, JsonConvert.SerializeObject(logWebService), null, "Retorno da Chamada a API Rest - Método " + Mod_Gerais.MethodName(),
                                 "L", "", "", Mod_Gerais.MethodName());
                objLog.GravaLog();
                objLog = null;

                return new ObjectResult(new
                {
                    Mensagem = logWebService.Mensagem
                })
                { StatusCode = 200 };
            }
            catch(Exception ex)
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
