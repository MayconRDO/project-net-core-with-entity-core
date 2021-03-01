using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TalkToAPI.V1.Models;

namespace TalkToAPI.V1.Repositories.Contracts
{
    public interface IApplicationUserRepository
    {
        ApplicationUser Get(string email, string password);
        ApplicationUser Get(string id);
        void Add(ApplicationUser user, string password);
    }
}
