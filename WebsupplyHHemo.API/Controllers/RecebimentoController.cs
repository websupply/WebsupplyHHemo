using Microsoft.AspNetCore.Mvc;

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
        public Object EnviaRecebimento()
        {
            return new Object();
        }
    }
}
