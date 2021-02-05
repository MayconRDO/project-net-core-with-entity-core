using Microsoft.AspNetCore.Mvc;

namespace MimicryAPI.V2.Controllers
{
    [ApiController]
    //[Route("api/v{version:apiVersion}/[controller]")]
    [Route("api/[controller]")]
    [ApiVersion("2.0")]
    public class WordsController : ControllerBase
    {
        [HttpGet("", Name = "GetAll")]
        public string Get()
        {
            return "Versão 2.0";
        }
    }
}
