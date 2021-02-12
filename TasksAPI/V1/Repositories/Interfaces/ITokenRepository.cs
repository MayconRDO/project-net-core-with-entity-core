using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TasksAPI.API.Models;

namespace TasksAPI.API.Repositories.Interfaces
{
    public interface ITokenRepository
    {
        void Add(Token token);
        Token Get(string refreshToken);
        void Update(Token token);
    }
}
