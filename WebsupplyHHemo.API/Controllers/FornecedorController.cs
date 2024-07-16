﻿using Microsoft.AspNetCore.Mvc;
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
    public class FornecedorController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public FornecedorController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpPost]
        [Route("atualiza-status")]
        public ObjectResult AtualizaStatus(FornecedorRequestDto objRequest)
        {
            // Pega a Claims
            ClaimsIdentity identity = (ClaimsIdentity)User.Identity;
            UserModel objUser = HelperClaims.CarregarUsuario(identity);

            // Instancia o ADO do Log
            LogDeOperacaoModel objLog = new LogDeOperacaoModel();

            // Gera o Log de Operação
            objLog.nCod_Operacao = _configuration.GetValue<int>("Parametros:LogDeOperacao:Operacoes_Tipos-Codigo_Atualizacao_Fornecedor_Integracao");
            objLog.cDetalhe = $"Atualização de Status do Fornecedor com A2_COD [{objRequest.A2_COD}] referente a A2_LOJA [{objRequest.A2_LOJA}] do A2_CGC [{objRequest.A2_CGC}] para o Status - A2_MSBLQL [{objRequest.A2_MSBLQL}] com sucesso. (Apenas mensagem de teste)"; ;
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

            // Instancia o ADO da Atualização de Status do Fornecedor
            FornecedorADO objADO = new FornecedorADO();

            if (!objADO.ATUALIZA_STATUS_FORNECEDOR(_configuration.GetValue<string>("ConnectionStrings:DefaultConnection"), objRequest))
            {
                // Gera o Log de Operação
                objLog.nCod_Operacao = _configuration.GetValue<int>("Parametros:LogDeOperacao:Operacoes_Tipos-Codigo_Atualizacao_Fornecedor_Integracao");
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
            objLog.nCod_Operacao = _configuration.GetValue<int>("Parametros:LogDeOperacao:Operacoes_Tipos-Codigo_Atualizacao_Fornecedor_Integracao");
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