using Microsoft.AspNetCore.Mvc;
using WebsupplyHHemo.API.Dto;
using WebsupplyHHemo.API.ADO;
using WebsupplyHHemo.API.Models;
using WebsupplyHHemo.API.Helpers;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

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

            // Instancia o ADO do Log
            LogDeOperacaoModel objLog = new LogDeOperacaoModel();

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
    }
}
