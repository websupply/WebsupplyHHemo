using Microsoft.AspNetCore.Mvc;
using WebsupplyHHemo.API.Dto;
using WebsupplyHHemo.Interface.Metodos;

namespace WebsupplyHHemo.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InterfacesController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public InterfacesController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet]
        [Route("consome-interface/{interfaceWS}")]
        public ObjectResult ConsomeInterface(string interfaceWS)
        {
            return Ok(interfaceWS);
            // Variaveis de Controle
            //string strMensagem consomeWS;

            // Verifica qual chamada ira realizar, e caso seja
            // uma chamada invalida, devolve erro
            //interfaceWS.ToLower() switch
            //{
            //    "centroscustos" => consomeWS = new CentroCustoMetodo.ConsomeWS()
            //    "centroscustos" => new ObjectResult(new { CentrosCustos = DashboardADO.CARREGA_LISTA_CENTROSCUSTO_DASHBOARD_PROJETOS(_configuration.GetValue<string>("ConnectionStrings:DefaultConnection"), objUser, objDashboardProjetosEventosRequest) }),
            //    "projetoseventos" => new ObjectResult(new { ProjetosEventos = DashboardADO.CARREGA_LISTA_PROJETOS_EVENTOS_DASHBOARD_PROJETOS(_configuration.GetValue<string>("ConnectionStrings:DefaultConnection"), objUser, objDashboardProjetosEventosRequest) }),
            //    "fornecedores" => new ObjectResult(new { Fornecedores = DashboardADO.CARREGA_LISTA_FORNECEDORES_DASHBOARD_PROJETOS(_configuration.GetValue<string>("ConnectionStrings:DefaultConnection"), objUser, objDashboardProjetosEventosRequest, iIdProjetos) }),
            //    "itens" => new ObjectResult(new { Itens = DashboardADO.CARREGA_LISTA_ITENS_DASHBOARD_PROJETOS(_configuration.GetValue<string>("ConnectionStrings:DefaultConnection"), objUser, objDashboardProjetosEventosRequest, iIdProjetos, cCGCF_SEL) }),
            //    _ => new ObjectResult(new { Mensagem = "Tipo de Filtro Incorreto ou Não Parametrizado" }) { StatusCode = 400 }
            //};
            //CentroCustoMetodo centroCusto = new CentroCustoMetodo();

            //if (!centroCusto.ConsomeWS())
            //{
            //    return new ObjectResult(new
            //    {
            //        Mensagem = centroCusto.strMensagem
            //    })
            //    { StatusCode = 500 };
            //}

            //return new ObjectResult(new
            //{
            //    Mensagem = centroCusto.strMensagem
            //})
            //{ StatusCode = 200 };
        }
    }
}
