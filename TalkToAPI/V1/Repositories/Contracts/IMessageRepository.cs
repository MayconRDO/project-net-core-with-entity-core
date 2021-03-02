using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TalkToAPI.V1.Models;

namespace TalkToAPI.V1.Repositories.Contracts
{
    /// <summary>
    /// Interface do objeto Mensagem
    /// </summary>
    public interface IMessageRepository
    {
        /// <summary>
        /// Obter mensagem por ID
        /// </summary>
        /// <param name="id">Identificador da mensagem</param>
        /// <returns></returns>
        Message Get(int id);

        /// <summary>
        /// Obter todas as mensagens
        /// </summary>
        /// <param name="userFrom">Usuário remetente</param>
        /// <param name="userTo">Usuário destino</param>
        /// <returns></returns>
        List<Message> GetAll(string userFrom, string userTo);

        /// <summary>
        /// Cadastro de mensagem
        /// </summary>
        /// <param name="message">Objeto Mensagem</param>
        void Add(Message message);

        /// <summary>
        /// Alterar mensagem
        /// </summary>
        /// <param name="message">Objeto Mensagem</param>
        void Update(Message message);   
    }
}
