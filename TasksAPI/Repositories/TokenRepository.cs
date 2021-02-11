using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TasksAPI.DataBase;
using TasksAPI.Models;
using TasksAPI.Repositories.Interfaces;

namespace TasksAPI.Repositories
{
    public class TokenRepository : ITokenRepository
    {
        private readonly TasksContext _context;
        public TokenRepository(TasksContext context)
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
