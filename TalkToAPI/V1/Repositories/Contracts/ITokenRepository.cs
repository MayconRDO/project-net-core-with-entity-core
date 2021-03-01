using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TalkToAPI.V1.Models;

namespace TalkToAPI.V1.Repositories.Contracts
{
    public interface ITokenRepository
    {
        void Add(Token token);
        Token Get(string refreshToken);
        void Update(Token token);
    }
}
