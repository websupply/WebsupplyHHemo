using Microsoft.AspNetCore.Mvc;
using WebsupplyHHemo.API.Dto;
using WebsupplyHHemo.API.ADO;
using WebsupplyHHemo.API.Models;
using WebsupplyHHemo.API.Helpers;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using WebsupplyHHemo.Interface.Model;

namespace WebsupplyHHemo.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
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

            // Limpa os espaços em branco
            objRequest.NumPedido = objRequest.NumPedido.Trim();
            objRequest.NumDoc = objRequest.NumDoc.Trim();
            objRequest.Serie = objRequest.Serie.Trim();
            objRequest.CodFornecedor = objRequest.CodFornecedor.Trim();
            objRequest.CnpjFornecedor = objRequest.CnpjFornecedor.Trim();
            objRequest.Status = objRequest.Status.Trim();

            // Instancia o Model do Log
            LogDeOperacaoModel objLog = new LogDeOperacaoModel();

            try
            {
                // Gera o Log de Operação
                objLog.nCod_Operacao = _configuration.GetValue<int>("Parametros:LogDeOperacao:Operacoes_Tipos-Codigo_Recebimento_Fiscal_Integracao");
                objLog.cDetalhe = $"Recebimento Fiscal do número [{objRequest.NumDoc}] referente ao pedido [{objRequest.NumPedido}] realizado com sucesso. (Apenas mensagem de teste)";
                objLog.cIP = Request.HttpContext.Connection.RemoteIpAddress.ToString();

                LogsADO.GERA_LOGDEOPERACAO(
                        _configuration.GetValue<string>("ConnectionStrings:DefaultConnection"),
                        objUser,
                        objLog
                    );

                return new ObjectResult(new
                {
                    Mensagem = "Requisição Realizada com sucesso"
                })
                { StatusCode = 200 };

                // Instancia o ADO do Complemento Contabil do Item
                RecebimentoADO objADO = new RecebimentoADO();

                if (!objADO.ATUALIZA_RECEBIMENTO_FISCAL(_configuration.GetValue<string>("ConnectionStrings:DefaultConnection"), objRequest))
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
            catch (Exception ex)
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
