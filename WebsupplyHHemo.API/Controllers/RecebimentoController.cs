using Microsoft.AspNetCore.Mvc;
using WebsupplyHHemo.API.Dto;

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
        public Object EnviaRecebimento(RecebimentoRequestDto objRequest)
        {
            return new Object();
        }
    }
}
