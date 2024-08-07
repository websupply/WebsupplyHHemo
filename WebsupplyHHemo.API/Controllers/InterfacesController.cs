using Microsoft.AspNetCore.Mvc;
using WebsupplyHHemo.API.Attributes;
using WebsupplyHHemo.API.Dto;
using WebsupplyHHemo.API.Models;
using WebsupplyHHemo.Interface.Metodos;
using WebsupplyHHemo.Interface.Funcoes;
using Newtonsoft.Json;

namespace WebsupplyHHemo.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InterfacesController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly string _ambiente;
        private static int _transacao;
        private static int _servico;
        private static string _identificador;

        public InterfacesController(IConfiguration configuration)
        {
            _configuration = configuration;
            _ambiente = _configuration.GetValue<string>("Parametros:Ambiente");
        }

        [HttpGet]
        [Route("consome-interface/{interfaceWS}/{codFilial?}")]
        [Servico(21)]
        public ObjectResult ConsomeInterface(string interfaceWS, string? codFilial)
        {
            // Instancia o obj do Log
            Class_Log_Hhemo objLog;

            // Instancia a Model de Log
            LogWebService logWebService;

            // Pega o Atributo de Serviço
            var servicoAttribute = (ServicoAttribute)Attribute.GetCustomAttribute(
                typeof(AutenticacaoController).GetMethod(nameof(ConsomeInterface)),
                typeof(ServicoAttribute));

            // Seta os parametros inicias do Log
            _transacao = 0;
            _servico = servicoAttribute.IDServico;
            _identificador = "ConsomeInterface" + Mod_Gerais.RetornaIdentificador();

            // Instancia a model do Log
            logWebService = new LogWebService()
            {
                Mensagem = "Chamada da API de Consumo de Interfaces iniciada",
                Retorno = new
                {
                    interfaceWS = interfaceWS,
                    codFilial = codFilial
                }
            };

            // Gera Log
            objLog = new Class_Log_Hhemo(_identificador, _transacao, _servico,
                             0, 0, JsonConvert.SerializeObject(logWebService), null, "Chamada a API Rest - Método " + Mod_Gerais.MethodName(),
                             "L", "", "", Mod_Gerais.MethodName());
            objLog.GravaLog();
            objLog = null;

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

            // Instancia a model do Log
            logWebService = new LogWebService()
            {
                Mensagem = "Chamada da API de Consumo de Interfaces Finalizada com sucesso.",
                Retorno = new
                {
                    interfaceWS = interfaceWS,
                    codFilial = codFilial,
                    status = retornoInterface,
                    mensagem = strMensagem
                }
            };

            // Gera Log
            objLog = new Class_Log_Hhemo(_identificador, _transacao, _servico,
                             0, 0, JsonConvert.SerializeObject(logWebService), null, "Chamada a API Rest - Método " + Mod_Gerais.MethodName(),
                             "L", "", "", Mod_Gerais.MethodName());
            objLog.GravaLog();
            objLog = null;

            // Retorno da Chamada da Interface
            return new ObjectResult(new
            {
                Mensagem = strMensagem
            })
            { StatusCode = (retornoInterface ? 200 : 500) };
        }
    }
}
