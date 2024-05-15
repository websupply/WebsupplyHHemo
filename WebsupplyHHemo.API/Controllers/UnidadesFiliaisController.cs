using Microsoft.AspNetCore.Mvc;

namespace WebsupplyHHemo.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UnidadesFiliaisController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public UnidadesFiliaisController(IConfiguration configuration)
        {
            _configuration = configuration;
        }
    }
}
