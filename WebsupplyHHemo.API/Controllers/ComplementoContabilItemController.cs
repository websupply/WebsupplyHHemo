using Microsoft.AspNetCore.Mvc;
using WebsupplyHHemo.API.Dto;
using WebsupplyHHemo.API.ADO;
using WebsupplyHHemo.API.Models;
using WebsupplyHHemo.API.Helpers;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using WebsupplyHHemo.Interface.Model;
using WebsupplyHHemo.Interface.Funcoes;

namespace WebsupplyHHemo.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ComplementoContabilItemController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public ComplementoContabilItemController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpPost]
        [Route("complemento-item")]
        public ObjectResult ComplementoItem(ComplementoContabilItemRequestDto objRequest)
        {
            // Pega a Claims
            ClaimsIdentity identity = (ClaimsIdentity)User.Identity;
            UserModel objUser = HelperClaims.CarregarUsuario(identity);

            // Limpa os espaços em branco
            objRequest.CodWebsupply = objRequest.CodWebsupply.Trim();
            objRequest.CodProtheus = objRequest.CodProtheus.Trim();
            objRequest.ContaContabil = objRequest.ContaContabil.Trim();

            // Instancia o ADO do Log
            LogDeOperacaoModel objLog = new LogDeOperacaoModel();

            try
            {
                // Instancia o ADO do Complemento Contabil do Item
                ComplementoContabilItemADO objADO = new ComplementoContabilItemADO();

                if (!objADO.ATUALIZA_DADOS_CONTABEIS_ITEM(_configuration.GetValue<string>("ConnectionStrings:DefaultConnection"), objRequest))
                {
                    // Gera o Log de Operação
                    objLog.nCod_Operacao = _configuration.GetValue<int>("Parametros:LogDeOperacao:Operacoes_Tipos-Codigo_Conta_Contabil_Integracao");
                    objLog.cDetalhe = objADO.strMensagem;
                    objLog.cIP = Request.HttpContext.Connection.RemoteIpAddress.ToString();

                    LogsADO.GERA_LOGDEOPERACAO(
                        _configuration.GetValue<string>("ConnectionStrings:DefaultConnection"),
                        objUser,
                        objLog
                    );

                    return new ObjectResult(new
                    {
                        Mensagem = objADO.strMensagem
                    })
                    { StatusCode = 500 };
                }

                // Gera o Log de Operação
                objLog.nCod_Operacao = _configuration.GetValue<int>("Parametros:LogDeOperacao:Operacoes_Tipos-Codigo_Conta_Contabil_Integracao");
                objLog.cDetalhe = objADO.strMensagem;
                objLog.cIP = Request.HttpContext.Connection.RemoteIpAddress.ToString();

                LogsADO.GERA_LOGDEOPERACAO(
                    _configuration.GetValue<string>("ConnectionStrings:DefaultConnection"),
                    objUser,
                    objLog
                );

                return new ObjectResult(new
                {
                    Mensagem = objADO.strMensagem
                })
                { StatusCode = 200 };
            }
            catch(Exception ex)
            {
                // Inicializa a Model de Excepetion
                ExcepetionModel excepetionEstruturada = new ExcepetionModel(ex, true);

                // Gera o Log de Operação
                objLog.nCod_Operacao = _configuration.GetValue<int>("Parametros:LogDeOperacao:Operacoes_Tipos-Codigo_Recebimento_Fiscal_Integracao");
                objLog.cDetalhe = excepetionEstruturada.Mensagem;
                objLog.cIP = Request.HttpContext.Connection.RemoteIpAddress.ToString();

                LogsADO.GERA_LOGDEOPERACAO(
                        _configuration.GetValue<string>("ConnectionStrings:DefaultConnection"),
                        objUser,
                        objLog
                    );

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
