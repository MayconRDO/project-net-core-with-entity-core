using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TalkToAPI.V1.Models;
using TalkToAPI.V1.Repositories.Contracts;

namespace TalkToAPI.V1.Controllers
{
    /// <summary>
    /// Controlador do objeto Mensagem
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [ApiVersion("1.0")]
    public class MessageController : ControllerBase
    {
        private readonly IMessageRepository _messageRepository;

        /// <summary>
        /// Construtor
        /// </summary>
        /// <param name="messageRepository">Repositório mensagem</param>
        public MessageController(IMessageRepository messageRepository)
        {
            _messageRepository = messageRepository;
        }

        /// <summary>
        /// Obter mensagem
        /// </summary>
        /// <param name="userFrom">Usuário remetente</param>
        /// <param name="userTo">Usuário Destino</param>
        /// <returns></returns>
        [Authorize]
        [HttpGet("{userFrom}/{userTo}")]
        public ActionResult GetAll(string userFrom, string userTo)
        {
            if (string.IsNullOrEmpty(userFrom) || string.IsNullOrEmpty(userTo) || userFrom == userTo)
            {
                return UnprocessableEntity();
            }

            var messages = _messageRepository.GetAll(userFrom, userTo);

            return Ok(messages);
        }

        /// <summary>
        /// Cadastro de mensagem
        /// </summary>
        /// <param name="message">Objeto mensagem</param>
        /// <returns></returns>
        [Authorize]
        [HttpPost("")]
        public ActionResult Add([FromBody] Message message)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _messageRepository.Add(message);
                    return Ok(message);
                }
                catch (Exception ex)
                {
                    return UnprocessableEntity(ex);
                }
            }
            else
            {
                return UnprocessableEntity(ModelState);
            }
        }
    }
}
