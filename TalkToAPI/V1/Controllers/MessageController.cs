using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TalkToAPI.V1.Models;
using TalkToAPI.V1.Models.DTO;
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
        private readonly IMapper _mapper;
        private readonly IMessageRepository _messageRepository;

        /// <summary>
        /// Construtor
        /// </summary>
        /// <param name="messageRepository">Repositório mensagem</param>
        /// <param name="mapper">Auto Mapper</param>
        public MessageController(IMessageRepository messageRepository, IMapper mapper)
        {
            _messageRepository = messageRepository;
            _mapper = mapper;
        }

        /// <summary>
        /// Obter mensagem
        /// </summary>
        /// <param name="userFrom">Usuário remetente</param>
        /// <param name="userTo">Usuário Destino</param>
        /// <returns></returns>
        [Authorize]
        [HttpGet("{userFrom}/{userTo}", Name = "MessageGet")]
        public ActionResult GetAll(string userFrom, string userTo)
        {
            if (string.IsNullOrEmpty(userFrom) || string.IsNullOrEmpty(userTo) || userFrom == userTo)
            {
                return UnprocessableEntity();
            }

            var messages = _messageRepository.GetAll(userFrom, userTo);

            var listMessages = _mapper.Map<List<Message>, List<MessageDTO>>(messages);

            var list = new ListDTO<MessageDTO>() { List = listMessages };
            list.Links.Add(new LinkDTO("_self", Url.Link("MessageGet", new { userFrom = userFrom, userTo = userTo }), "GET"));

            return Ok(list);
        }

        /// <summary>
        /// Cadastro de mensagem
        /// </summary>
        /// <param name="message">Objeto mensagem</param>
        /// <returns></returns>
        [Authorize]
        [HttpPost("", Name = "MessageAdd")]
        public ActionResult Add([FromBody] Message message)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _messageRepository.Add(message);

                    var messageDTO = _mapper.Map<Message, MessageDTO>(message);
                    messageDTO.Links.Add(new LinkDTO("_self", Url.Link("MessageAdd", null), "POST"));
                    messageDTO.Links.Add(new LinkDTO("_UpdatePartial", Url.Link("UpdatePartial", new { id = messageDTO.Id }), "PATCH"));

                    return Ok(messageDTO);
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

        /// <summary>
        /// Alteração parcial da mensagem
        /// </summary>
        /// <param name="id">Identificador</param>
        /// <param name="jsonPatch">Objeto mensagem</param>
        /// <returns></returns>
        [Authorize]
        [HttpPatch("{id}", Name = "UpdatePartial")]
        public ActionResult UpdatePartial(int id, [FromBody] JsonPatchDocument<Message> jsonPatch)
        {
            if (jsonPatch == null)
            {
                return BadRequest();
            }

            var message = _messageRepository.Get(id);

            jsonPatch.ApplyTo(message);
            message.ModifiedDate = DateTime.Now;

            _messageRepository.Update(message);

            var messageDTO = _mapper.Map<Message, MessageDTO>(message);
            messageDTO.Links.Add(new LinkDTO("_self", Url.Link("UpdatePartial", new { id = messageDTO.Id }), "PATCH"));
            
            return Ok(messageDTO);
        }
    }
}
