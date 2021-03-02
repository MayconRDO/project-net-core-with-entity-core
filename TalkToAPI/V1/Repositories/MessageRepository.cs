using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TalkToAPI.DataBase;
using TalkToAPI.V1.Models;
using TalkToAPI.V1.Repositories.Contracts;

namespace TalkToAPI.V1.Repositories
{
    /// <summary>
    /// Repositório do objeto mensagem
    /// </summary>
    public class MessageRepository : IMessageRepository
    {
        private readonly TalkToContext _context;

        /// <summary>
        /// Construtor
        /// </summary>
        /// <param name="context"></param>
        public MessageRepository(TalkToContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Obter mensagem por ID
        /// </summary>
        /// <param name="id">Identificador da mensagem</param>
        public Message Get(int id)
        {
            return _context.Messages.Find(id);
        }

        /// <summary>
        /// Obter usuário
        /// </summary>
        /// <param name="userFrom">Usuário remetente</param>
        /// <param name="userTo">Usuário destino</param>
        /// <returns></returns>
        public List<Message> GetAll(string userFrom, string userTo)
        {
            return _context.Messages.Where(m => (m.FromId == userFrom || m.FromId == userTo)
                                             && (m.ToId == userFrom || m.ToId == userTo)).ToList();
        }

        /// <summary>
        /// Cadastrar usuário
        /// </summary>
        /// <param name="message"></param>
        public void Add(Message message)
        {
            _context.Messages.Add(message);
            _context.SaveChanges();
        }

        /// <summary>
        /// Alterar mensagem
        /// </summary>
        /// <param name="message">Objeto Mensagem</param>
        public void Update(Message message)
        {
            _context.Messages.Update(message);
            _context.SaveChanges();
        }        
    }
}
