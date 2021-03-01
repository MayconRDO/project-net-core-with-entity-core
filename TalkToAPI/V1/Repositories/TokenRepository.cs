using System.Linq;
using TalkToAPI.DataBase;
using TalkToAPI.V1.Models;
using TalkToAPI.V1.Repositories.Contracts;

namespace TalkToAPI.V1.Repositories
{
    public class TokenRepository : ITokenRepository
    {
        private readonly TalkToContext _context;
        public TokenRepository(TalkToContext context)
        {
            _context = context;
        }

        public Token Get(string refreshToken)
        {
           return _context.Tokens.FirstOrDefault(t => t.RefreshToken == refreshToken && !t.Used);
        }

        public void Add(Token token)
        {
            _context.Tokens.Add(token);
            _context.SaveChanges();
        }        

        public void Update(Token token)
        {
            _context.Tokens.Update(token);
            _context.SaveChanges();
        }
    }
}
