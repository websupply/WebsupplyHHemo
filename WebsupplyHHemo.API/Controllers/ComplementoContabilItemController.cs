using Microsoft.AspNetCore.Mvc;
using WebsupplyHHemo.API.Dto;
using WebsupplyHHemo.API.ADO;

namespace WebsupplyHHemo.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
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
            ComplementoContabilItemADO objADO = new ComplementoContabilItemADO();

            if(!objADO.ATUALIZA_DADOS_CONTABEIS_ITEM(_configuration.GetValue<string>("ConnectionStrings:DefaultConnection"), objRequest))
            {
                return new ObjectResult(new
                {
                    Mensagem = objADO.strMensagem
                })
                { StatusCode = 500 };
            }

            return new ObjectResult(new
            {
                Mensagem = objADO.strMensagem
            })
            { StatusCode = 200 };
        }
    }
}
