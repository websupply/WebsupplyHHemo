using Microsoft.AspNetCore.Mvc;

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
        public Object ComplementoItem()
        {
            return new Object();
        }
    }
}
