using Microsoft.AspNetCore.Mvc;

namespace WebsupplyHHemo.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CadastroUnidadesFiliaisController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public CadastroUnidadesFiliaisController(IConfiguration configuration)
        {
            _configuration = configuration;
        }
    }
}
