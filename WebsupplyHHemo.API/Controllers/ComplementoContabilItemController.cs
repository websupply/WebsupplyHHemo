using Microsoft.AspNetCore.Mvc;
using WebsupplyHHemo.API.Dto;

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
        public Object ComplementoItem(ComplementoContabilItemRequestDto objRequest)
        {
            return new Object();
        }
    }
}
