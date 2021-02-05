using Microsoft.AspNetCore.Mvc;

namespace MimicryAPI.V2.Controllers
{
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("2.0")]
    public class WordsController : ControllerBase
    {
        /// <summary>
        /// Obter todas as palavras.
        /// </summary>
        /// <returns>Listagem de palavras</returns>
        [HttpGet("", Name = "GetAll")]
        public string Get()
        {
            return "Versão 2.0";
        }
    }
}
