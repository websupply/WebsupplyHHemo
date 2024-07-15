using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using WebsupplyHHemo.API.ADO;
using WebsupplyHHemo.API.Dto;
using WebsupplyHHemo.API.Helpers;
using WebsupplyHHemo.API.Models;

namespace WebsupplyHHemo.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RecebimentoController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public RecebimentoController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpPost]
        [Route("envia-recebimento")]
        public ObjectResult ComplementoItem(RecebimentoRequestDto objRequest)
        {
            // Pega a Claims
            ClaimsIdentity identity = (ClaimsIdentity)User.Identity;
            UserModel objUser = HelperClaims.CarregarUsuario(identity);

            // Instancia o ADO do Log
            LogDeOperacaoModel objLog = new LogDeOperacaoModel();

            // Gera o Log de Operação
            objLog.nCod_Operacao = _configuration.GetValue<int>("Parametros:LogDeOperacao:Operacoes_Tipos-Codigo_Recebimento_Fiscal_Integracao");
            objLog.cDetalhe = $"Recebimento Fiscal do número [{objRequest.NumDoc}] referente ao pedido [{objRequest.NumPedido}] realizado com sucesso. (Apenas mensagem de teste)"; ;
            objLog.cIP = Request.HttpContext.Connection.RemoteIpAddress.ToString();

            return new ObjectResult(new
            {
                Mensagem = "Requisição Realizada com sucesso"
            })
            { StatusCode = 200 };

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
