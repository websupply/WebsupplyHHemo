using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using WebsupplyHHemo.API.Attributes;
using WebsupplyHHemo.Interface.Funcoes;

namespace WebsupplyHHemo.API.Filters
{
    public class ValidacaoModelFilter : IActionFilter
    {
        private readonly IConfiguration _configuration;

        public ValidacaoModelFilter(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            var actionDescriptor = (Microsoft.AspNetCore.Mvc.Controllers.ControllerActionDescriptor)context.ActionDescriptor;
            var servicoAttribute = actionDescriptor.MethodInfo.GetCustomAttributes(typeof(ServicoAttribute), false).FirstOrDefault() as ServicoAttribute;

            if (servicoAttribute == null)
            {
                context.Result = new BadRequestObjectResult("Serviço indisponível ou não parametrizado.");
                return;
            }

            int _transacao = 0;
            int _servico = servicoAttribute.IDServico;
            string _identificador = "Validacao" + Mod_Gerais.RetornaIdentificador();

            if (!context.ModelState.IsValid)
            {
                var logModel = new
                {
                    Identificador = _identificador,
                    Transacao = _transacao,
                    Servico = _servico,
                    Errors = context.ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList()
                };

                var objLog = new Class_Log_Hhemo(_identificador, _transacao, _servico,
                                    1, -1, JsonConvert.SerializeObject(logModel), null, "Erro de validação",
                                    "E", "", "", Mod_Gerais.MethodName());
                objLog.GravaLog();

                context.Result = new BadRequestObjectResult(context.ModelState);
            }
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            // Não implementado
        }
    }
}
