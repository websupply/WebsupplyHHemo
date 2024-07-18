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
        [Route("consome-interface/{interfaceWS}/{codFilial}")]
        public ObjectResult ConsomeInterface(string interfaceWS, string? codFilial)
        {
            // Variaveis de Controle
            string strMensagem = string.Empty;
            bool retornoInterface = false;

            // Verifica qual chamada ira realizar, e caso seja
            // uma chamada invalida, devolve erro
            switch (interfaceWS.ToLower())
            {
                case "centro-custo":
                    CentroCustoMetodo centroCustoMetodo = new CentroCustoMetodo();

                    centroCustoMetodo.strCodFilial = codFilial;

                    retornoInterface = centroCustoMetodo.ConsomeWS();
                    strMensagem = centroCustoMetodo.strMensagem;
                    break;
                case "condicao-pagamento":
                    CondicaoPagtoMetodo condicaoPagtoMetodo = new CondicaoPagtoMetodo();

                    condicaoPagtoMetodo.strCodFilial = codFilial;

                    retornoInterface = condicaoPagtoMetodo.ConsomeWS();
                    strMensagem = condicaoPagtoMetodo.strMensagem;
                    break;
                case "forma-pagamento":
                    FormaPagtoMetodo formaPagtoMetodo = new FormaPagtoMetodo();

                    formaPagtoMetodo.strCodFilial = codFilial;

                    retornoInterface = formaPagtoMetodo.ConsomeWS();
                    strMensagem = formaPagtoMetodo.strMensagem;
                    break;
                case "natureza":
                    NaturezaMetodo naturezaMetodo = new NaturezaMetodo();

                    naturezaMetodo.strCodFilial = codFilial;

                    retornoInterface = naturezaMetodo.ConsomeWS();
                    strMensagem = naturezaMetodo.strMensagem;
                    break;
                case "plano-conta":
                    PlanoContaMetodo planoContaMetodo = new PlanoContaMetodo();

                    planoContaMetodo.strCodFilial = codFilial;

                    retornoInterface = planoContaMetodo.ConsomeWS();
                    strMensagem = planoContaMetodo.strMensagem;
                    break;
                case "tipo-operacao":
                    TipoOperacaoMetodo tipoOperacaoMetodo = new TipoOperacaoMetodo();

                    tipoOperacaoMetodo.strCodFilial = codFilial;

                    retornoInterface = tipoOperacaoMetodo.ConsomeWS();
                    strMensagem = tipoOperacaoMetodo.strMensagem;
                    break;
                case "unidade-medida":
                    UnidadeMedidaMetodo unidadeMedidaMetodo = new UnidadeMedidaMetodo();

                    unidadeMedidaMetodo.strCodFilial = codFilial;

                    retornoInterface = unidadeMedidaMetodo.ConsomeWS();
                    strMensagem = unidadeMedidaMetodo.strMensagem;
                    break;
                case "unidades-filial":
                    UnidadesFiliaisMetodo unidadesFiliaisMetodo = new UnidadesFiliaisMetodo();

                    retornoInterface = unidadesFiliaisMetodo.ConsomeWS();
                    strMensagem = unidadesFiliaisMetodo.strMensagem;
                    break;
                case "usuarios":
                    UsuarioMetodo usuarioMetodo = new UsuarioMetodo();

                    retornoInterface = usuarioMetodo.ConsomeWS();
                    strMensagem = usuarioMetodo.strMensagem;
                    break;
                case "unidades-usuarios":
                    UsuarioUnidadeMetodo usuarioUnidadeMetodo = new UsuarioUnidadeMetodo();

                    retornoInterface = usuarioUnidadeMetodo.ConsomeWS();
                    strMensagem = usuarioUnidadeMetodo.strMensagem;
                    break;
                default:
                    retornoInterface = false;
                    strMensagem = $"Metodo {interfaceWS.ToLower()} não existe ou não está parametrizado";
                    break;
            }

            // Retorno da Chamada da Interface
            return new ObjectResult(new
            {
                Mensagem = strMensagem
            })
            { StatusCode = (retornoInterface ? 200 : 500) };
        }
    }
}
